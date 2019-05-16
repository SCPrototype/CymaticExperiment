using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chladni : MonoBehaviour
{
    const int _MaxSand = 1000;

    //Changable variations
    public static int plateSize = 100;

    public GameObject TargetPlane;
    public GameObject PixelPrefab;
    public GameObject SandPrefab;
    public Material[] MaterialCache;
    public bool changedValue = false;
    public GameObject collisionBox;

    float start = 0.4f;         // a value for start simulation;
    float wfMax = 7.0f;         // a value for end up the simulation;
    float A = 0.5f;             // for Wave attenuation, should between 0 to 1;
    float k = 0.0f;             // for Wave source shift from plate center
    bool three_d = false;       // 2D/3D draw option;

    float pixelSizeX = 0;
    float pixelSizeZ = 0;

    // unchangable variations
    GameObject[,] pixelGrid = new GameObject[plateSize, plateSize];
    MeshRenderer[,] pixelRenderers = new MeshRenderer[plateSize, plateSize];
    float[,] vibrations = new float[plateSize, plateSize];
    List<Pixel> p;
    List<Sand> sand = new List<Sand>();
    int R;
    float waveLengthFactor;
    float waveIncrease = 0.01f;
    bool photo = false;
    float sumOfWholePlate0, sumOfWholePlate1 = 0.0f, sumOfWholePlate2;
    float maxY = 0;
    float sum = 0;
    int frameNr = 1;
    int[] frameNrArray = new int[] { 4, 47, 67, 107, 167 };


    // Start is called before the first frame update
    void Start()
    {
        sumOfWholePlate0 = sumOfWholePlate2 = plateSize * plateSize;
        R = (int)(-2.0 / Mathf.Log10(A)) + 1;
        prepare();
    }

    void prepare()
    {
        pixelSizeX = TargetPlane.transform.localScale.x / plateSize;
        pixelSizeZ = TargetPlane.transform.localScale.z / plateSize;
        for (int i = 0; i < plateSize; i++)
        {
            for (int j = 0; j < plateSize; j++)
            {
                pixelGrid[i, j] = GameObject.Instantiate(PixelPrefab, TargetPlane.transform);
                pixelGrid[i, j].transform.localScale = new Vector3(pixelSizeX, 0.1f, pixelSizeZ);
                pixelGrid[i, j].transform.position = TargetPlane.transform.position + new Vector3(pixelSizeX * (i - (plateSize / 2)) * 10, 0.001f, pixelSizeZ * (j - (plateSize / 2)) * 10) + new Vector3(pixelSizeX * 5f, 0, pixelSizeZ * 5f);
                pixelRenderers[i, j] = pixelGrid[i, j].GetComponent<MeshRenderer>();
                //Set all grid pixels to their default state (100% vibration).
                pixelRenderers[i, j].material = MaterialCache[0];
                vibrations[i, j] = 1;
            }
        }

        p = new List<Pixel>();
        for (int i = 0; i < Mathf.Floor(plateSize / 2.0f); i++)
        {
            for (int j = 0; j <= i; j++)
            {
                Pixel pp = new Pixel(j, i, R, A, k);
                float lamda = plateSize / (start + waveLengthFactor);
                pp.interference(lamda);
                p.Add(pp);
            }
        }
    }

    void doInterference()
    {
        sum = 0.0f;
        maxY = 0.0f;
        float lamda = plateSize / (start + waveLengthFactor);
        for (int i = 0; i < p.Count; i++)
        {
            p[i].interference(lamda);
            sum += Mathf.Abs(p[i].getY());
            maxY = Mathf.Abs(p[i].getY()) > maxY ? Mathf.Abs(p[i].getY()) : maxY;
        }
    }

    void draw()
    {
        //Debug.Log("frameNr=" + frameNr + "  R=" + R + "  waveLengthFactor=" + waveLengthFactor);
        if (photo)
        {
            if (!Input.GetKey(KeyCode.P) && !changedValue)
            {
                return;
            }
        }

        changedValue = false;
        waveLengthFactor = Mathf.Ceil(frameNr * waveIncrease * 100) / 100;
        doInterference();

        float y = 255.0f / maxY;

        Material targetMaterial = MaterialCache[0];

        for (int i = 0; i < p.Count; i++)
        {
            Pixel pp = p[i];

            float clr = 1.0f - ((y * Mathf.Abs(pp.getY())) / 255.0f);

            targetMaterial = MaterialCache[Mathf.Max(0, Mathf.CeilToInt(clr * MaterialCache.Length) - 1)];

            if (three_d)
            {
                float fy = (float)(0.1 * pp.getY() * y);

                //pixelRenderers[(plateSize / 2) + pp.getX(), (plateSize / 2) + pp.getZ()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) - pp.getX(), (plateSize / 2) + pp.getZ()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) - pp.getX(), (plateSize / 2) - pp.getZ()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) + pp.getX(), (plateSize / 2) - pp.getZ()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) + pp.getZ(), (plateSize / 2) + pp.getX()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) - pp.getZ(), (plateSize / 2) + pp.getX()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) - pp.getZ(), (plateSize / 2) - pp.getX()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) + pp.getZ(), (plateSize / 2) - pp.getX()].material = targetMaterial;

                vibrations[(plateSize / 2) + pp.getX(), (plateSize / 2) + pp.getZ()] = 1.0f - clr;
                vibrations[(plateSize / 2) - pp.getX(), (plateSize / 2) + pp.getZ()] = 1.0f - clr;
                vibrations[(plateSize / 2) - pp.getX(), (plateSize / 2) - pp.getZ()] = 1.0f - clr;
                vibrations[(plateSize / 2) + pp.getX(), (plateSize / 2) - pp.getZ()] = 1.0f - clr;
                vibrations[(plateSize / 2) + pp.getZ(), (plateSize / 2) + pp.getX()] = 1.0f - clr;
                vibrations[(plateSize / 2) - pp.getZ(), (plateSize / 2) + pp.getX()] = 1.0f - clr;
                vibrations[(plateSize / 2) - pp.getZ(), (plateSize / 2) - pp.getX()] = 1.0f - clr;
                vibrations[(plateSize / 2) + pp.getZ(), (plateSize / 2) - pp.getX()] = 1.0f - clr;

                //point(pp.getX(), pp.getY(), fy);
                //point(-pp.getX(), pp.getY(), fy);
                //point(-pp.getX(), -pp.getY(), fy);
                //point(pp.getX(), -pp.getY(), fy);
                //point(pp.getY(), pp.getX(), fy);
                //point(-pp.getY(), pp.getX(), fy);
                //point(-pp.getY(), -pp.getX(), fy);
                //point(pp.getY(), -pp.getX(), fy);
            }
            else
            {
                //pixelRenderers[(plateSize / 2) + pp.getX(), (plateSize / 2) + pp.getZ()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) - pp.getX(), (plateSize / 2) + pp.getZ()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) - pp.getX(), (plateSize / 2) - pp.getZ()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) + pp.getX(), (plateSize / 2) - pp.getZ()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) + pp.getZ(), (plateSize / 2) + pp.getX()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) - pp.getZ(), (plateSize / 2) + pp.getX()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) - pp.getZ(), (plateSize / 2) - pp.getX()].material = targetMaterial;
                //pixelRenderers[(plateSize / 2) + pp.getZ(), (plateSize / 2) - pp.getX()].material = targetMaterial;

                vibrations[(plateSize / 2) + pp.getX(), (plateSize / 2) + pp.getZ()] = 1.0f - clr;
                vibrations[(plateSize / 2) - pp.getX(), (plateSize / 2) + pp.getZ()] = 1.0f - clr;
                vibrations[(plateSize / 2) - pp.getX(), (plateSize / 2) - pp.getZ()] = 1.0f - clr;
                vibrations[(plateSize / 2) + pp.getX(), (plateSize / 2) - pp.getZ()] = 1.0f - clr;
                vibrations[(plateSize / 2) + pp.getZ(), (plateSize / 2) + pp.getX()] = 1.0f - clr;
                vibrations[(plateSize / 2) - pp.getZ(), (plateSize / 2) + pp.getX()] = 1.0f - clr;
                vibrations[(plateSize / 2) - pp.getZ(), (plateSize / 2) - pp.getX()] = 1.0f - clr;
                vibrations[(plateSize / 2) + pp.getZ(), (plateSize / 2) - pp.getX()] = 1.0f - clr;

                //point(pp.getX(), pp.getY());
                //point(-pp.getX(), pp.getY());
                //point(-pp.getX(), -pp.getY());
                //point(pp.getX(), -pp.getY());
                //point(pp.getY(), pp.getX());
                //point(-pp.getY(), pp.getX());
                //point(-pp.getY(), -pp.getX());
                //point(pp.getY(), -pp.getX());
            }
        }

        if (frameNr > Mathf.Floor(wfMax / waveIncrease))
        {
            Application.Quit();
        }

        if (photo)
        {
            //saveFrame("Exp2d/"+Math.floor(A*100)/100+"_"+Math.floor(k*100)/100+"/photo/"+frameNr+"-"+waveLengthFactor+".png");
            photo = false;
            frameNr++;
        }
        //saveFrame("Exp2d/"+Math.floor(A*100)/100+"_"+Math.floor(k*100)/100+"/movie/"+frameNr+"-"+waveLengthFactor+".png");

        if (!photo)
        {
            sumOfWholePlate2 = sum;
            if (sumOfWholePlate0 < sumOfWholePlate1 && sumOfWholePlate1 > sumOfWholePlate2 && !photo)
            {
                photo = true;
                //frameNr--;
            }

            sumOfWholePlate0 = sumOfWholePlate1;
            sumOfWholePlate1 = sumOfWholePlate2;
            //frameNr++;
        }
    }

    public void AddSand(Sand pSand)
    {
        pSand.gameObject.transform.SetParent(TargetPlane.transform);
        sand.Add(pSand);
        if (sand.Count > _MaxSand)
        {
            int sandIndex = Random.Range(0, sand.Count - (_MaxSand / 10));
            Destroy(sand[sandIndex].gameObject);
            sand.RemoveAt(sandIndex);
        }
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        draw();
        if (Input.GetKeyDown(KeyCode.O))
        {
            //Spawn sphere on plate.
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    GameObject grainOfSand = GameObject.Instantiate(SandPrefab, TargetPlane.transform);
                    grainOfSand.transform.localPosition = new Vector3((-TargetPlane.transform.localScale.x * 5) + i, 1, (-TargetPlane.transform.localScale.z * 5) + j);
                    AddSand(grainOfSand.GetComponent<Sand>());
                }
            }
        }

        float plateScaleX = TargetPlane.transform.localScale.x;
        float plateScaleZ = TargetPlane.transform.localScale.z;
        float plateOffsetX = plateScaleX * 5;
        float plateOffsetZ = plateScaleZ * 5;
        Vector3 localPos;
        int xIndex = 0;
        int yIndex = 0;
        for (int i = 0; i < sand.Count; i++)
        {
            localPos = sand[i].gameObject.transform.localPosition;
            if (localPos.y >= 0.0f)
            {
                xIndex = Mathf.Clamp((int)((localPos.x + plateOffsetX) / pixelSizeX) / 10, 0, plateSize - 1);
                yIndex = Mathf.Clamp((int)((localPos.z + plateOffsetZ) / pixelSizeZ) / 10, 0, plateSize - 1);

                if (vibrations[xIndex, yIndex] > 0.1f)
                {
                    sand[i].SetVelocity(new Vector3(Random.Range(-vibrations[xIndex, yIndex], vibrations[xIndex, yIndex]) * plateScaleX, sand[i].GetVelocity().y, Random.Range(-vibrations[xIndex, yIndex], vibrations[xIndex, yIndex]) * plateScaleZ));
                }
            }
            else
            {
                Destroy(sand[i].gameObject);
                sand.RemoveAt(i);
            }
        }
    }

    public float[,] GetVibrations()
    {
        return vibrations;
    }

    public void ResetPlate()
    {
        for (int i = sand.Count - 1; i >= 0; i--)
        {
            Destroy(sand[i].gameObject);
        }
        sand.Clear();
    }

    public void ChangeAmplitude(int pCounter)
    {
        //Write check here.
        if (pCounter >= frameNrArray.Length || pCounter < 0)
        { }
        else
        {
            frameNr = frameNrArray[pCounter];
            changedValue = true;
        }
    }
}

/*

import processing.opengl.*;

// This program is built by HUNG-YI YEH
// for Chladni Plate Simulation (2D wave interference)
//
// piggybio@me.com / teachbio@me.com
// I am a Taiwan people who love processing and physics.
// This project has been launched from 2009, after many times
// trouble shooting in Math, it is almost done.
*/
