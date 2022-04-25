using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotationComponent
{

    public float value;
    public float oscillationAmplitude;
    public float oscillationFrequency;

    public RotationComponent(float val, float amp, float freq)
    {
        value = val;
        oscillationAmplitude = amp;
        oscillationFrequency = freq;
    }

}
