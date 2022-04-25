using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class FourDim : MonoBehaviour
{
    List<FourDPoint> balls = new List<FourDPoint>();


    float fourDSphereRadius = 0.015f;


    Matrix4x4 FourDRotationMatrix = new Matrix4x4();


    //float alpha, beta, gamma, delta, epsilon, nu;
    Vector4 rotationRowOne, rotationRowTwo, rotationRowThree, rotationRowFour;

    float[] paramsZero = {0, 3, 0, 0, 6, 0};
    float[] paramsSet = {3, 5, 4, 3, 2, 7};

    Color defaultBallColor = new Color(1f, 0f, 0f);
    Color fogColor = new Color(0.9f, 0.9f, 0.9f);

    List<FourDPlane> planeList = new List<FourDPlane>();

    FourDPlane side1 = new FourDPlane(500, 0, -0.5f, 0.4f);
    FourDPlane side2 = new FourDPlane(500, 0, 0.5f, 0.4f);
    FourDPlane side3 = new FourDPlane(500, 1, -0.5f, 0.4f);
    FourDPlane side4 = new FourDPlane(500, 1, 0.5f, 0.4f);
    FourDPlane side5 = new FourDPlane(500, 2, -0.5f, 0.4f);
    FourDPlane side6 = new FourDPlane(500, 2, 0.5f, 0.4f);
    FourDPlane side7 = new FourDPlane(500, 3, -0.5f, 0.4f);
    FourDPlane side8 = new FourDPlane(500, 3, 0.5f, 0.4f);


    BuildConfig cubeSides;

    RotationComponent alpha, beta, gamma, delta, epsilon, nu;
    List<RotationComponent> components = new List<RotationComponent>();

    //List<Rotation> rotationObjects;

    Rotations rotations;
    


    Rotation fullRoto;






    void UpdateRotationMatrix(float deltaTime, float timeSec, Rotation rotation)
    {
        float angularSpeed = 1f/128f;
        float angleStepSize = deltaTime * angularSpeed * rotation.speedModifier;

        //*Mathf.Sin(timeSec)
  

        float xy =  rotation.finalRoto(rotation.xy, timeSec) * angleStepSize;
        float xz = rotation.finalRoto(rotation.xz, timeSec) * angleStepSize;
        float xw = rotation.finalRoto(rotation.xw, timeSec) * angleStepSize;
        float yz = rotation.finalRoto(rotation.yz, timeSec) * angleStepSize;
        float yw = rotation.finalRoto(rotation.yw, timeSec) * angleStepSize;
        float zw = rotation.finalRoto(rotation.zw, timeSec) * angleStepSize;

        rotationRowOne = new Vector4(1, xy, xz, xw);
        rotationRowTwo = new Vector4(-xy, 1, yz, yw);
        rotationRowThree = new Vector4(-xz, -yz, 1, zw);
        rotationRowFour = new Vector4(-xw, -yw, -zw, 1);

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
            balls[i].sphere = (Instantiate(sphere, FourDMath.Projection(balls[i].point), Quaternion.identity));
            balls[i].sphereRenderer = balls[i].sphere.GetComponent<Renderer>();          
            balls[i].originalColor = defaultBallColor;          
        }
    }


    Color ExponentialFog(float distance, Color ogBallColor) 
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


    //read rotation objects from json File
    void readRotationObject()
    {

        string fileName = "Assets/RotationObjects.json";
        string jsonString = System.IO.File.ReadAllText(fileName);
        rotations = JsonUtility.FromJson<Rotations>(jsonString);
        

    }


    // Start is called before the first frame update
    void Start()
    {



        //int numBalls = 3000;

        planeList.Add(side1);
        planeList.Add(side2);
        planeList.Add(side3);
        planeList.Add(side4);
        planeList.Add(side5);
        planeList.Add(side6);
        planeList.Add(side7);
        planeList.Add(side8);

        cubeSides = new BuildConfig(planeList);

        alpha = new RotationComponent(0, 0.1f, 1);
        beta = new RotationComponent(0, 0, 0);
        gamma = new RotationComponent(0f, 0f, 0);
        delta = new RotationComponent(0, 0, 0);
        epsilon = new RotationComponent(0f, 0f, 0);
        nu = new RotationComponent(0f, 0f, 0);

        fullRoto = new Rotation(alpha, beta, gamma, delta, epsilon, nu, 1f);

        readRotationObject();




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

        //CreateSphere(sphere, numBalls, initRange);
        //balls = BuildFourD.CreateSphereSurface(balls, numBalls, initRange);
        BuildFourD.BuildPlanes(balls, cubeSides);
        UpdateBallList(sphere);

        //sphere.SetActive(false);

    }

    

    // Update is called once per frame
    void Update()
    {



        int currentRotation = ((int)Time.fixedTime / 10) % rotations.rotations.Length;
        UpdateRotationMatrix(Time.deltaTime, Time.fixedTime, rotations.rotations[currentRotation]);
        

        //update position loop x, y, z, w positions
        for (int i = 0; i < balls.Count; i++)
        {

            //Rotate 4d Sphere
            balls[i].point = FourDRotationMatrix * balls[i].point;

            //project 4d ball vector to 3d ball vector (vector = position) and update rendered ball position
            Vector3 projected = FourDMath.Projection(balls[i].point);
            balls[i].sphere.transform.position = projected;


 
            balls[i].sphereRenderer.material.color = ExponentialFog(Vector4.Magnitude(balls[i].point - FourDMath.wHat), balls[i].originalColor);


            //Update ball scale with uniform and directional scaling thorugh matrix transform
            float S = FourDMath.ShortRadiusScaling(balls[i].point);

            //find additional pi direction scaling
            float Sprime = FourDMath.LongRadiusScaling(projected);
            
            //apply scales (additional scales in z direction)
            balls[i].sphere.transform.localScale = new Vector3(S*fourDSphereRadius, S*fourDSphereRadius, S*Sprime*fourDSphereRadius);

            //rotate z direction to pi direction
            balls[i].sphere.transform.rotation = Quaternion.LookRotation(balls[i].sphere.transform.position);

        }
    }
}
