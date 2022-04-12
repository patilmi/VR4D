using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FourDim : MonoBehaviour
{
    //use arrays?
    List<Vector4> balls = new List<Vector4>();
    List<GameObject> ballList = new List<GameObject>();
    
    float fourDSphereRadius = 0.015f;
    float maxDfromEye;
    float minDfromEye;

    Matrix4x4 FourDRotationMatrix = new Matrix4x4();

    
    float alpha, beta, gamma, delta, epsilon, nu;
    Vector4 rotationRowOne, rotationRowTwo, rotationRowThree, rotationRowFour;


    //void CreateSphere(GameObject sphere, int count, float initRange)
    //{

    //    int i = 0;
    //    while (i < count)
    //    {
    //        //initialize ball as a 4d vector and add to balls list
    //        Vector4 ball = new Vector4(UnityEngine.Random.Range(-initRange, initRange),
    //            UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange));

    //        Vector3 projected = FourDMath.Projection(ball);
    //        if (FourDMath.DistanceSquared(ball, origin) < initRange * initRange)
    //        {
    //            balls.Add(ball);
    //            ballList.Add(Instantiate(sphere, projected, Quaternion.identity));
    //            ++i;

    //        }
    //        //project ball to 3d vector and create a sphere copy with projected coordinates;
    //    }

    //}

    //void CreateSphereSurface(GameObject sphere, int count, float initRange)
    //{

    //    int i = 0;
    //    while (i < count)
    //    {
    //        //initialize ball as a 4d vector and add to balls list
    //        Vector4 ball = new Vector4(UnityEngine.Random.Range(-initRange, initRange),
    //            UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange));

    //        Vector3 projected = FourDMath.Projection(ball);
    //        float distanceFromOrigin = FourDMath.DistanceSquared(ball, origin);
    //        if (distanceFromOrigin < initRange * initRange &&  distanceFromOrigin > (initRange - initRange/20) * (initRange - initRange / 20))
    //        {
    //            balls.Add(ball);
    //            ballList.Add(Instantiate(sphere, projected, Quaternion.identity));
    //            ++i;

    //        }
    //        //project ball to 3d vector and create a sphere copy with projected coordinates;
    //    }

    //}



    //void CreateCube(GameObject sphere, int count, float initRange)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        Vector4 ball = new Vector4(UnityEngine.Random.Range(-initRange, initRange),
    //            UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange));

    //        Vector3 projected = FourDMath.Projection(ball);
         
    //        balls.Add(ball);
    //        ballList.Add(Instantiate(sphere, projected, Quaternion.identity));
    
    //        //project ball to 3d vector and create a sphere copy with projected coordinates;
    //    }
    //}



    void UpdateRotationMatrix(float deltaTime)
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

    }

    void UpdateBallList(GameObject sphere)
    {
        for (int i = 0; i < balls.Count; i++)
        {
            ballList.Add(Instantiate(sphere, FourDMath.Projection(balls[i]), Quaternion.identity));
        }



    }

    Color ApplyFog(Vector4 ball, float grayscale)
    {
        float distanceFromEye = FourDMath.DistanceSquared(ball, FourDMath.wHat);
        float distanceOverMaxDistance = (distanceFromEye) / (maxDfromEye);

        Color gray = new Color(grayscale, grayscale, grayscale);
        Color fogged = new Color(1 - ((1-grayscale) * distanceOverMaxDistance), grayscale * distanceOverMaxDistance, grayscale * distanceOverMaxDistance);
        return fogged;

    }


    // Start is called before the first frame update
    void Start()
    {

        int numBalls = 5000;

        //instantiate sample sphere to clone other spheres from
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var sphereRenderer = sphere.GetComponent<Renderer>();
        Shader unlit = Shader.Find("Unlit/Color");

        sphereRenderer.material.shader = unlit;
        sphereRenderer.material.SetColor("_Color", Color.red);
        sphereRenderer.allowOcclusionWhenDynamic = false;
        sphereRenderer.receiveShadows = false;
        sphereRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);



        var sphereCollider = sphere.GetComponent<SphereCollider>();
        sphereCollider.enabled = false;

        //range from 4d origin to generate balls
        float initRange = 0.5f;
        maxDfromEye = (1f + initRange) * (1f + initRange);
        minDfromEye = (1f - initRange) * (1f - initRange);

        //CreateSphere(sphere, numBalls, initRange);
        //balls = BuildFourD.CreateSphereSurface(balls, numBalls, initRange);
        //balls = BuildFourD.BuildCube(balls, numBalls, initRange);
        balls = BuildFourD.BuildIntersectingPlanes(balls, numBalls, initRange, 1);
        UpdateBallList(sphere);

        sphere.SetActive(false);

    }

    

    // Update is called once per frame
    void Update()
    {

        UpdateRotationMatrix(Time.deltaTime);

        //update position loop x, y, z, w positions
        for (int i = 0; i < balls.Count; i++)
        {

            //Rotate 4d Sphere
            balls[i] = FourDRotationMatrix * balls[i];

            //project 4d ball vector to 3d ball vector (vector = position) and update rendered ball position
            Vector3 projected = FourDMath.Projection(balls[i]);
            ballList[i].transform.position = projected;



            float distanceFromEye = FourDMath.DistanceSquared(balls[i], FourDMath.wHat);
            ballList[i].GetComponent<Renderer>().material.color = ApplyFog(balls[i], 0.9f);


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
