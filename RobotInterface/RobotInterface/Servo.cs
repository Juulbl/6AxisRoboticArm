using System;
namespace RobotInterface
{
    public class Servo
    {
        #region FIELDS

        private double minAngle;
        private double maxAngle;
        private double angle;

        #endregion


        #region PROPERTIES

        public double MinAngle
        {
            get => this.minAngle;
            private set => this.minAngle = value;
        }

        public double MaxAngle
        {
            get => this.maxAngle;
            private set => this.maxAngle = value;
        }

        public double Angle
        {
            get => this.angle;
            set
            {
                //Make sure angle is within range.
                if (value > this.MaxAngle) value = this.MaxAngle;
                else if (value < this.MinAngle) value = this.MinAngle;

                //Set angle.
                this.angle = value;
            }
        }

        #endregion


        #region CONSTRUCTORS

        public Servo(double minAngle, double maxAngle, double initalAngle)
        {
            this.MinAngle = minAngle;
            this.MaxAngle = maxAngle;
            this.Angle = initalAngle;
        }

        #endregion
    }
}
