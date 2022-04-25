using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rotation
{
    // Start is called before the first frame update

    public RotationComponent xy;
    public RotationComponent xz;
    public RotationComponent xw;
    public RotationComponent yz;
    public RotationComponent yw;
    public RotationComponent zw;


    public float speedModifier;

    public Rotation(RotationComponent xy, RotationComponent xz, RotationComponent xw,
        RotationComponent yz, RotationComponent yw, RotationComponent zw, float speedMod = 1)
    {
        this.xy = xy;
        this.xz = xz;
        this.xw = xw;
        this.yz = yz;
        this.yw = yw;
        this.zw = zw;
        speedModifier = speedMod;
    }


    public float finalRoto(RotationComponent rotationPlane, float timeSec)
    {
        return rotationPlane.value + (rotationPlane.oscillationAmplitude * Mathf.Sin(rotationPlane.oscillationFrequency * timeSec * speedModifier));
    }

}
