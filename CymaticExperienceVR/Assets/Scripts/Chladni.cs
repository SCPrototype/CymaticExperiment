using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chladni : MonoBehaviour
{
    //Changable variations
    public static int plateSize = 50;

    public GameObject TargetPlane;
    public GameObject PlatePixel;

    float start = 0.4f;         // a value for start simulation;
    float wfMax = 7.0f;         // a value for end up the simulation;
    float A = 0.5f;             // for Wave attenuation, should between 0 to 1;
    float k = 0.0f;             // for Wave source shift from plate center
    bool three_d = false;       // 2D/3D draw option;

    // unchangable variations
    GameObject[,] pixelGrid = new GameObject[plateSize, plateSize];
    List<Pixel> p;
    //ArrayList p;
    int R;
    float waveLengthFactor;
    float waveIncrease = 0.01f;
    bool photo = false;
    float sumOfWholePlate0, sumOfWholePlate1 = 0.0f, sumOfWholePlate2;
    float maxY = 0;
    float sum = 0;
    int frameNr = 1;


    // Start is called before the first frame update
    void Start()
    {
        sumOfWholePlate0 = sumOfWholePlate2 = plateSize * plateSize;
        R = (int)(-2.0 / Mathf.Log10(A)) + 1;
        prepare();
    }

    void prepare()
    {
        float pixelSizeX = TargetPlane.transform.localScale.x / plateSize;
        float pixelSizeZ = TargetPlane.transform.localScale.z / plateSize;
        for (int i = 0; i < plateSize; i++)
        {
            for (int j = 0; j < plateSize; j++)
            {

                pixelGrid[i, j] = GameObject.Instantiate(PlatePixel, TargetPlane.transform);
                pixelGrid[i, j].transform.localScale = new Vector3(pixelSizeX, 0.1f, pixelSizeZ);
                pixelGrid[i, j].transform.position = TargetPlane.transform.position + new Vector3(pixelSizeX * (i - (plateSize / 2)) * 10, 0.05f, pixelSizeZ * (j - (plateSize / 2)) * 10);
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
            
            //Pixel pp = (Pixel)p[i];
            p[i].interference(lamda);
            sum += Mathf.Abs(p[i].getY());
            maxY = Mathf.Abs(p[i].getY()) > maxY ? Mathf.Abs(p[i].getY()) : maxY;
        }
    }

    void draw()
    {
        //Debug.Log("frameNr=" + frameNr + "  R=" + R + "  waveLengthFactor=" + waveLengthFactor);

        waveLengthFactor = Mathf.Ceil(frameNr * waveIncrease * 100) / 100;
        doInterference();

        float y = 255.0f / maxY;

        Color pixelClr = new Color(0, 0, 0);

        for (int i = 0; i < p.Count; i++)
        {
            Pixel pp = p[i];

            float clr = (y * Mathf.Abs(pp.getY())) / 255.0f;


            if (photo)
            {
                pixelClr = new Color(clr, clr, clr);
            }
            else
            {
                if (pp.getY() > 0)
                {
                    pixelClr = new Color(clr, 0, 0);
                }
                if (pp.getY() < 0)
                {
                    pixelClr = new Color(0, 0, clr);
                }
            }

            if (three_d)
            {
                float fy = (float)(0.1 * pp.getY() * y);
                pixelGrid[pp.getX() + (plateSize/2), pp.getZ() + (plateSize / 2)].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[pp.getX(), pp.getZ() + (plateSize / 2)].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[pp.getX(), pp.getZ()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[pp.getX() + (plateSize / 2), pp.getZ()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[pp.getZ() + (plateSize / 2), pp.getX() + (plateSize / 2)].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[pp.getZ(), pp.getX() + (plateSize / 2)].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[pp.getZ(), pp.getX()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[pp.getZ() + (plateSize / 2), pp.getX()].GetComponent<MeshRenderer>().material.color = pixelClr;
                //point(pp.getX(), pp.getY(), fz);
                //point(-pp.getX(), pp.getY(), fz);
                //point(-pp.getX(), -pp.getY(), fz);
                //point(pp.getX(), -pp.getY(), fz);
                //point(pp.getY(), pp.getX(), fz);
                //point(-pp.getY(), pp.getX(), fz);
                //point(-pp.getY(), -pp.getX(), fz);
                //point(pp.getY(), -pp.getX(), fz);
            }
            else
            {
                pixelGrid[(plateSize / 2) + pp.getX(), (plateSize / 2) + pp.getZ()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[(plateSize / 2) - pp.getX(), (plateSize / 2) + pp.getZ()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[(plateSize / 2) - pp.getX(), (plateSize / 2) - pp.getZ()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[(plateSize / 2) + pp.getX(), (plateSize / 2) - pp.getZ()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[(plateSize / 2) + pp.getZ(), (plateSize / 2) + pp.getX()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[(plateSize / 2) - pp.getZ(), (plateSize / 2) + pp.getX()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[(plateSize / 2) - pp.getZ(), (plateSize / 2) - pp.getX()].GetComponent<MeshRenderer>().material.color = pixelClr;
                pixelGrid[(plateSize / 2) + pp.getZ(), (plateSize / 2) - pp.getX()].GetComponent<MeshRenderer>().material.color = pixelClr;

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
                //photo = true;
                //frameNr--;
            }

            sumOfWholePlate0 = sumOfWholePlate1;
            sumOfWholePlate1 = sumOfWholePlate2;
            frameNr++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        draw();
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

//Changable variations
double start = 0.4;         // a value for start simulation;
double wfMax = 1700.0;      // a value for end up the simulation;
double A = 0.5;             // for Wave attenuation, should between 0 to 1;
double k = 0.0;             // for Wave source shift from plate center
boolean three_d = false;    // 2D/3D draw option;

// unchangable variations
ArrayList p;
int R;
double lamda;
double waveLengthFactor;
double waveIncrease = 1.0;
boolean photo = false;
double sumOfWholePlate0, sumOfWholePlate1 = 0.0, sumOfWholePlate2;
double maxZ = 0;
double sum = 0;
int frameNr = 1;

void setup()
{
  size(251, 251, P2D);
  sumOfWholePlate0 = sumOfWholePlate2 = width*height;  
  R = (int)(-2.0/Math.log10(A))+1;
  prepare();
  smooth();
}

void prepare()
{
  p = new ArrayList();
  for (int i = 0; i < floor(height/2.0); i++)
  {
    for (int j = 0; j <= i; j++)
    {
      Pixel pp = new Pixel(j, i, R, A, k);
      double lamda = max(width, height)/(start+waveLengthFactor);
      pp.interference(lamda);
      p.add(pp);
    }
  }
}

void doInterference()
{
  sum = 0.0;
  maxZ = 0.0;
  for (int i = 0; i < p.size(); i++)
  {
    Pixel pp = (Pixel) p.get(i);
    double lamda = max(width, height)/(start+waveLengthFactor);
    pp.interference(lamda);
    sum += Math.abs(pp.z);
    maxZ = Math.abs(pp.z) > maxZ ? Math.abs(pp.z) : maxZ;
  }
}

void draw()
{
  println("frameNr="+frameNr+"  R="+R+"  waveLengthFactor="+waveLengthFactor);
  
  waveLengthFactor = Math.ceil(frameNr*waveIncrease*100)/100;
  doInterference();

  double z = 255/maxZ;

  background(0);
  if (three_d)
  {
    camera(width, height, width, 0, 0, 0, 0, 0, -1);
    strokeWeight(5);
  }
  else
  {
    translate(width/2, height/2);
    strokeWeight(1);
  }
  for (int i = 0; i < p.size(); i++)
  {
    Pixel pp = (Pixel) p.get(i);
    
    int clr = (int)(z*Math.abs(pp.z));

    if (photo)
    {
      stroke(clr);
    }
    else
    {
      if (pp.z > 0) stroke(clr, clr, clr);
      if (pp.z < 0) stroke(clr, clr, clr);
    }
    
    if (three_d)
    {
      float fz = (float)(0.1 * pp.z * z);
      point(pp.x, pp.y, fz);
      point(-pp.x, pp.y, fz);
      point(-pp.x, -pp.y, fz);
      point(pp.x, -pp.y, fz);
      point(pp.y, pp.x, fz);
      point(-pp.y, pp.x, fz);
      point(-pp.y, -pp.x, fz);
      point(pp.y, -pp.x, fz);
    }
    else
    {
      point(pp.x, pp.y);
      point(-pp.x, pp.y);
      point(-pp.x, -pp.y);
      point(pp.x, -pp.y);
      point(pp.y, pp.x);
      point(-pp.y, pp.x);
      point(-pp.y, -pp.x);
      point(pp.y, -pp.x);
    }
  }

  if (frameNr > Math.floor(wfMax/waveIncrease)) exit();

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
      //photo = true;
      //frameNr--;
    }
  
    sumOfWholePlate0 = sumOfWholePlate1;
    sumOfWholePlate1 = sumOfWholePlate2;
    frameNr++;
  }
}

*/
