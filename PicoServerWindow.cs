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
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;



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

            AppConfig config = ConfigLoader.LoadConfig("config.json");


            //start serial port
            monitor = new Monitor(richTextBox1,config);

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
            //if shutdown reason, send SLEEP command
            if(e.CloseReason == CloseReason.WindowsShutDown)            
            {
                monitor.send("SLEEP");
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

        

async void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Suspend:
                    // The computer is entering sleep mode
                    monitor.send("SLEEP");
                    break;
                case PowerModes.Resume:
                    // The computer is waking up from sleep mode
                    await Task.Run(async () => await monitor.ReconnectMqttClient());
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

          

            //if clicked off, do nothing
            if (!rgb_single_color.Checked)
            {
                return;
            }

            //show color dialog
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                //get color
                Color c = colorDialog1.Color;
                //convert colour to rgb565 format

                // Convert the color to RGB565 format
                int r = (c.R >> 3) & 0x1F;
                int g = (c.G >> 2) & 0x3F;
                int b = (c.B >> 3) & 0x1F;

                // Combine the RGB components into a single 16-bit value
                int rgb565 = (r << 11) | (g << 5) | b;

                // Convert the RGB565 value to a hex string
                string hexString = rgb565.ToString("X4");


                monitor.send("COLOR_MODE:SINGLE_COLOR;COLOR:" + hexString);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            monitor.send("COLOR_MODE:RAINBOW_SNAKE");
        }

        private void rgb_none_CheckedChanged(object sender, EventArgs e)
        {
            monitor.send("COLOR_MODE:NONE");
        }

        private void rgb_rainbow_line_CheckedChanged(object sender, EventArgs e)
        {
            monitor.send("COLOR_MODE:RAINBOW_LINE");
        }

        private void PicoServerWindow_Load(object sender, EventArgs e)
        {

        }

        private void sleep_button_Click(object sender, EventArgs e)
        {
            monitor.send("SLEEP");
        }

        private void awake_button_Click(object sender, EventArgs e)
        {
            monitor.send("WAKEUP");
        }
    }

    //class that starts a serial port COM8 with baud 115200
    //it also polls hardware and sends over serial port
    //it uses librehardwarelib to poll hardware
    //it uses serialport to send over serial
    //it uses timer to poll hardware and send over serial
    class Monitor
    {


        private IMqttClient _mqttClient;
        private AppConfig _config;

        //constructor
        public Monitor(RichTextBox t1, AppConfig config)
        {
            this.textBox_info = t1;

            //start serial port
            serialPort = new System.IO.Ports.SerialPort(config.SerialPort.PortName, config.SerialPort.BaudRate);
            serialPort.Open(); 
            
            //send WAKEUP command
            send("WAKEUP");
            
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
                IsMemoryEnabled = true,
                IsPsuEnabled = true,
                IsNetworkEnabled = true
            };

            computer.Open();
            computer.Accept(new UpdateVisitor());

            _config = config;

            InitialiseMqttClient();

        }

        private async void InitialiseMqttClient()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId(_config.Mqtt.ClientId)
                .WithTcpServer(_config.Mqtt.BrokerAddress, _config.Mqtt.BrokerPort)
                .WithCredentials(_config.Mqtt.Username, _config.Mqtt.Password)
                .WithCleanSession()
                .Build();

            await _mqttClient.ConnectAsync(options,System.Threading.CancellationToken.None);

            //send configure message:
            /*{
            "device_class": "power",
            "unique_id": "your-unique-id",
            "object_id": "THE-BEAST_PSUWattage",
            "unit_of_measurement": "W",
            "availability_topic": "homeassistant/sensor/THE-BEAST/availability",
            "device": {
            "identifiers": "hass.agent-THE-BEAST",
            "manufacturer": "HASS.Agent Team",
            "model": "Microsoft Windows NT",
            "sw_version": "2.0.1",
            "name": "THE-BEAST"
            },
            "state_topic": "homeassistant/sensor/THE-BEAST/PSUWattage/state",
            "name": "PSUWattage",
            "platform": "mqtt"
            }
            */
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("homeassistant/sensor/THE-BEAST/PSUWattage/config")
                .WithPayload("{\"device_class\": \"power\",\"unique_id\": \"your-unique-id\",\"object_id\": \"THE-BEAST_PSUWattage\",\"unit_of_measurement\": \"W\",\"availability_topic\": \"homeassistant/sensor/THE-BEAST/availability\",\"device\": {\"identifiers\": \"hass.agent-THE-BEAST\",\"manufacturer\": \"HASS.Agent Team\",\"model\": \"Microsoft Windows NT\",\"sw_version\": \"2.0.1\",\"name\": \"THE-BEAST\"},\"state_topic\": \"homeassistant/sensor/THE-BEAST/PSUWattage/state\",\"name\": \"PSUWattage\",\"platform\": \"mqtt\"}")
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);


        }

