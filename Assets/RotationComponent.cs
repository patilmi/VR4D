using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationComponent
{

    float value;
    float oscillationAmplitude;
    float oscillationFrequency;

    public RotationComponent(float val, float amp, float freq)
    {
        value = val;
        oscillationAmplitude = amp;
        oscillationFrequency = freq;
    }

}
