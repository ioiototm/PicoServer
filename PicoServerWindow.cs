using LibreHardwareMonitor.Hardware;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicoServer
{
    public partial class PicoServerWindow : Form
    {

        //monitor
        private Monitor monitor;
        private NotifyIcon trayIcon;

        private bool exitButtonClicked = false;

        public PicoServerWindow()
        {
            InitializeComponent();

            //start serial port
            monitor = new Monitor(richTextBox1);

            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            trayIcon = new NotifyIcon();
            trayIcon.Text = "PicoServer";
            //use the same icon as the form
            trayIcon.Icon = this.Icon;

            trayIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                }
            };
            
        }


        private void PicoServerWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!exitButtonClicked)
            {
                //hide the form
                e.Cancel = true;
                this.Hide();
                trayIcon.Visible = true;
            }
        }
        

        //send button onclick
        private void button1_Click(object sender, EventArgs e)
        {
            monitor.send(txtBox_command.Text);
        }

        
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                // The session is locked
                monitor.send("LOCK");
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                // The session is unlocked
                monitor.send("UNLOCK");
            }
        }

        

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Suspend:
                    // The computer is entering sleep mode
                    monitor.send("SLEEP");
                    break;
                case PowerModes.Resume:
                    // The computer is waking up from sleep mode
                    monitor.send("WAKEUP");
                    break;
                default:
                    // Unknown power mode
                    break;
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            exitButtonClicked = true;
            Application.Exit();
        }
    }

    //class that starts a serial port COM8 with baud 115200
    //it also polls hardware and sends over serial port
    //it uses librehardwarelib to poll hardware
    //it uses serialport to send over serial
    //it uses timer to poll hardware and send over serial
    class Monitor
    {


        //constructor
        public Monitor(RichTextBox t1)
        {
            this.textBox_info = t1;

            //start serial port
            serialPort = new System.IO.Ports.SerialPort("COM8", 115200);
            serialPort.Open();  
            
            //start timer
            pollingTimer = new Timer();
            pollingTimer.Interval = 1000;
            pollingTimer.Tick += pollHardwareAndSendOverSerial;
            pollingTimer.Start();

            //init computer
            computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true
            };

            computer.Open();
            computer.Accept(new UpdateVisitor());


        }

        //function to send a string over serial with \n at the end
        public void send(string s)
        {
            serialPort.Write(s + "\n");
        }


      

        //poll hardware and send over serial
        public async void pollHardwareAndSendOverSerial(object sender, EventArgs e)
        {

            string cpuTemp = "";
            string gpuTemp = "";
            string ramUsage = "";
            string cpuUsage = "";

            //poll hardware

            foreach (IHardware hardware in computer.Hardware)
            {

                //skip if not cpu or gpu or ram
                //
                if (hardware.HardwareType != HardwareType.Cpu && hardware.HardwareType != HardwareType.GpuNvidia && hardware.HardwareType != HardwareType.Memory)
                {
                    continue;
                }
                //Console.WriteLine("Hardware: {0}", hardware.Name);

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    //Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        //skip if not temperature

                        if (sensor.SensorType != SensorType.Temperature)
                        {
                            continue;
                        }

                        //Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {
                
                    //only print if it's the package temperature, CPU Total, Ram Usage, or GPU temperature
                    if (sensor.Name != "CPU Package" && sensor.Name != "CPU Total" && sensor.Name != "GPU Core" && sensor.Name != "Memory Used")
                    {
                        continue;
                    }

                    //check if it's cpu and if it's temperature and save it
                    if (hardware.HardwareType == HardwareType.Cpu && sensor.SensorType == SensorType.Temperature)
                    {
                        cpuTemp = sensor.Value.ToString();
                    }

                    //check if it's cpu and if it's usage/load and save it
                    if (hardware.HardwareType == HardwareType.Cpu && sensor.SensorType == SensorType.Load)
                    {
                        cpuUsage = sensor.Value.ToString();
                    }

                    //check if it's gpu and if it's temperature and save it
                    if (hardware.HardwareType == HardwareType.GpuNvidia && sensor.SensorType == SensorType.Temperature)
                    {
                        gpuTemp = sensor.Value.ToString();
                    }

                    //check if it's ram and if it's usage and save it
                    if (hardware.HardwareType == HardwareType.Memory && sensor.SensorType == SensorType.Data)
                    {
                        ramUsage = sensor.Value.ToString();
                    }

                }
            }

            //go through each hardware and update it
            foreach (IHardware hardware in computer.Hardware)
            {
                await Task.Run(() => hardware.Update());
            }

            //update ram usage to be a percentage instead of a number, max is 32GB
            double ramUsagePercent = (Convert.ToDouble(ramUsage) / 32) * 100;
           
            //send to port like CPU:TEMP:USAGE;GPU:TEMP;RAM:USAGE; use the rounded values
            serialPort.Write("CPU:" + Math.Round(Convert.ToDouble(cpuTemp)) + ":" + Math.Round(Convert.ToDouble(cpuUsage)) + ";GPU:" + Math.Round(Convert.ToDouble(gpuTemp)) + ";RAM:" + Math.Round(ramUsagePercent)+"\n");
            //write to t1 what was sent
            textBox_info.Text = "CPU:" + Math.Round(Convert.ToDouble(cpuTemp)) + ":" + Math.Round(Convert.ToDouble(cpuUsage)) + ";GPU:" + Math.Round(Convert.ToDouble(gpuTemp)) + ";RAM:" + Math.Round(ramUsagePercent)+"\n";

        }

        //variables
        private Timer pollingTimer;
        private System.IO.Ports.SerialPort serialPort;
        private Computer computer;
        private RichTextBox textBox_info;

    }



    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor)
        {
            //refresh the value
            sensor.Hardware.Update();





        }
        public void VisitParameter(IParameter parameter)
        {
            //refresh the value
            parameter.Sensor.Hardware.Update();
        }
    }
}
