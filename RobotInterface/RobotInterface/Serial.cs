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
                //If no instance of serial port in serial, create new instance.
                if (Serial.instance == null) Serial.instance = new Lazy<Serial>();

                //Return serial instance.
                return Serial.instance.Value;
            }
        }

        #endregion

        #region FIELDS

        //Serial port.
        private SerialPort serialPort = new SerialPort();

        //Communication
        private const char startChar = '#';
        private const char endChar = '%';
        private const char splitChar = ':';

        #endregion


        #region PROPERTIES

        public SerialPort SerialPort
        {
            get => this.serialPort;
            private set => this.serialPort = value;
        }

        public bool IsOpen
        {
            get => this.SerialPort.IsOpen;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// Gets the port names.
        /// </summary>
        /// <returns>The port names.</returns>
        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Open serial communication.
        /// </summary>
        /// <returns>If serial port opened successfully.</returns>
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

        /// <summary>
        /// Close serial port.
        /// </summary>
        /// <returns>If serial port closed successfully.</returns>
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

        /// <summary>
        /// Sets the baud rate.
        /// </summary>
        /// <returns><c>true</c>, if baud rate was set, <c>false</c> otherwise.</returns>
        /// <param name="baudRate">Baud rate.</param>
        public bool SetBaudRate(int baudRate)
        {
            //If serial port is open, return false.
            if (this.serialPort.IsOpen) return false;

            //Set baud rate.
            this.serialPort.BaudRate = baudRate;

            return true;
        }

        /// <summary>
        /// Sets the serial port.
        /// </summary>
        /// <returns><c>true</c>, if serial port was set, <c>false</c> otherwise.</returns>
        /// <param name="portName">Port name.</param>
        public bool SetSerialPort(string portName)
        {
            //If serial port is open, return false.
            if (this.serialPort.IsOpen) return false;

            //Set serial port.
            this.serialPort.PortName = portName;

            return true;
        }

        /// <summary>
        /// Write the specified message.
        /// </summary>
        /// <returns>The write.</returns>
        /// <param name="message">Message.</param>
        public bool Write(string message)
        {
            //If serial port is open, return false.
            if (!this.serialPort.IsOpen) return false;

            //Write message.
            this.serialPort.WriteLine(startChar + message + endChar);

            return true;
        }

        /// <summary>
        /// Writes the command.
        /// </summary>
        /// <returns><c>true</c>, if command was writed, <c>false</c> otherwise.</returns>
        /// <param name="command">Command.</param>
        /// <param name="parameters">Parameters.</param>
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
