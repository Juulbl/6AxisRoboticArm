using System;
namespace RobotInterface
{
    public class Servo
    {
        #region FIELDS

        private float minAngle;
        private float maxAngle;
        private float angle;

        #endregion


        #region PROPERTIES

        public float MinAngle
        {
            get => this.minAngle;
            private set => this.minAngle = value;
        }

        public float MaxAngle
        {
            get => this.maxAngle;
            private set => this.maxAngle = value;
        }

        public float Angle
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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RobotInterface.Servo"/> class.
        /// </summary>
        /// <param name="minAngle">Minimum angle.</param>
        /// <param name="maxAngle">Max angle.</param>
        /// <param name="initalAngle">Inital angle.</param>
        public Servo(float minAngle, float maxAngle, float initalAngle)
        {
            this.MinAngle = minAngle;
            this.MaxAngle = maxAngle;
            this.Angle = initalAngle;
        }

        #endregion
    }
}
