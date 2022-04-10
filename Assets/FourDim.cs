using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FourDim : MonoBehaviour
{
    //use arrays?
    List<Vector4> balls = new List<Vector4>();
    List<GameObject> ballList = new List<GameObject>();
    Vector4 wHat = new Vector4(0, 0, 0, 1);
    Vector4 origin = new Vector4(0, 0, 0, 0);
    float fourDR = 0.015f;
    float maxDistance;

    Matrix4x4 rotation = new Matrix4x4();

    float alpha, beta, gamma, delta, epsilon, nu;
    Vector4 xUpdate, yUpdate, zUpdate, wUpdate;

    

    //float Vector4.Dot(Vector4 left, Vector4 right)
    //{
    //    if (left.Count != right.Count)
    //    {
    //        throw new ArgumentException("Vectors have mismatched dimension");
    //    }
    //    float sum = 0;
    //    for (int i = 0; i < left.Count; i++)
    //    {
    //        sum += left[i] * right[i];
    //    }
    //    return sum;
    //}

    //Vector4 VectorsSummed(Vector4 left, Vector4 right)
    //{
    //    if (left.Count != right.Count)
    //    {
    //        throw new ArgumentException("Vectors have mismatched dimension");
    //    }
    //    Vector4 result = new Vector4();
    //    for (int i = 0; i < left.Count; i++)
    //    {
    //        result.Add(left[i] + right[i]);
    //    }

    //    return result;
    //}


    //Vector4 VectorsSubtracted(Vector4 left, Vector4 right)
    //{
    //    if (left.Count != right.Count)
    //    {
    //        throw new ArgumentException("Vectors have mismatched dimension");
    //    }
    //    Vector4 result = new Vector4() {};
    //    for (int i = 0; i < left.Count; i++)
    //    {
    //        result.Add(left[i] - right[i]);
    //    }

    //    return result;
    //}

    //Vector4 VectorScaled(float scale, Vector4 toScale)
    //{
    //    Vector4 scaled = new Vector4(toScale);
    //    for (int i = 0; i < toScale.Count; i++)
    //    {
    //        scaled[i] *= scale;
    //    }

    //    return scaled;
    //}

    Vector4 Projected(Vector4 fourDpoint)
    {
        float pDotDub = Vector4.Dot(fourDpoint, wHat);
        Vector4 projected = fourDpoint - pDotDub * wHat;
        float scaleDown = 1 / (1 - pDotDub);
        projected = scaleDown * projected;

        return projected;

    }


    //takes in 4d ball location
    float UniformProjectedRscale(Vector4 fourDpoint)
    {
        float S = 1 / (1 - Vector4.Dot(fourDpoint, wHat));
        return S;
    }

    Vector4 multiIncrement(Vector4 xInc, Vector4 yInc, Vector4 zInc, Vector4 wInc, Vector4 originalVec, int numInc)
    {   
        //Save original vector to update
        for(int i = 0; i < numInc; i++)
        {
            //Vector4 ogCopy = new Vector4(originalVec);
            originalVec[0] = Vector4.Dot(originalVec, xInc);
            originalVec[1] = Vector4.Dot(originalVec, yInc);
            originalVec[2] = Vector4.Dot(originalVec, zInc);
            originalVec[3] = Vector4.Dot(originalVec, wInc);


        }

        return originalVec;
        
    }

    //float vSquared(Vector4 v)
    //{
    //    return Vector4.Dot(v, v);
    //}

    float dSquared(Vector4 one, Vector4 two)
    {
        return Vector4.SqrMagnitude(one - two);
    }


    void createSphere(GameObject sphere, int count, float initRange)
    {

        int i = 0;
        while (i < count)
        {
            //initialize ball as a 4d vector and add to balls list
            Vector4 ball = new Vector4(UnityEngine.Random.Range(-initRange, initRange),
                UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange));

            Vector3 projected = Projected(ball);
            if (dSquared(ball, origin) < initRange * initRange)
            {
                balls.Add(ball);
                ballList.Add(Instantiate(sphere, projected, Quaternion.identity));
                ++i;

            }
            //project ball to 3d vector and create a sphere copy with projected coordinates;
        }

    }

    void createSphereSurface(GameObject sphere, int count, float initRange)
    {

        int i = 0;
        while (i < count)
        {
            //initialize ball as a 4d vector and add to balls list
            Vector4 ball = new Vector4(UnityEngine.Random.Range(-initRange, initRange),
                UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange));

            Vector3 projected = Projected(ball);
            if (dSquared(ball, origin) < initRange * initRange && dSquared(ball, origin) > (initRange - initRange/20) * (initRange - initRange / 20))
            {
                balls.Add(ball);
                ballList.Add(Instantiate(sphere, projected, Quaternion.identity));
                ++i;

            }
            //project ball to 3d vector and create a sphere copy with projected coordinates;
        }

    }



    void createCube(GameObject sphere, int count, float initRange)
    {
        for (int i = 0; i < count; i++)
        {
            Vector4 ball = new Vector4(UnityEngine.Random.Range(-initRange, initRange),
                UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange));

            Vector3 projected = Projected(ball);
         
            balls.Add(ball);
            ballList.Add(Instantiate(sphere, projected, Quaternion.identity));
    
            //project ball to 3d vector and create a sphere copy with projected coordinates;
        }
    }

    Matrix4x4 getRotation(Matrix4x4 rotation, int increments)
    {
        while(increments != 1)
        {
            rotation = rotation * rotation;
            increments /= 2;
        }
        return rotation;
    }

    // Start is called before the first frame update
    void Start()
    {

        int numBalls = 3000;

        //instantiate sample sphere to clone other spheres from
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material.SetColor("_Color", Color.red);
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        //range from 4d origin to generate balls
        float initRange = 0.5f;
        maxDistance = (1f + initRange) * (1f + initRange);

        //createSphereSurface(sphere, numBalls, initRange);
        createCube(sphere, numBalls, initRange);

        sphere.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // factor for making dividing up tiny positional updates. Needs to be fixed so so factor applies to identity matrix as well
        float speed = 0.0005f;
        float factor = Time.deltaTime * speed;


        //rotation matrix factors
        alpha = 3*factor;
        beta = 5*factor;
        gamma = 4*factor;
        delta = 3*factor;
        epsilon = 2*factor;
        nu = 7*factor;

        xUpdate = new Vector4(1, alpha, beta, gamma);
        yUpdate = new Vector4(-alpha, 1, delta, epsilon);
        zUpdate = new Vector4(-beta, -delta, 1, nu);
        wUpdate = new Vector4(-gamma, -epsilon, -nu, 1);

        rotation.SetRow(0, xUpdate);
        rotation.SetRow(1, yUpdate);
        rotation.SetRow(2, zUpdate);
        rotation.SetRow(3, wUpdate);

        rotation = getRotation(rotation, 128);


        //Vector4 RMCOne = new Vector4(1, 0, 0, 0);
        //Vector4 RMCTwo = new Vector4(0, 1, 0, 0 );
        //Vector4 RMCThree = new Vector4(0, 0, 1, 0);
        //Vector4 RMCFour = new Vector4(0, 0, 0, 1);

        ////consolidate rotation matrix
        //RMCOne = multiIncrement(xUpdate, yUpdate, zUpdate, wUpdate, RMCOne, 100);
        //RMCTwo = multiIncrement(xUpdate, yUpdate, zUpdate, wUpdate, RMCTwo, 100);
        //RMCThree = multiIncrement(xUpdate, yUpdate, zUpdate, wUpdate, RMCThree, 100);
        //RMCFour = multiIncrement(xUpdate, yUpdate, zUpdate, wUpdate, RMCFour, 100);


        //update position loop x, y, z, w positions
        for (int i = 0; i < balls.Count; i++)
        {

            //apply consolidated rotation matrix to ball
            //balls[i] = multiIncrement(RMCOne, RMCTwo, RMCThree, RMCFour, balls[i], 1);
            balls[i] = rotation * balls[i];

            //project 4d ball vector to 3d ball vector (vector = position) and update rendered ball position
            Vector3 projected = Projected(balls[i]);
            ballList[i].transform.position = projected;

            float distanceFromEye = dSquared(balls[i], wHat);

            ballList[i].GetComponent<Renderer>().material.color = new Color(1, distanceFromEye / maxDistance, distanceFromEye / maxDistance);


            //Update ball scale with uniform and directional scaling thorugh matrix transform

            //find uniform scaling
            float S = UniformProjectedRscale(balls[i]);
            //find additional pi direction scaling
            float Sprime = (float)Math.Sqrt((double)Vector4.Dot(projected, projected) + 1);
            
            //apply scales (additional scales in z direction)
            ballList[i].transform.localScale = new Vector3(S*fourDR, S*fourDR, S*Sprime*fourDR);

            //rotate z direction to pi direction
            ballList[i].transform.rotation = Quaternion.LookRotation(ballList[i].transform.position);

        }
    }
}
