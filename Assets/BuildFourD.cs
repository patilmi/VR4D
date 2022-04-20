using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildFourD
{
    public static List<Vector4> CreateSphere(List<Vector4> balls, int count, float initRange)
    {

        int i = 0;
        while (i < count)
        {
            //initialize ball as a 4d vector and add to balls list
            Vector4 ball = new Vector4(Random.Range(-initRange, initRange), Random.Range(-initRange, initRange),
                Random.Range(-initRange, initRange), Random.Range(-initRange, initRange));

            if (FourDMath.DistanceSquared(ball, FourDMath.origin) < initRange * initRange)
            {
                balls.Add(ball);
                ++i;
            }
            //project ball to 3d vector and create a sphere copy with projected coordinates;
        }
        return balls;

    }

    public static List<Vector4> CreateSphereSurface(List<Vector4> balls, int count, float initRange)
    {

        int i = 0;
        while (i < count)
        {
            //initialize ball as a 4d vector and add to balls list
            Vector4 ball = new Vector4(Random.Range(-initRange, initRange), Random.Range(-initRange, initRange),
                Random.Range(-initRange, initRange), Random.Range(-initRange, initRange));

            float distanceFromOrigin = FourDMath.DistanceSquared(ball, FourDMath.origin);
            if (distanceFromOrigin < initRange * initRange && distanceFromOrigin > (initRange - initRange / 20) * (initRange - initRange / 20))
            {
                balls.Add(ball);
                ++i;

            }
            //project ball to 3d vector and create a sphere copy with projected coordinates;
        }
        return balls;

    }



    public static void BuildPlanes(List<FourDPoint> balls, BuildConfig config)
    {

        for (int i = 0; i < config.planes.Count; i++)
        {
            int normalAxis = config.planes[i].normalAxis;
            for (int j = 0; j < config.planes[i].count;)
            {

                float halfSideLen = config.planes[i].sideLength;
                Vector4 ball = new Vector4(Random.Range(-halfSideLen, halfSideLen), Random.Range(-halfSideLen, halfSideLen),
                    Random.Range(-halfSideLen, halfSideLen), Random.Range(-halfSideLen, halfSideLen));

                ball[normalAxis] = config.planes[i].constantVal;

                balls.Add(new FourDPoint(ball));
                ++j;


            }

        }

    }

}
