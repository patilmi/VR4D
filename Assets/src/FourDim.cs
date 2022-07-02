using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class FourDim : MonoBehaviour
{
    List<FourDPoint> balls = new List<FourDPoint>();


    const float fourDSphereRadius = 0.004f;
    Matrix4x4 FourDRotationMatrix = new Matrix4x4();


    Color defaultBallColor = new Color(1f, 0f, 0f);
    Color fogColor = new Color(0.9f, 0.9f, 0.9f);

    List<FourDPlane> planeList = new List<FourDPlane>();

    //Going to be replaced with json config object

    static float planeDisaplacementFromCenter = 0.1f;
    static float planeSideLength = 0.1f;

    static int ballCount = 2000;

    FourDPlane side1 = new FourDPlane(ballCount, 0, -planeDisaplacementFromCenter, planeSideLength);
    //FourDPlane side2 = new FourDPlane(ballCount, 0, planeDisaplacementFromCenter, planeSideLength);
    //FourDPlane side3 = new FourDPlane(ballCount, 1, -planeDisaplacementFromCenter, planeSideLength);
    //FourDPlane side4 = new FourDPlane(ballCount, 1, planeDisaplacementFromCenter, planeSideLength);
    //FourDPlane side5 = new FourDPlane(ballCount, 2, -planeDisaplacementFromCenter, planeSideLength);
    //FourDPlane side6 = new FourDPlane(ballCount, 2, planeDisaplacementFromCenter, planeSideLength);
    //FourDPlane side7 = new FourDPlane(ballCount, 3, -planeDisaplacementFromCenter, planeSideLength);
    //FourDPlane side8 = new FourDPlane(ballCount, 3, planeDisaplacementFromCenter, planeSideLength);


    BuildConfig cubeSides;
    List<RotationComponent> components = new List<RotationComponent>();

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

        Vector4 rotationRowOne = new Vector4(1, xy, xz, xw);
        Vector4 rotationRowTwo = new Vector4(-xy, 1, yz, yw);
        Vector4 rotationRowThree = new Vector4(-xz, -yz, 1, zw);
        Vector4 rotationRowFour = new Vector4(-xw, -yw, -zw, 1);

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
        float lambda = -1f;
        float decay = Mathf.Exp(lambda * distance);
        Color fog = new Color(Fogged(decay, ogBallColor[0], fogColor[0]), Fogged(decay, ogBallColor[1], fogColor[1]),
            Fogged(decay, ogBallColor[2], fogColor[2]));
        return fog;
    }

    float Fogged(float decay, float colorVal, float fogVal) 
    {
        return (colorVal * decay) + (fogVal * (1 - decay));
    }


    //read rotation objects from json File
    void LoadRotationObject()
    {

        string fileName = "Assets/src/config/RotationObjects.json";
        string jsonString = System.IO.File.ReadAllText(fileName);
        rotations = JsonUtility.FromJson<Rotations>(jsonString);
        

    }

    void LoadStructureObject()
    {
        //replace with json implementation
        //planeList.Add(side1);
        //planeList.Add(side2);
        //planeList.Add(side3);
        //planeList.Add(side4);
        //planeList.Add(side5);
        //planeList.Add(side6);
        //planeList.Add(side7);
        //planeList.Add(side8);
        string fileName = "Assets/src/config/BuildConfig.json";
        string jsonString = System.IO.File.ReadAllText(fileName);
        cubeSides = JsonUtility.FromJson<BuildConfig>(jsonString);

        //cubeSides = new BuildConfig(planeList);

    }



    // Start is called before the first frame update
    void Start()
    {
        //Load config structure and rotation config objects
        LoadStructureObject();
        LoadRotationObject();

        //instantiate sample sphere to clone other spheres from
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        BuildFourD.RootSphereConfig(sphere);
        
        //Build structure embedded in 4D space
        BuildFourD.BuildPlanes(balls, cubeSides);
        UpdateBallList(sphere);

        //apply fog to original sphere
        sphere.GetComponent<Renderer>().material.color = ExponentialFog(Vector4.Magnitude(FourDMath.wHat), sphere.GetComponent<Renderer>().material.color);
        sphere.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        //sphere.SetActive(false);

    }

    

    // Update is called once per frame
    void Update()
    {

        int cycleRange = rotations.rotations.Length;
        

        int currentRotation = ((int)Time.fixedTime / 10) % cycleRange;
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
