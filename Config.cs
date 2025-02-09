using System;

public class AppConfig
{
    public MqttConfig Mqtt { get; set; }
    public SerialPortConfig SerialPort { get; set; }
    // Add any other configuration sections you need.
}

public class MqttConfig
{
    public string BrokerAddress { get; set; }
    public int BrokerPort { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
}

public class SerialPortConfig
{
    public string PortName { get; set; }
    public int BaudRate { get; set; }
}
