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

        public Robot(params Servo[] servos)
        {
            this.Servos = servos;
        }

        #endregion


        #region METHODS

        public bool InitializeServoAngles()
        {
            //If serial not open, return false.
            if (!Serial.Instance.IsOpen()) return false;

            //Set all servo angles.
            for(int i = 0; i < this.Servos.Length; i++)
            {
                this.SetServoAngle(i, this.Servos[i].Angle);
            }

            return true;
        }

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

        public double DegreesToRadians(float degrees)
        {
            return degrees * (Math.PI / 180);
        }

        #endregion

    }
}
