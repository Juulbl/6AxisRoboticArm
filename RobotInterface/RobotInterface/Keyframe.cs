using System;
namespace RobotInterface
{
    public struct Keyframe 
    {
        public string name;
        public UInt32 time;
        public float[] actuatorValues;
    }
}