//function to send a string over serial with \n at the end
public void send(string s)
{
    serialPort.Write(s + "\n");
}


private bool everyTwoSeconds = false;
//private float currentWattage = 0;
private float? previousWattage = null;

private void ProcessWattage(float? wattage)
{
    if (wattage == null || wattage == 0)
    {
        if (previousWattage != null)
        {
            wattage = (float)previousWattage;
        }
        else
        {
            return;
        }
    }

    previousWattage = wattage;

    SendWattageToMQTT(wattage);

}

        public async Task ReconnectMqttClient()
        {
            while (!_mqttClient.IsConnected)
            {
                try
                {
                    await _mqttClient.ConnectAsync(_mqttClient.Options, System.Threading.CancellationToken.None);
                    Console.WriteLine("MQTT client reconnected.");
                }
                catch
                {
                    Console.WriteLine("MQTT reconnection failed. Retrying...");
                    await Task.Delay(5000); // Retry every 5 seconds
                }
            }

        }


        private async void SendWattageToMQTT(float? wattage)
        {
            try
            {
                if (!_mqttClient.IsConnected)
                {
                    Console.WriteLine("MQTT client not connected. Attempting to reconnect...");
                    await ReconnectMqttClient();
                }

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("homeassistant/sensor/THE-BEAST/PSUWattage/state")
                    .WithPayload(wattage.ToString())
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                    .WithRetainFlag()
                    .Build();

                await _mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
            }
            catch (MQTTnet.Exceptions.MqttClientNotConnectedException ex)
            {
                Console.WriteLine("MQTT client is not connected: " + ex.Message);
            }
            catch (MQTTnet.Client.MqttClientDisconnectedException ex)
            {
                Console.WriteLine("MQTT client disconnected: " + ex.Message);
                await ReconnectMqttClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred while publishing: " + ex.Message);
            }
        }



        //poll hardware and send over serial
        public async void pollHardwareAndSendOverSerial(object sender, EventArgs e)
        {

            string cpuTemp = "";
            string gpuTemp = "";
            string ramUsage = "";
            string cpuUsage = "";
            string vramUsage = "";
            string downloadSpeed = "";
            string uploadSpeed = "";

            //poll hardware

            if (everyTwoSeconds)
            {
                everyTwoSeconds = false;
            }
            else
            {
                everyTwoSeconds = true;
            }

            foreach (IHardware hardware in computer.Hardware)
            {

                //skip if not cpu or gpu or ram
                //
                if (hardware.HardwareType != HardwareType.Cpu && hardware.HardwareType != HardwareType.GpuNvidia && hardware.HardwareType != HardwareType.Memory && hardware.HardwareType != HardwareType.Psu && hardware.HardwareType != HardwareType.Network)
                {
                    continue;
                }
                //Console.WriteLine("Hardware: {0}", hardware.Name);

                //check if psu
                if (hardware.HardwareType == HardwareType.Psu)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        //only print if it's power
                        if (sensor.Name != "Total watts" && sensor.Name != "Total Output")
                        {
                            continue;
                        }

                        if (everyTwoSeconds)
                        {

                            //Console.WriteLine("\t\tSENSOR WATTAGE: {0}, value: {1}", sensor.Name, sensor.Value);

                            ProcessWattage(sensor.Value);


                        }

                    }
                }

                ////if it's hardware type netwrok and the name is Ethernet
                //if (hardware.HardwareType == HardwareType.Network && hardware.Name == "Ethernet")
                //{
                //    foreach (ISensor sensor in hardware.Sensors)
                //    {
                //        //only print if it's download or upload speed
                //        if (sensor.Name != "Download Speed" && sensor.Name != "Upload Speed")
                //        {
                //            continue;
                //        }
                //        //Console.WriteLine("\t\tSENSOR: {0}, value: {1}", sensor.Name, sensor.Value);
                //        if (sensor.Name == "Download Speed")
                //        {
                //            downloadSpeed = sensor.Value.ToString();
                //        }
                //        if (sensor.Name == "Upload Speed")
                //        {
                //            uploadSpeed = sensor.Value.ToString();
                //        }
                //    }
                //}



                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        //skip if not temperature

                        if (sensor.SensorType != SensorType.Temperature)
                        {
                            //continue;
                        }

                        //Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {

                    //only print if it's the package temperature, CPU Total, Ram Usage, or GPU temperature
                    if (sensor.Name != "CPU Package" && sensor.Name != "CPU Total" && sensor.Name != "GPU Core" && sensor.Name != "Memory Used" && sensor.Name != "GPU Memory Used" && sensor.Name != "Download Speed" && sensor.Name != "Upload Speed")
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

                    if (hardware.HardwareType == HardwareType.GpuNvidia && sensor.SensorType == SensorType.SmallData)
                    {
                        vramUsage = sensor.Value.ToString();
                    }
                    

                    //check if it's ram and if it's usage and save it
                    if (hardware.HardwareType == HardwareType.Memory && sensor.SensorType == SensorType.Data)
                    {
                        ramUsage = sensor.Value.ToString();
                    }

                    //check if it's network and if it's download speed and save it
                    if (hardware.HardwareType == HardwareType.Network && sensor.SensorType == SensorType.Throughput && hardware.Name == "Ethernet" && sensor.Name == "Download Speed")
                    {

                        downloadSpeed = sensor.Value.ToString();

                    }

                    //check if it's network and if it's upload speed and save it
                    if (hardware.HardwareType == HardwareType.Network && sensor.SensorType == SensorType.Throughput && hardware.Name == "Ethernet" && sensor.Name == "Upload Speed")
                    {
                        
                        uploadSpeed = sensor.Value.ToString();
                    }

                    //print the sensor value
                    //Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);



                }
            }

            //go through each hardware and update it
            foreach (IHardware hardware in computer.Hardware)
            {
                await Task.Run(() => hardware.Update());
            }

            //update ram usage to be a percentage instead of a number, max is 96GB
            double ramUsagePercent = (Convert.ToDouble(ramUsage) / 96) * 100;

            //update vram usage to be a percentage instead of a number, max is 24564
            double vramUsagePercent = (Convert.ToDouble(vramUsage) / 24564) * 100;

            // Convert the download speed (given in B/s) to bits per second.
            double dsBytes = Convert.ToDouble(downloadSpeed);
            double dsBps = dsBytes * 8;
            double dsKbps = dsBps / 1000.0;

            // Use kbps if the speed is less than 100 kbps; otherwise, convert to Mbps.
            if (dsKbps < 100)
            {
                // Use no decimal places for kbps.
                downloadSpeed = dsKbps.ToString("F1") + " kbps";
            }
            else
            {
                // For speeds 100 kbps and above, convert to Mbps with one decimal place.
                downloadSpeed = (dsBps / 1000000.0).ToString("F1") + " mbps";
            }

            // Do the same for the upload speed.
            double usBytes = Convert.ToDouble(uploadSpeed);
            double usBps = usBytes * 8;
            double usKbps = usBps / 1000.0;

            if (usKbps < 100)
            {
                uploadSpeed = usKbps.ToString("F1") + " kbps";
            }
            else
            {
                uploadSpeed = (usBps / 1000000.0).ToString("F1") + " mbps";
            }


            //send to port like CPU:TEMP:USAGE;GPU:TEMP:VRAM:USAGE;RAM:USAGE; use the rounded values
            serialPort.Write("CPU:" + Math.Round(Convert.ToDouble(cpuTemp)) + ":" + Math.Round(Convert.ToDouble(cpuUsage)) + ";GPU:" + Math.Round(Convert.ToDouble(gpuTemp)) + ";VMEM:" + Math.Round(vramUsagePercent) + ";RAM:" + Math.Round(ramUsagePercent) + ";DS:" + downloadSpeed + ";US:" + uploadSpeed + "\n");
            //write to t1 what was sent
            textBox_info.Text = "CPU:" + Math.Round(Convert.ToDouble(cpuTemp)) + ":" + Math.Round(Convert.ToDouble(cpuUsage)) + ";GPU:" + Math.Round(Convert.ToDouble(gpuTemp)) + ";VMEM:" + Math.Round(vramUsagePercent) + ";RAM:" + Math.Round(ramUsagePercent) + ";DS:" + downloadSpeed + ";US:" + uploadSpeed + "\n";

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
