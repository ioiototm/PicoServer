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
        private ComboBox gpuSelectionComboBox; // Added for GPU selection

        private bool exitButtonClicked = false;


        public PicoServerWindow()
        {
            InitializeComponent();

            AppConfig config = ConfigLoader.LoadConfig("config.json");


            //start serial port
            monitor = new Monitor(richTextBox1, config);

            // Initialize and populate GPU selection ComboBox
            InitializeGpuSelectionComboBox();


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
                    // Ensure the ComboBox is brought to front if it was hidden
                    gpuSelectionComboBox.BringToFront();
                }
            };

        }

        private void InitializeGpuSelectionComboBox()
        {
            gpuSelectionComboBox = new ComboBox();
            gpuSelectionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            gpuSelectionComboBox.Location = new System.Drawing.Point(15, 280); // Adjust location as needed
            gpuSelectionComboBox.Size = new System.Drawing.Size(200, 21); // Adjust size as needed
            gpuSelectionComboBox.Name = "gpuSelectionComboBox";

            List<string> gpuNames = monitor.GetGpuNames();
            if (gpuNames.Any())
            {
                gpuSelectionComboBox.Items.AddRange(gpuNames.ToArray());
                // Try to select preferred GPU from config, otherwise select the first one
                string preferredGpu = monitor.GetSelectedGpuName();
                if (!string.IsNullOrEmpty(preferredGpu) && gpuNames.Contains(preferredGpu))
                {
                    gpuSelectionComboBox.SelectedItem = preferredGpu;
                }
                else
                {
                    gpuSelectionComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                gpuSelectionComboBox.Items.Add("No GPUs Found");
                gpuSelectionComboBox.SelectedIndex = 0;
                gpuSelectionComboBox.Enabled = false;
            }

            gpuSelectionComboBox.SelectedIndexChanged += GpuSelectionComboBox_SelectedIndexChanged;
            this.Controls.Add(gpuSelectionComboBox);
            gpuSelectionComboBox.BringToFront(); // Ensure it's visible
        }

        private void GpuSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gpuSelectionComboBox.SelectedItem != null && gpuSelectionComboBox.Enabled)
            {
                monitor.SetSelectedGpu(gpuSelectionComboBox.SelectedItem.ToString());
            }
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
            //if shutdown reason, send SLEEP command and mark offline
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                monitor.send("SLEEP");
                monitor.PublishOfflineAsync().GetAwaiter().GetResult();
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
                    await monitor.PublishOfflineAsync();
                    break;
                case PowerModes.Resume:
                    // The computer is waking up from sleep mode
                    await Task.Delay(2000); // Give system time to stabilize after wake-up
                    await monitor.ReconnectMqttClient();
                    monitor.ReInitializeHardware(); // Add this new method call
                    monitor.send("WAKEUP");
                    break;
                default:
                    // Unknown power mode
                    break;
            }
        }

        private async void btn_exit_Click(object sender, EventArgs e)
        {
            exitButtonClicked = true;
            await monitor.PublishOfflineAsync();
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
            if (colorDialog1.ShowDialog() == DialogResult.OK)
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
        private List<IHardware> _gpus; // Added to store detected GPUs
        private IHardware _selectedGpu; // Added to store the selected GPU
        private string _preferredGpuName; // Added to store preferred GPU name from config

        //constructor
        public Monitor(RichTextBox t1, AppConfig config)
        {
            this.textBox_info = t1;
            _config = config; // Store config
            _preferredGpuName = _config.PreferredGpuName; // Get preferred GPU name

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

            // Initialize GPU list and select default
            InitializeGpus();


            InitialiseMqttClient();

        }

        private void InitializeGpus()
        {
            _gpus = new List<IHardware>();
            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd)
                {
                    _gpus.Add(hardware);
                }
            }

            if (_gpus.Any())
            {
                // Try to select preferred GPU
                if (!string.IsNullOrEmpty(_preferredGpuName))
                {
                    _selectedGpu = _gpus.FirstOrDefault(g => g.Name.Equals(_preferredGpuName, StringComparison.OrdinalIgnoreCase));
                }
                // If preferred GPU not found or not set, select the first one
                if (_selectedGpu == null)
                {
                    _selectedGpu = _gpus.First();
                }
            }
        }

        public List<string> GetGpuNames()
        {
            return _gpus.Select(g => g.Name).ToList();
        }

        public string GetSelectedGpuName()
        {
            return _selectedGpu?.Name;
        }

        public void SetSelectedGpu(string gpuName)
        {
            _selectedGpu = _gpus.FirstOrDefault(g => g.Name.Equals(gpuName, StringComparison.OrdinalIgnoreCase));
            // Optionally, save this preference back to config if desired
            // _config.PreferredGpuName = gpuName;
            // ConfigLoader.SaveConfig("config.json", _config);
        }

        private async void InitialiseMqttClient()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId(_config.Mqtt.ClientId)
                .WithTcpServer(_config.Mqtt.BrokerAddress, _config.Mqtt.BrokerPort)
                .WithCredentials(_config.Mqtt.Username, _config.Mqtt.Password)
                .WithWillTopic("homeassistant/sensor/THE-BEAST/availability")
                .WithWillPayload("offline")
                .WithWillRetain(true)
                .WithCleanSession()
                .Build();

            await _mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None);

            await PublishAvailability("online");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("homeassistant/sensor/THE-BEAST/PSUWattage/config")
                .WithPayload("{\"device_class\": \"power\",\"state_class\": \"measurement\",\"unique_id\": \"your-unique-id\",\"object_id\": \"THE-BEAST_PSUWattage\",\"unit_of_measurement\": \"W\",\"availability_topic\": \"homeassistant/sensor/THE-BEAST/availability\",\"device\": {\"identifiers\": \"hass.agent-THE-BEAST\",\"manufacturer\": \"HASS.Agent Team\",\"model\": \"Microsoft Windows NT\",\"sw_version\": \"2.0.1\",\"name\": \"THE-BEAST\"},\"state_topic\": \"homeassistant/sensor/THE-BEAST/PSUWattage/state\",\"name\": \"PSUWattage\",\"platform\": \"mqtt\"}")
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
        }

        private async Task PublishAvailability(string status)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("homeassistant/sensor/THE-BEAST/availability")
                .WithPayload(status)
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
        }

        //function to send a string over serial with \n at the end
        public void send(string s)
        {
            serialPort.Write(s + "\n");
        }

        public async Task PublishOfflineAsync()
        {
            if (_mqttClient != null && _mqttClient.IsConnected)
                await PublishAvailability("offline");
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
            int retryCount = 0;
            const int maxRetries = 5;
            
            while (!_mqttClient.IsConnected && retryCount < maxRetries)
            {
                try
                {
                    retryCount++;
                    textBox_info.Invoke((MethodInvoker)delegate {
                        textBox_info.AppendText($"Attempting MQTT reconnection (try {retryCount}/{maxRetries})...\n");
                    });
                    
                    var options = new MqttClientOptionsBuilder()
                        .WithClientId(_config.Mqtt.ClientId)
                        .WithTcpServer(_config.Mqtt.BrokerAddress, _config.Mqtt.BrokerPort)
                        .WithCredentials(_config.Mqtt.Username, _config.Mqtt.Password)
                        .WithWillTopic("homeassistant/sensor/THE-BEAST/availability")
                        .WithWillPayload("offline")
                        .WithWillRetain(true)
                        .WithCleanSession()
                        .Build();

                    await _mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None);

                    await PublishAvailability("online");

                    textBox_info.Invoke((MethodInvoker)delegate {
                        textBox_info.AppendText("MQTT client reconnected successfully\n");
                    });

                    // Re-publish configuration message
                    var message = new MqttApplicationMessageBuilder()
                        .WithTopic("homeassistant/sensor/THE-BEAST/PSUWattage/config")
                        .WithPayload("{\"device_class\": \"power\",\"state_class\": \"measurement\",\"unique_id\": \"your-unique-id\",\"object_id\": \"THE-BEAST_PSUWattage\",\"unit_of_measurement\": \"W\",\"availability_topic\": \"homeassistant/sensor/THE-BEAST/availability\",\"device\": {\"identifiers\": \"hass.agent-THE-BEAST\",\"manufacturer\": \"HASS.Agent Team\",\"model\": \"Microsoft Windows NT\",\"sw_version\": \"2.0.1\",\"name\": \"THE-BEAST\"},\"state_topic\": \"homeassistant/sensor/THE-BEAST/PSUWattage/state\",\"name\": \"PSUWattage\",\"platform\": \"mqtt\"}")
                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                        .WithRetainFlag()
                        .Build();

                    await _mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);

                    return;
                }
                catch (Exception ex)
                {
                    textBox_info.Invoke((MethodInvoker)delegate {
                        textBox_info.AppendText($"MQTT reconnection attempt failed: {ex.Message}\n");
                    });
                    await Task.Delay(5000); // Retry every 5 seconds
                }
            }

            if (!_mqttClient.IsConnected)
            {
                textBox_info.Invoke((MethodInvoker)delegate {
                    textBox_info.AppendText("Failed to reconnect MQTT after maximum retries\n");
                });
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
                    .WithPayload(wattage.Value.ToString(System.Globalization.CultureInfo.InvariantCulture))
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
            // Stop the timer so a slow poll can't overlap with the next tick
            pollingTimer.Stop();
            try
            {
                // Update all hardware in parallel FIRST so the sensor reads below are fresh this tick,
                // not stale from the previous cycle
                await Task.WhenAll(computer.Hardware.Select(h => Task.Run(() => h.Update())));

                // Store sensor values as doubles directly — avoids string→double round-trips
                // and the locale bug where sensor.Value.ToString() could produce "123,4" on some systems
                double cpuTemp = 0;
                double gpuTemp = 0;
                double ramUsage = 0;
                double cpuUsage = 0;
                double vramUsage = 0;
                string downloadSpeed = "0";
                string uploadSpeed = "0";

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
                    if (hardware.HardwareType != HardwareType.Cpu &&
                        hardware.HardwareType != HardwareType.Memory &&
                        hardware.HardwareType != HardwareType.Psu &&
                        hardware.HardwareType != HardwareType.Network)
                    {
                        if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd)
                        {
                            if (_selectedGpu == null || hardware.Identifier != _selectedGpu.Identifier)
                                continue;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (hardware.HardwareType == HardwareType.Psu)
                    {
                        foreach (ISensor sensor in hardware.Sensors)
                        {
                            if (sensor.Name != "Total watts" && sensor.Name != "Total Output")
                                continue;

                            if (everyTwoSeconds)
                                ProcessWattage(sensor.Value);
                        }
                    }

                    foreach (IHardware subhardware in hardware.SubHardware)
                    {
                        Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                        foreach (ISensor sensor in subhardware.Sensors)
                        {
                            if (sensor.SensorType != SensorType.Temperature)
                            {
                                //continue;
                            }
                        }
                    }

                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.Name != "CPU Package" && sensor.Name != "CPU Total" && sensor.Name != "Memory Used" && sensor.Name != "Download Speed" && sensor.Name != "Upload Speed")
                        {
                            if (!(hardware == _selectedGpu && (sensor.Name == "GPU Core" || sensor.Name == "GPU Memory Used")))
                                continue;
                        }

                        // sensor.Value is float? — using ?? 0 handles null safely without any string conversion
                        if (hardware.HardwareType == HardwareType.Cpu && sensor.SensorType == SensorType.Temperature)
                            cpuTemp = sensor.Value ?? 0;

                        if (hardware.HardwareType == HardwareType.Cpu && sensor.SensorType == SensorType.Load)
                            cpuUsage = sensor.Value ?? 0;

                        if (hardware.HardwareType == HardwareType.Memory && sensor.SensorType == SensorType.Data)
                            ramUsage = sensor.Value ?? 0;

                        // Network speeds stay as strings since they get overwritten with a formatted value later.
                        // InvariantCulture ensures "123.4" not "123,4" regardless of system locale
                        if (hardware.HardwareType == HardwareType.Network && sensor.SensorType == SensorType.Throughput && hardware.Name == "Ethernet" && sensor.Name == "Download Speed")
                            downloadSpeed = sensor.Value?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "0";

                        if (hardware.HardwareType == HardwareType.Network && sensor.SensorType == SensorType.Throughput && hardware.Name == "Ethernet" && sensor.Name == "Upload Speed")
                            uploadSpeed = sensor.Value?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "0";
                    }
                }

                // GPU sensors — no separate Update() call needed, already updated at the top
                if (_selectedGpu != null)
                {
                    foreach (ISensor sensor in _selectedGpu.Sensors)
                    {
                        if (sensor.Name == "GPU Core" && sensor.SensorType == SensorType.Temperature)
                            gpuTemp = sensor.Value ?? 0;
                        else if (sensor.Name == "GPU Memory Used" && sensor.SensorType == SensorType.SmallData)
                            vramUsage = sensor.Value ?? 0;
                    }
                }

                double ramUsagePercent = (ramUsage / 128) * 100;
                double vramUsagePercent = (vramUsage / 24564) * 100;

                // TryParse with InvariantCulture safely handles the locale issue and defaults to 0 on failure
                double.TryParse(downloadSpeed, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double dsBytes);
                double dsBps = dsBytes * 8;
                double dsKbps = dsBps / 1000.0;

                if (dsKbps < 100)
                    downloadSpeed = dsKbps.ToString("F1") + " kbps";
                else
                    downloadSpeed = (dsBps / 1000000.0).ToString("F1") + " mbps";

                double.TryParse(uploadSpeed, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double usBytes);
                double usBps = usBytes * 8;
                double usKbps = usBps / 1000.0;

                if (usKbps < 100)
                    uploadSpeed = usKbps.ToString("F1") + " kbps";
                else
                    uploadSpeed = (usBps / 1000000.0).ToString("F1") + " mbps";

                // Values are already doubles now — no Convert.ToDouble needed
                serialPort.Write("CPU:" + Math.Round(cpuTemp) + ":" + Math.Round(cpuUsage) + ";GPU:" + Math.Round(gpuTemp) + ";VMEM:" + Math.Round(vramUsagePercent) + ";RAM:" + Math.Round(ramUsagePercent) + ";DS:" + downloadSpeed + ";US:" + uploadSpeed + "\n");
                textBox_info.Text = "CPU:" + Math.Round(cpuTemp) + ":" + Math.Round(cpuUsage) + ";GPU:" + Math.Round(gpuTemp) + ";VMEM:" + Math.Round(vramUsagePercent) + ";RAM:" + Math.Round(ramUsagePercent) + ";DS:" + downloadSpeed + ";US:" + uploadSpeed + "\n";
            }
            finally
            {
                // Always restart the timer, even if something threw — next tick starts after this poll fully finishes
                pollingTimer.Start();
            }
        }

        public void ReInitializeHardware()
        {
            try
            {
                // Restart hardware monitoring
                computer.Close();
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

                // Re-initialize GPU list and selection
                InitializeGpus();

                // Force a hardware update cycle
                foreach (IHardware hardware in computer.Hardware)
                {
                    hardware.Update();
                }

                // Reset previous wattage to force a new reading
                previousWattage = null;

                textBox_info.Invoke((MethodInvoker)delegate {
                    textBox_info.AppendText("Hardware monitoring reinitialized after wake-up\n");
                });
            }
            catch (Exception ex)
            {
                textBox_info.Invoke((MethodInvoker)delegate {
                    textBox_info.AppendText("Error reinitializing hardware: " + ex.Message + "\n");
                });
            }
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
            //sensor.Hardware.Update();





        }
        public void VisitParameter(IParameter parameter)
        {
            //refresh the value
            //parameter.Sensor.Hardware.Update();
        }
    }
}
