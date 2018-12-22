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


        #region METHODS

        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        #endregion
    }
}
