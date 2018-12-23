using System;
using System.Collections.Generic;
using Gtk;
using RobotInterface;

public partial class MainWindow : Gtk.Window
{

    #region FIELDS

    private Robot robot = new Robot(
            new Servo(10, 170, 90),
            new Servo(10, 170, 90),
            new Servo(10, 170, 90),
            new Servo(10, 170, 90),
            new Servo(10, 170, 90),
            new Servo(10, 170, 90)
        );
    private List<Gtk.HScale> actuatorScales = new List<Gtk.HScale>();

    #endregion


    #region CONSTRUCTORS/DESCTRUCTORS

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        //Init actuator scales.
        this.InitActuatorScales();

        //Load available serial ports.
        this.LoadAvailableSerialPorts();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    #endregion


    #region METHODS

    private void InitActuatorScales()
    {
        //Clear actuator scales list.
        this.actuatorScales.Clear();

        //Add actuators to actuator scales list.
        this.actuatorScales.Add(this.ActuatorScale);
        this.actuatorScales.Add(this.ActuatorScale1);
        this.actuatorScales.Add(this.ActuatorScale2);
        this.actuatorScales.Add(this.ActuatorScale3);
        this.actuatorScales.Add(this.ActuatorScale4);
        this.actuatorScales.Add(this.ActuatorScale5);

        //Set values of scales.
        for(int i = 0; i < actuatorScales.Count; i++) 
        {
            this.actuatorScales[i].Adjustment.Lower = this.robot.Servos[i].MinAngle;
            this.actuatorScales[i].Adjustment.Upper = this.robot.Servos[i].MaxAngle;
            this.actuatorScales[i].Adjustment.Value = this.robot.Servos[i].Angle;
        }
    }

    private void LoadAvailableSerialPorts()
    {

        //Add serial ports to dropdown.
        foreach (string portName in Serial.Instance.GetPortNames())
        {
            this.SerialPortDropdown.AppendText(portName); 
        }

        //Set active serial port.
        this.SerialPortDropdown.Active = 0;

    }

    #endregion


    #region EVENTS

    private void OnConnectSerial()
    {

        //Set connect serial action icon.
        this.connectSerialAction.StockId = Stock.Connect;

        //Disable baudrate and port dropdowns.
        this.BaudRateDropdown.Sensitive = false;
        this.SerialPortDropdown.Sensitive = false;

    }

    private void OnDisconnectSerial()
    {

        //Set connect serial action icon.
        this.connectSerialAction.StockId = Stock.Disconnect;

        //Enable baudrate and port dropdowns.
        this.BaudRateDropdown.Sensitive = true;
        this.SerialPortDropdown.Sensitive = true;

    }

    protected void OnSerialPortDropdownChanged(object sender, EventArgs e)
    {

        //Set serial port.
        Serial.Instance.SetSerialPort(((ComboBox)sender).ActiveText);

    }

    protected void OnBaudRateDropdownChanged(object sender, EventArgs e)
    {

        int baudRate = 0;

        try
        {
            baudRate = Convert.ToInt32(((ComboBox)sender).ActiveText);
        }
        catch(Exception)
        {
            Console.WriteLine("Could not convert dropdown baud rate to int.");
        }

        //Set baud rate.
        Serial.Instance.SetBaudRate(baudRate);

    }

    protected void OnConnectSerialActivated(object sender, EventArgs e)
    {

        if (!Serial.Instance.IsOpen())
        {
            if (Serial.Instance.Open()) this.OnConnectSerial();
            else 
            {
                MessageDialog dialog = new MessageDialog(
                        null,
                        DialogFlags.Modal,
                        MessageType.Info,
                        ButtonsType.Ok,
                        "Could not open serial port."
                    );
                dialog.Run();
                dialog.Destroy();
            }
        }
        else
        {
            if (Serial.Instance.Close()) this.OnDisconnectSerial();
            else
            {
                MessageDialog dialog = new MessageDialog(
                            null,
                            DialogFlags.Modal,
                            MessageType.Info,
                            ButtonsType.Ok,
                            "Could not close serial port."
                        );
                dialog.Run();
                dialog.Destroy();
            }
        }

    }

    protected void OnActuatorScaleChanged(object sender, EventArgs e)
    {

    }

    #endregion
}
