using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FourDim : MonoBehaviour
{
    //use arrays?
    List<Vector4> balls = new List<Vector4>();
    List<GameObject> ballList = new List<GameObject>();
    static Vector4 wHat = new Vector4(0, 0, 0, 1);
    static Vector4 origin = new Vector4(0, 0, 0, 0);
    float fourDSphereRadius = 0.015f;
    float maxDfromEye;

    Matrix4x4 FourDRotationMatrix = new Matrix4x4();

    
    float alpha, beta, gamma, delta, epsilon, nu;
    Vector4 rotationRowOne, rotationRowTwo, rotationRowThree, rotationRowFour;


    void CreateSphere(GameObject sphere, int count, float initRange)
    {

        int i = 0;
        while (i < count)
        {
            //initialize ball as a 4d vector and add to balls list
            Vector4 ball = new Vector4(UnityEngine.Random.Range(-initRange, initRange),
                UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange));

            Vector3 projected = FourDMath.Projection(ball);
            if (FourDMath.DistanceSquared(ball, origin) < initRange * initRange)
            {
                balls.Add(ball);
                ballList.Add(Instantiate(sphere, projected, Quaternion.identity));
                ++i;

            }
            //project ball to 3d vector and create a sphere copy with projected coordinates;
        }

    }

    void CreateSphereSurface(GameObject sphere, int count, float initRange)
    {

        int i = 0;
        while (i < count)
        {
            //initialize ball as a 4d vector and add to balls list
            Vector4 ball = new Vector4(UnityEngine.Random.Range(-initRange, initRange),
                UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange));

            Vector3 projected = FourDMath.Projection(ball);
            float distanceFromOrigin = FourDMath.DistanceSquared(ball, origin);
            if (distanceFromOrigin < initRange * initRange &&  distanceFromOrigin > (initRange - initRange/20) * (initRange - initRange / 20))
            {
                balls.Add(ball);
                ballList.Add(Instantiate(sphere, projected, Quaternion.identity));
                ++i;

            }
            //project ball to 3d vector and create a sphere copy with projected coordinates;
        }

    }



    void CreateCube(GameObject sphere, int count, float initRange)
    {
        for (int i = 0; i < count; i++)
        {
            Vector4 ball = new Vector4(UnityEngine.Random.Range(-initRange, initRange),
                UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange));

            Vector3 projected = FourDMath.Projection(ball);
         
            balls.Add(ball);
            ballList.Add(Instantiate(sphere, projected, Quaternion.identity));
    
            //project ball to 3d vector and create a sphere copy with projected coordinates;
        }
    }

    Matrix4x4 UpdateRotationMatrix(float deltaTime)
    {
        float angularSpeed = 0.0003f;
        float angleStepSize = deltaTime * angularSpeed;
        alpha = 3 * angleStepSize;
        beta = 5 * angleStepSize;
        gamma = 4 * angleStepSize;
        delta = 3 * angleStepSize;
        epsilon = 2 * angleStepSize;
        nu = 7 * angleStepSize;

        rotationRowOne = new Vector4(1, alpha, beta, gamma);
        rotationRowTwo = new Vector4(-alpha, 1, delta, epsilon);
        rotationRowThree = new Vector4(-beta, -delta, 1, nu);
        rotationRowFour = new Vector4(-gamma, -epsilon, -nu, 1);

        FourDRotationMatrix.SetRow(0, rotationRowOne);
        FourDRotationMatrix.SetRow(1, rotationRowTwo);
        FourDRotationMatrix.SetRow(2, rotationRowThree);
        FourDRotationMatrix.SetRow(3, rotationRowFour);

        FourDRotationMatrix = FourDMath.ToBinaryPower(FourDRotationMatrix, 128);

        return FourDRotationMatrix;

    }



    // Start is called before the first frame update
    void Start()
    {

        int numBalls = 1500;

        //instantiate sample sphere to clone other spheres from
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material.SetColor("_Color", Color.red);
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        //range from 4d origin to generate balls
        float initRange = 0.5f;
        maxDfromEye = (1f + initRange) * (1f + initRange);

        //CreateSphere(sphere, numBalls, initRange);
        //CreateSphereSurface(sphere, numBalls, initRange);
        CreateCube(sphere, numBalls, initRange);

        sphere.SetActive(false);

    }

    
    


    // Update is called once per frame
    void Update()
    {

        FourDRotationMatrix = UpdateRotationMatrix(Time.deltaTime);

        //update position loop x, y, z, w positions
        for (int i = 0; i < balls.Count; i++)
        {

            //apply consolidated rotation matrix to ball
            balls[i] = FourDRotationMatrix * balls[i];

            //project 4d ball vector to 3d ball vector (vector = position) and update rendered ball position
            Vector3 projected = FourDMath.Projection(balls[i]);
            ballList[i].transform.position = projected;

            float distanceFromEye = FourDMath.DistanceSquared(balls[i], wHat);

            ballList[i].GetComponent<Renderer>().material.color = new Color(1, distanceFromEye / maxDfromEye, distanceFromEye / maxDfromEye);


            //Update ball scale with uniform and directional scaling thorugh matrix transform

            float S = FourDMath.ShortRadiusScaling(balls[i]);
            //find additional pi direction scaling
            float Sprime = FourDMath.LongRadiusScaling(projected);
            
            //apply scales (additional scales in z direction)
            ballList[i].transform.localScale = new Vector3(S*fourDSphereRadius, S*fourDSphereRadius, S*Sprime*fourDSphereRadius);

            //rotate z direction to pi direction
            ballList[i].transform.rotation = Quaternion.LookRotation(ballList[i].transform.position);

        }
    }
}
