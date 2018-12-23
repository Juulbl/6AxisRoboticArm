using System;
using Gtk;
using RobotInterface;

public partial class MainWindow : Gtk.Window
{

    #region CONSTRUCTORS/DESCTRUCTORS

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

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
