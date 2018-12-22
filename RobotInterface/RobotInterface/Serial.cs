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
        private const char startChar = '#';
        private const char endChar = '%';
        private const char splitChar = ':';

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

            //Set serial port.
            this.serialPort.PortName = portName;

            return true;
        }

        public bool Write(string message)
        {
            //If serial port is open, return false.
            if (!this.serialPort.IsOpen) return false;

            //Write message.
            this.serialPort.WriteLine(startChar + message + endChar);

            return true;
        }

        public bool WriteCommand(string command, params string[] parameters)
        {
            //Parameters to string.
            string parametersStr = "";
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0) parametersStr += ',';
                parametersStr += parameters[i];
            }

            return this.Write(command + splitChar + parametersStr);
        }

        #endregion
    }
}
