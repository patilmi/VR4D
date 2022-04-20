using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    List<RotationComponent> fullRotation;

    public Rotation(List<RotationComponent> components)
    {
        fullRotation = components;
    }

    public void AddRotation(RotationComponent component)
    {
        fullRotation.Add(component);
    }

}
