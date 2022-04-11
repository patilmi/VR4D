using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FourDMath
{
    static Vector4 wHat = new Vector4(0, 0, 0, 1);
    public static Vector4 Projection(Vector4 point)
    {
        float pDotDub = Vector4.Dot(point, wHat);
        Vector4 projected = point - pDotDub * wHat;
        //scale
        float scaleDown = 1 / (1 - pDotDub);
        projected = scaleDown * projected;

        return projected;

    }

    public static float ShortRadiusScaling(Vector4 point)
    {
        float S = 1 / (1 - Vector4.Dot(point, wHat));
        return S;
    }

    public static float LongRadiusScaling(Vector3 projectedPoint)
    {
        return (float)Math.Sqrt((double)Vector4.Dot(projectedPoint, projectedPoint) + 1);
    }

    public static float DistanceSquared(Vector4 one, Vector4 two)
    {
        return Vector4.SqrMagnitude(one - two);
    }

    public static Matrix4x4 ToBinaryPower(Matrix4x4 matrice, int increments)
    {
        while (increments != 1)
        {
            matrice = matrice * matrice;
            increments /= 2;
        }
        return matrice;
    }

}
