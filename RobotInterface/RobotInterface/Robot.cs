using System;

namespace RobotInterface
{
    public class Robot
    {
        #region PROPERTIES

        public Servo[] Servos
        {
            get;
            private set;
        }

        #endregion


        #region CONSTRUCTORS

        public Robot(params Servo[] servos)
        {
            this.Servos = servos;
        }

        #endregion

    }
}
