using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation
{
    // Start is called before the first frame update
    public List<RotationComponent> fullRotation;

    public Rotation(List<RotationComponent> components)
    {
        fullRotation = components;
    }

    public void AddRotation(RotationComponent component)
    {
        fullRotation.Add(component);
    }

    public float finalRoto(int index, float timeSec)
    {
        return fullRotation[index].value + (fullRotation[index].oscillationAmplitude * Mathf.Sin(fullRotation[index].oscillationFrequency * timeSec));
    }

}
