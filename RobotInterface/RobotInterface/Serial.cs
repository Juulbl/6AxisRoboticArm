using System;
using System.IO.Ports;

namespace RobotInterface
{
    public class Serial
    {

        #region SINGLETON

        private static Lazy<Serial> instance = null;

        public static Serial Instance 
        {
            get
            {
                if (Serial.instance == null) Serial.instance = new Lazy<Serial>();
                return Serial.instance.Value;
            }
        }

        #endregion


        #region FIELDS

        private SerialPort serialPort = new SerialPort();

        #endregion


        #region METHODS

        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public bool IsOpen()
        {
            return this.serialPort.IsOpen;
        }

        public bool Open()
        {
            //If serial port is open, return false.
            if (this.serialPort.IsOpen) return false;

            try
            {
                //Open serial port.
                this.serialPort.Open();
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public bool Close()
        {
            //If serial port is not open, return false.
            if (!this.serialPort.IsOpen) return false;

            try
            {
                //Close serial port.
                this.serialPort.Close();
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public bool SetBaudRate(int baudRate)
        {
            //If serial port is open, return false.
            if (this.serialPort.IsOpen) return false;

            //Set baud rate.
            this.serialPort.BaudRate = baudRate;

            return true;
        }

        public bool SetSerialPort(string portName)
        {
            //If serial port is open, return false.
            if (this.serialPort.IsOpen) return false;

            //Set baud rate.
            this.serialPort.PortName = portName;

            return true;
        }

        #endregion
    }
}
