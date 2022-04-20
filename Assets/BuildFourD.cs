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


    public static List<Vector4> BuildCube(List<Vector4> balls, int count, float initRange, bool filled = false, int facePairs = 4)
    {

        int total = count;
        for (int i = 0; i < facePairs && count > 0; i++)
        {
            int j = 0;
            while (j < total/facePairs)
            {
                Vector4 ball = new Vector4(Random.Range(-initRange, initRange), Random.Range(-initRange, initRange),
                 Random.Range(-initRange, initRange), Random.Range(-initRange, initRange));

                if (!filled)
                {
                    ball[i] = (i % 2 == 0) ? initRange : -initRange;
                }
                
                balls.Add(ball);
                count--;
                j++;
            }
        }
        return balls;

    }




    public static List<Vector4> BuildIntersectingPlanes(List<Vector4> balls, int count, float initRange, int numPlanes = 4)
    {
        for (int i = 0; i < count; i++)
        {
            Vector4 ball = new Vector4(Random.Range(-initRange, initRange), Random.Range(-initRange, initRange),
                 Random.Range(-initRange, initRange), Random.Range(-initRange, initRange));

            ball[i % numPlanes] = 0f;
            balls.Add(ball);

        }
        return balls;
    }

    public static List<Vector4> BuildIntersectingPlanesClustered (List<Vector4> balls, int count, float initRange, int numPlanes = 4)
    {
        for (int i = 0; i < count; i++)
        {
            Vector4 ball = new Vector4(Random.Range(-initRange, initRange), Random.Range(-initRange, initRange),
                 Random.Range(-initRange, initRange), Random.Range(-initRange, initRange));

            ball[i % numPlanes] = 0f;
            balls.Add(ball);

        }
        return balls;
    }


}
