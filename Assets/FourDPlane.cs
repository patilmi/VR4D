using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDPlane
{

    public int count;
    public int normalAxis;
    public float constantVal;
    public float sideLength;

    public FourDPlane(int numBalls, int constantAxis, float constantVal, float sideLength)
    {
        this.count = numBalls;
        this.normalAxis = constantAxis;
        this.constantVal = constantVal;
        this.sideLength = sideLength;
    }

}
