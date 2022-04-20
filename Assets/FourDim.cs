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
    //float maxDfromEye;
    //float minDfromEye;

    Matrix4x4 FourDRotationMatrix = new Matrix4x4();

 

    float alpha, beta, gamma, delta, epsilon, nu;
    Vector4 rotationRowOne, rotationRowTwo, rotationRowThree, rotationRowFour;

    float[] paramsZero = {0, 3, 0, 3, 8, 2};
    float[] paramsSet = {3, 5, 4, 3, 2, 7};

    Color ogBallColor = new Color(1f, 0f, 0f);
    Color fogColor = new Color(0.9f, 0.9f, 0.9f);


    void UpdateRotationMatrix(float deltaTime, float timeSec, float[] paramValues)
    {
        float angularSpeed = 0.0003f;
        float angleStepSize = deltaTime * angularSpeed;

        //*Mathf.Sin(timeSec)
  

        alpha = paramValues[0] * angleStepSize;
        beta = paramValues[1] * angleStepSize;
        gamma = paramValues[2] * angleStepSize;
        delta = paramValues[3] * Mathf.Sin(7 * timeSec) * angleStepSize;
        epsilon = paramValues[4] * angleStepSize;
        nu = paramValues[5] * Mathf.Sin(10*timeSec) * angleStepSize;

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


    Color ExponentialFog(float distance) 
    {
        float lambda = -0.8f;
        float decay = Mathf.Exp(lambda * distance);
        Color fog = new Color(fogged(decay, ogBallColor[0], fogColor[0]), fogged(decay, ogBallColor[1], fogColor[1]),
            fogged(decay, ogBallColor[2], fogColor[2]));
        return fog;
    }

    float fogged(float decay, float colorVal, float fogVal) 
    {
        return (colorVal * decay) + (fogVal * (1 - decay));
    }


    // Start is called before the first frame update
    void Start()
    {

        int numBalls = 3000;

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
        float initRangeCube = 0.45f;
        float initRangePlane = 0.5f;
        //maxDfromEye = Mathf.Sqrt((1f + 2*initRangeCube) * (1f + 2*initRangeCube));
        //minDfromEye = Mathf.Sqrt((1f - 2*initRangeCube) * (1f - 2*initRangeCube));

        //CreateSphere(sphere, numBalls, initRange);
        //balls = BuildFourD.CreateSphereSurface(balls, numBalls, initRange);
        balls = BuildFourD.BuildCube(balls, numBalls, initRangeCube);
        //balls = BuildFourD.BuildIntersectingPlanes(balls, numBalls, initRangePlane, 1);
        UpdateBallList(sphere);

        sphere.SetActive(false);

    }

    

    // Update is called once per frame
    void Update()
    {

        UpdateRotationMatrix(Time.deltaTime, Time.fixedTime, paramsZero);
        

        //update position loop x, y, z, w positions
        for (int i = 0; i < balls.Count; i++)
        {

            //Rotate 4d Sphere
            balls[i] = FourDRotationMatrix * balls[i];

            //project 4d ball vector to 3d ball vector (vector = position) and update rendered ball position
            Vector3 projected = FourDMath.Projection(balls[i]);
            ballList[i].transform.position = projected;


            //ballList[i].GetComponent<Renderer>().material.color = ApplyFog(balls[i], 0.9f);

            var ballRenderer = ballList[i].GetComponent<Renderer>();
            ballRenderer.material.color = ExponentialFog(Vector4.Magnitude(balls[i] - FourDMath.wHat));
            //Color ballColor = ballRenderer.material.color;
            //ballColor[0] = 1f;
            //ballColor[1] = 0f;
            //ballColor[2] = 0f;

            //ballRenderer.material.color = ballColor;





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
