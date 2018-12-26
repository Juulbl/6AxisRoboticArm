using System;
using System.IO.Ports;
using System.Collections.Generic;
using Gtk;
using RobotInterface;

public partial class MainWindow : Gtk.Window
{

    #region FIELDS

    private bool isLoadingFrame = true;
    private Timeline timeline;
    private Robot robot = new Robot(
            new Servo(10, 170, 90),
            new Servo(10, 170, 170),
            new Servo(10, 170, 35),
            new Servo(10, 170, 90),
            new Servo(10, 170, 90),
            new Servo(10, 170, 90),
            new Servo(0, 180, 0)
        );
    private List<Gtk.HScale> actuatorScales = new List<Gtk.HScale>();

    #endregion


    #region CONSTRUCTORS/DESCTRUCTORS

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        //Set frame property panel not sensitive.
        this.FramePropertiesPanel.Sensitive = false;

        //Init actuator scales.
        this.InitActuatorScales();

        //Load available serial ports.
        this.LoadAvailableSerialPorts();

        //Update baud rate and serial port.
        this.OnBaudRateDropdownChanged(this.BaudRateDropdown, null);
        this.OnSerialPortDropdownChanged(this.SerialPortDropdown, null);

        //Init timeline.
        this.timeline = new Timeline(ref this.FrameTreeView, ref this.robot);

        this.isLoadingFrame = false;
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
        this.actuatorScales.Add(this.FrameActuatorScale);
        this.actuatorScales.Add(this.FrameActuatorScale1);
        this.actuatorScales.Add(this.FrameActuatorScale2);
        this.actuatorScales.Add(this.FrameActuatorScale3);
        this.actuatorScales.Add(this.FrameActuatorScale4);
        this.actuatorScales.Add(this.FrameActuatorScale5);
        this.actuatorScales.Add(this.FrameActuatorScale6);

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

    private void AddToSerialTerminal(string text)
    {
        //Add time to text.
        DateTime dateTime = DateTime.Now;
        text = string.Format(
                "[{0}:{1}:{2}.{3}] {4}",
                dateTime.Hour.ToString().PadLeft(2, '0'),
                dateTime.Minute.ToString().PadLeft(2, '0'),
                dateTime.Second.ToString().PadLeft(2, '0'),
                dateTime.Millisecond.ToString().PadLeft(3, '0'),
                text
            );

        //Add text.
        var iter = this.SerialTerminal.Buffer.GetIterAtLine(this.SerialTerminal.Buffer.LineCount);
        this.SerialTerminal.Buffer.Insert(ref iter, text + "\n");
    }

    private float[] GetActuatorScaleValues()
    {
        float[] values = new float[this.actuatorScales.Count];

        for (int i = 0; i < values.Length; i++)
            values[i] = (float)this.actuatorScales[i].Adjustment.Value;

        return values;
    }

    private void SetSelectedFrame(int index)
    {
        //Set is loading frame true.
        this.isLoadingFrame = true;

        this.timeline.SetSelectedKeyframeIndex(index);

        //Set selected frame to the last selected frame.
        this.SetSelectedFrame(this.timeline.Keyframes[index]);
    }

    private void SetSelectedFrame(Keyframe frame)
    {
        //Set is loading frame true.
        this.isLoadingFrame = true;

        this.FramePropertiesPanel.Sensitive = true;
        this.FrameNameEntry.Text = frame.name;
        this.FrameTimeEntry.Text = frame.time.ToString();

        for (int i = 0; i < this.actuatorScales.Count; i++)
        {
            this.actuatorScales[i].Value = frame.actuatorValues[i];
        }

        //Set is loading frame false.
        this.isLoadingFrame = false;
    }

    private void UpdateSelectedKeyframe()
    {
        //Get selected keyframe. If null, return.
        Nullable<Keyframe> keyframeItem = this.timeline.GetSelectedKeyframe();
        if (!keyframeItem.HasValue) return;

        Keyframe keyframe = keyframeItem.Value;

        //Update values.
        keyframe.name = this.FrameNameEntry.Text;
        keyframe.time = Convert.ToUInt32(this.FrameTimeEntry.Text);
        keyframe.actuatorValues = this.GetActuatorScaleValues();

        this.timeline.UpdateSelectedKeyframe(keyframe);
    }

    #endregion


    #region EVENTS

    private void OnConnectSerial()
    {
        this.AddToSerialTerminal("Device connected successfully.");

        //Set connect serial action icon.
        this.connectSerialAction.StockId = Stock.Connect;

        //Disable baudrate and port dropdowns.
        this.BaudRateDropdown.Sensitive = false;
        this.SerialPortDropdown.Sensitive = false;

        //Delay
        System.Threading.Thread.Sleep(3000);

        //Init servo angles.
        this.robot.InitializeServoAngles();

    }

    private void OnDisconnectSerial()
    {
        this.AddToSerialTerminal("Device disconnected successfully.");

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
        catch (Exception)
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

    protected void OnAddFrameActivated(object sender, EventArgs e)
    {
        this.timeline.AddKeyframe(null, null, this.GetActuatorScaleValues());
    }

    protected void OnRemoveFrameActivated(object sender, EventArgs e)
    {

    }

    protected void OnFrameTreeViewRowActivated(object o, RowActivatedArgs args)
    {
        //If is loading frame, return.
        if (this.isLoadingFrame) return;

        this.SetSelectedFrame(args.Path.Indices[args.Path.Indices.Length - 1]);
    }

    protected void OnFrameActuatorScaleChanged(object sender, EventArgs e)
    {
        //If is loading frame, return.
        if (this.isLoadingFrame) return;

        //Get sender name.
        string senderName = ((Gtk.Widget)sender).Name;

        //Check what scale to update.
        for (int i = 0; i < this.actuatorScales.Count; i++)
        {
            Gtk.HScale actuatorScale = this.actuatorScales[i];

            //If actuator name not sender name, continue.
            if (actuatorScale.Name != senderName) continue;

            //Update servo connected to actuator.
            this.robot.SetServoAngle(i, (float)actuatorScale.Adjustment.Value);

            break;
        }

        //Update selected keyframe.
        this.UpdateSelectedKeyframe();
    }

    protected void OnFramePropertyKeyRelease(object o, KeyReleaseEventArgs args)
    {
        if (args.Event.Key == Gdk.Key.Return)
        {
            this.UpdateSelectedKeyframe();
        }
    }

    #endregion
}
