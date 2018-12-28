using System;
using System.Threading;
using System.Collections.Generic;
using Gtk;
using RobotInterface;

public partial class MainWindow : Gtk.Window
{

    #region FIELDS

    //Is loading something.
    private bool isLoadingFrame = true;

    //Timeline and timeline updating.
    private Timeline timeline;
    private Thread timelineUpdateThread;

    //Robot and its actuator scales.
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


    #region THREAD_HANDLERS

    /// <summary>
    /// Handles the timeline update thread.
    /// </summary>
    private void HandleTimelineUpdateThread()
    {
        //Saves the current and last recorded date time to calculate the delta time.
        DateTime currentDateTime = DateTime.Now;
        DateTime lastDateTime = currentDateTime;
        double deltaTime = 0;

        //While this thread is running.
        while (true)
        {
            //Get current date time and calculate delta time.
            currentDateTime = DateTime.Now;
            deltaTime = (currentDateTime - lastDateTime).TotalMilliseconds;

            //Update timeline.
            this.timeline.Update(ref deltaTime);

            //Set last date time to current date time.
            lastDateTime = currentDateTime;
        }
    }

    #endregion


    #region CONSTRUCTORS/DESCTRUCTORS
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        //Init actuator scales.
        this.InitActuatorScales();

        //Load available serial ports.
        this.LoadAvailableSerialPorts();

        //Update baud rate and serial port.
        this.OnBaudRateDropdownChanged(this.BaudRateDropdown, null);
        this.OnSerialPortDropdownChanged(this.SerialPortDropdown, null);

        //Init timeline.
        this.timeline = new Timeline(ref this.FrameTreeView, ref this.FramesPanel, ref this.FramePropertiesPanel, ref this.robot);

        //Init timeline update thread.
        this.timelineUpdateThread = new Thread(HandleTimelineUpdateThread);
        this.timelineUpdateThread.Start();

        //Set is loading false.
        this.isLoadingFrame = false;
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        //Abort thread.
        this.timelineUpdateThread.Abort();
        this.timelineUpdateThread = null;

