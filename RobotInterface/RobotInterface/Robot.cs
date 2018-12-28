using System;

namespace RobotInterface
{
    public class Robot
    {
        #region FIELDS

        private int selectedServoIndex = -1;

        #endregion


        #region PROPERTIES

        public Servo[] Servos
        {
            get;
            private set;
        }

        public int SelectedServoIndex
        {
            get => this.selectedServoIndex;
            set
            {
                //If value is current selected servo, return.
                if (this.selectedServoIndex == value) return;

                //If value out of range, return.
                if (value < 0 || value >= this.Servos.Length) return;

                //Set selected servo index.
                this.selectedServoIndex = value;

                //Send select servo.
                Serial.Instance.WriteCommand("SELECT_SERVO", this.selectedServoIndex.ToString());
            }
        }

        #endregion


        #region CONSTRUCTORS

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RobotInterface.Robot"/> class.
        /// </summary>
        /// <param name="servos">Servos.</param>
        public Robot(params Servo[] servos)
        {
            this.Servos = servos;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// Initializes the servo angles.
        /// </summary>
        /// <returns><c>true</c>, if servo angles was initialized, <c>false</c> otherwise.</returns>
        public bool InitializeServoAngles()
        {
            //If serial not open, return false.
            if (!Serial.Instance.IsOpen) return false;

            //Set all servo angles.
            for(int i = 0; i < this.Servos.Length; i++)
            {
                this.SetServoAngle(i, this.Servos[i].Angle);
            }

            return true;
        }

        /// <summary>
        /// Sets the servo angle.
        /// </summary>
        /// <returns><c>true</c>, if servo angle was set, <c>false</c> otherwise.</returns>
        /// <param name="servoIndex">Servo index.</param>
        /// <param name="angle">Angle.</param>
        public bool SetServoAngle(int servoIndex, float angle)
        {
            //If servo does not exist, return false.
            if (servoIndex < 0 || servoIndex >= this.Servos.Length) return false;

            //Set selected servo index.
            this.SelectedServoIndex = servoIndex;

            //Set servo angle.
            this.Servos[servoIndex].Angle = angle;

            //Send servo angle to robot.
            Serial.Instance.WriteCommand("SET_ANGLE", this.DegreesToRadians(angle).ToString());

            return true;
        }

        /// <summary>
        /// Converts degreeses to radians.
        /// </summary>
        /// <returns>Degrees as radians.</returns>
        /// <param name="degrees">Degrees.</param>
        public double DegreesToRadians(float degrees)
        {
            return degrees * (Math.PI / 180);
        }

        #endregion

    }
}
