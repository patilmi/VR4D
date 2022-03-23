using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FourDim : MonoBehaviour
{
    List<List<float>> balls = new List<List<float>>();
    List<GameObject> ballList = new List<GameObject>();
    List<float> eye = new List<float>() {0, 0, 0, 1};
    List<float> wHat = new List<float>() {0, 0, 0, 1};
    float fourDR = 0.03f;


    float alpha, beta, gamma, delta, epsilon, nu;
    List<float> xUpdate, yUpdate, zUpdate, wUpdate;

    

    float DotProduct(List<float> left, List<float> right)
    {
        if (left.Count != right.Count)
        {
            throw new ArgumentException("Vectors have mismatched dimension");
        }
        float sum = 0;
        for (int i = 0; i < left.Count; i++)
        {
            sum += left[i] * right[i];
        }
        return sum;
    }

    List<float> VectorsSummed(List<float> left, List<float> right)
    {
        if (left.Count != right.Count)
        {
            throw new ArgumentException("Vectors have mismatched dimension");
        }
        List<float> result = new List<float>();
        for (int i = 0; i < left.Count; i++)
        {
            result.Add(left[i] + right[i]);
        }

        return result;
    }


    List<float> VectorsSubtracted(List<float> left, List<float> right)
    {
        if (left.Count != right.Count)
        {
            throw new ArgumentException("Vectors have mismatched dimension");
        }
        List<float> result = new List<float>() {};
        for (int i = 0; i < left.Count; i++)
        {
            result.Add(left[i] - right[i]);
        }

        return result;
    }

    List<float> VectorScaled(float scale, List<float> toScale)
    {
        List<float> scaled = new List<float>(toScale);
        for (int i = 0; i < toScale.Count; i++)
        {
            scaled[i] *= scale;
        }

        return scaled;
    }

    List<float> Projected(List<float> fourDpoint)
    {
        float pDotDub = DotProduct(fourDpoint, wHat);
        List<float> projected = VectorsSubtracted(fourDpoint, VectorScaled(pDotDub, wHat));
        float scaleDown = 1 / (1 - pDotDub);
        projected = VectorScaled(scaleDown, projected);
        projected.RemoveAt(3);

        return projected;

    }


    //takes in 4d ball location
    float ProjectedR(List<float> fourDpoint)
    {
        float S = 1 / (1 - DotProduct(fourDpoint, wHat));
        return S;
    }

    void multiIncrement(List<float> xInc, List<float> yInc, List<float> zInc, List<float> wInc, List<float> originalVec, int numInc)
    {   

        for(int i = 0; i < numInc; i++)
        {
            originalVec[0] = DotProduct(originalVec, xInc);
            originalVec[1] = DotProduct(originalVec, yInc);
            originalVec[2] = DotProduct(originalVec, zInc);
            originalVec[3] = DotProduct(originalVec, wInc);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {

        int numBalls = 5000;

        //instantiate sample sphere to clone other spheres from
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material.SetColor("_Color", Color.red);
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        //range from 4d origin to generate balls
        float initRange = 0.4f;

        //initialize balls at random positions around origin with scale untransformed 
        for (int i = 0; i < numBalls; i++)
        {
            //initialize ball as a 4d vector and add to balls list
            List<float> ball = new List<float>() {UnityEngine.Random.Range(-initRange, initRange),
                UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange), UnityEngine.Random.Range(-initRange, initRange)};
            balls.Add(ball);


            //project ball to 3d vector and create a sphere copy with projected coordinates;
            List<float> projected = Projected(ball);
            ballList.Add(Instantiate(sphere, new Vector3(projected[0], projected[1], projected[2]), Quaternion.identity));

        }
        //Hide original model sphere
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

        xUpdate = new List<float>() { 1, alpha, beta, gamma };
        yUpdate = new List<float>() { -alpha, 1, delta, epsilon};
        zUpdate = new List<float>() { -beta, -delta, 1, nu };
        wUpdate = new List<float>() { -gamma, -epsilon, -nu, 1 };

        List<float> RMCOne = new List<float>() { 1, 0, 0, 0 };
        List<float> RMCTwo = new List<float>() { 0, 1, 0, 0 };
        List<float> RMCThree = new List<float>() { 0, 0, 1, 0 };
        List<float> RMCFour = new List<float>() { 0, 0, 0, 1 };

        //consolidate rotation matrix
        multiIncrement(xUpdate, yUpdate, zUpdate, wUpdate, RMCOne, 100);
        multiIncrement(xUpdate, yUpdate, zUpdate, wUpdate, RMCTwo, 100);
        multiIncrement(xUpdate, yUpdate, zUpdate, wUpdate, RMCThree, 100);
        multiIncrement(xUpdate, yUpdate, zUpdate, wUpdate, RMCFour, 100);


        //update position loop x, y, z, w positions
        for (int i = 0; i < balls.Count; i++)
        {
           
            //apply consolidated rotation matrix to ball
            multiIncrement(RMCOne, RMCTwo, RMCThree, RMCFour, balls[i], 1);

            //project 4d ball vector to 3d ball vector (vector = position) and update rendered ball position
            List<float> projected = Projected(balls[i]);
            ballList[i].transform.position = new Vector3(projected[0], projected[1], projected[2]);
            


            //Update ball scale with uniform and directional scaling thorugh matrix transform

            //find uniform scalin
            float S = ProjectedR(balls[i]);
            //find additional pi direction scaling
            float Sprime = (float)Math.Sqrt((double)DotProduct(projected, projected) + 1);
            
            //apply scales (additional scales in z direction)
            ballList[i].transform.localScale = new Vector3(S*fourDR, S*fourDR, S*Sprime*fourDR);

            //rotate z direction to pi direction
            ballList[i].transform.rotation = Quaternion.LookRotation(ballList[i].transform.position);



        }
    }
}
