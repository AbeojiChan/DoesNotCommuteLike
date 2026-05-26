using System;
using System.Runtime.InteropServices;

[Serializable]
public class SteerRecord
{
    public float timestamp;
    public float steerValue;
    public SteerRecord(float time, float steer)
    {
        timestamp = time;
        steerValue = steer;
    }
}
