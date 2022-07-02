using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BuildConfig
{

    public FourDPlane[] planes;

    public BuildConfig(FourDPlane[] planes)
    {
        this.planes = planes;
    }


}