        //Quit app.
        Application.Quit();
        a.RetVal = true;
    }

    #endregion


    #region METHODS

    /// <summary>
    /// Initializes the actuator scales.
    /// </summary>
    private void InitActuatorScales()
    {
        //Clear actuator scales list.
        this.actuatorScales.Clear();

        //Add actuators to actuator scales list (Makes it easier to update these in the future).
        this.actuatorScales.Add(this.FrameActuatorScale);
        this.actuatorScales.Add(this.FrameActuatorScale1);
        this.actuatorScales.Add(this.FrameActuatorScale2);
        this.actuatorScales.Add(this.FrameActuatorScale3);
        this.actuatorScales.Add(this.FrameActuatorScale4);
        this.actuatorScales.Add(this.FrameActuatorScale5);
        this.actuatorScales.Add(this.FrameActuatorScale6);

        //Set values of scales.
        for (int i = 0; i < actuatorScales.Count; i++)
        {
            this.actuatorScales[i].Adjustment.Lower = this.robot.Servos[i].MinAngle;
            this.actuatorScales[i].Adjustment.Upper = this.robot.Servos[i].MaxAngle;
            this.actuatorScales[i].Adjustment.Value = this.robot.Servos[i].Angle;
        }
    }

    /// <summary>
    /// Loads the available serial ports.
    /// </summary>
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

    /// <summary>
    /// Adds to serial terminal.
    /// </summary>
    /// <param name="text">Text.</param>
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

    /// <summary>
    /// Gets the actuator scale values.
    /// </summary>
    /// <returns>The actuator scale values.</returns>
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

        this.timeline.SelectKeyframeIndex = index;

        //Set selected frame to the last selected frame.
        this.SetSelectedFrame(this.timeline.Keyframes[index]);
    }

    /// <summary>
    /// Sets the selected frame.
    /// </summary>
    /// <param name="frame">Frame.</param>
    private void SetSelectedFrame(Keyframe frame)
    {
        //Set is loading frame true.
        this.isLoadingFrame = true;

        this.FrameNameEntry.Text = frame.name;
        this.FrameTimeEntry.Text = frame.time.ToString();

        for (int i = 0; i < this.actuatorScales.Count; i++)
        {
            this.actuatorScales[i].Value = frame.actuatorValues[i];
        }

        //Set is loading frame false.
        this.isLoadingFrame = false;
    }

    /// <summary>
    /// Updates the selected keyframe.
    /// </summary>
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

    /// <summary>
    /// On connect serial event.
    /// </summary>
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

    /// <summary>
    /// On disconnect serial event.
    /// </summary>
    private void OnDisconnectSerial()
    {
        this.AddToSerialTerminal("Device disconnected successfully.");

        //Set connect serial action icon.
        this.connectSerialAction.StockId = Stock.Disconnect;

        //Enable baudrate and port dropdowns.
        this.BaudRateDropdown.Sensitive = true;
        this.SerialPortDropdown.Sensitive = true;

    }

    /// <summary>
    /// On serial port dropdown change event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnSerialPortDropdownChanged(object sender, EventArgs e)
    {

        //Set serial port.
        Serial.Instance.SetSerialPort(((ComboBox)sender).ActiveText);

    }

    /// <summary>
    /// On baud rate dropdown change event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnBaudRateDropdownChanged(object sender, EventArgs e)
    {

        int baudRate = 0;

        //Try to cast the baudrate. On error, return.
        try
        {
            baudRate = Convert.ToInt32(((ComboBox)sender).ActiveText);
        }
        catch (Exception)
        {
            Console.WriteLine("Could not convert dropdown baud rate to int.");
            return;
        }

        //Set baud rate.
        Serial.Instance.SetBaudRate(baudRate);

    }

    /// <summary>
    /// On connect serial activate event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnConnectSerialActivated(object sender, EventArgs e)
    {

        //If serial is not open.
        if (!Serial.Instance.IsOpen)
        {
            //If serial successfully opened, else display dialog.
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
            //If serial successfully closed, else display dialog.
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

    /// <summary>
    /// On add frame activate event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnAddFrameActivated(object sender, EventArgs e)
    {
        //If is loading frame, return.
        if (this.isLoadingFrame) return;

        this.timeline.AddKeyframe(null, null, this.GetActuatorScaleValues());
    }

    protected void OnRemoveFrameActivated(object sender, EventArgs e)
    {
        //If is loading frame, return.
        if (this.isLoadingFrame) return;

        this.timeline.DeleteKeyframe(this.timeline.SelectKeyframeIndex);
    }

    /// <summary>
    /// On frame tree view row activate event.
    /// </summary>
    /// <param name="o">O.</param>
    /// <param name="args">Arguments.</param>
    protected void OnFrameTreeViewRowActivated(object o, RowActivatedArgs args)
    {
        //If is loading frame, return.
        if (this.isLoadingFrame) return;

        this.SetSelectedFrame(args.Path.Indices[args.Path.Indices.Length - 1]);
    }

    /// <summary>
    /// On frame actuator scale change event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
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

    /// <summary>
    /// On frame property key release event.
    /// </summary>
    /// <param name="o">O.</param>
    /// <param name="args">Arguments.</param>
    protected void OnFramePropertyKeyRelease(object o, KeyReleaseEventArgs args)
    {
        //If key pressed is return or key pad enter key.
        if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter)
        {
            //Update selected frame with data in GUI.
            this.UpdateSelectedKeyframe();
        }
    }

    /// <summary>
    /// On reset timeline activate event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnResetTimelineActivated(object sender, EventArgs e)
    {
        this.timeline.CurrentTime = 0;
    }

    /// <summary>
    /// On stop timeline activate event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnStopTimelineActivated(object sender, EventArgs e)
    {
        this.timeline.Stop();
    }

    /// <summary>
    /// On play timeline activate event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnPlayTimelineActivated(object sender, EventArgs e)
    {
        this.timeline.Play();
    }

    /// <summary>
    /// On repeat timeline toggle event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnRepeatTimelineToggled(object sender, EventArgs e)
    {
        this.timeline.Repeat = ((ToggleAction)sender).Active;
    }

    #endregion
}
