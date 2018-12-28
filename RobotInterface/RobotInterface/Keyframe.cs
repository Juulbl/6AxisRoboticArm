using System;
namespace RobotInterface
{
    public struct Keyframe 
    {
        public string name;             //Keyframe name.
        public UInt32 time;             //Time in milliseconds.
        public float[] actuatorValues;  //Actuator values in degrees.
    }
}
