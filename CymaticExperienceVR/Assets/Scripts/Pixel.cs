using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixel
{
    int x, z;
    float y;
    int m;
    float[] a;
    float[] distances;

    public Pixel(int x, int z, int m, float A, float k)
    {
        this.x = x;
        this.z = z;
        this.m = m;
        int nrDist = 2 * m * m + 2 * m + 1;
        this.distances = new float[nrDist];
        this.a = new float[nrDist];
        calculateDistance(A, k);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void calculateDistance(float A, float kk)
    {
        int k = 0;
        for (int i = 0; i <= this.m; i++)
        {
            for (int j = i; j >= i - Mathf.Floor(i / 2); j--)
            {
                float w1 = j * Chladni.plateSize;
                float h1 = (i - j) * Chladni.plateSize;
                float w2 = (i - j) * Chladni.plateSize;
                float h2 = j * Chladni.plateSize;
                if (i == 0)
                {
                    this.distances[k++] = Mathf.Sqrt(this.x * this.x + this.z * this.z) - kk;
                }
                else if (j == i)
                {
                    this.distances[k++] = Mathf.Sqrt((w1 + this.x) * (w1 + this.x) + (this.z) * (this.z)) - kk;
                    this.distances[k++] = Mathf.Sqrt((w1 - this.x) * (w1 - this.x) + (this.z) * (this.z)) - kk;
                    this.distances[k++] = Mathf.Sqrt((this.x) * (this.x) + (h2 + this.z) * (h2 + this.z)) - kk;
                    this.distances[k++] = Mathf.Sqrt((this.x) * (this.x) + (h2 - this.z) * (h2 - this.z)) - kk;
                }
                else
                {
                    this.distances[k++] = Mathf.Sqrt((w1 + this.x) * (w1 + this.x) + (h1 + this.z) * (h1 + this.z)) - kk;
                    this.distances[k++] = Mathf.Sqrt((w1 - this.x) * (w1 - this.x) + (h1 + this.z) * (h1 + this.z)) - kk;
                    this.distances[k++] = Mathf.Sqrt((w1 + this.x) * (w1 + this.x) + (h1 - this.z) * (h1 - this.z)) - kk;
                    this.distances[k++] = Mathf.Sqrt((w1 - this.x) * (w1 - this.x) + (h1 - this.z) * (h1 - this.z)) - kk;
                    if (j != i - j)
                    {
                        this.distances[k++] = Mathf.Sqrt((w2 + this.x) * (w2 + this.x) + (h2 + this.z) * (h2 + this.z)) - kk;
                        this.distances[k++] = Mathf.Sqrt((w2 - this.x) * (w2 - this.x) + (h2 + this.z) * (h2 + this.z)) - kk;
                        this.distances[k++] = Mathf.Sqrt((w2 + this.x) * (w2 + this.x) + (h2 - this.z) * (h2 - this.z)) - kk;
                        this.distances[k++] = Mathf.Sqrt((w2 - this.x) * (w2 - this.x) + (h2 - this.z) * (h2 - this.z)) - kk;
                    }
                }
            }
        }

        for (int i = 0; i < distances.Length; i++) a[i] = Mathf.Pow(A, distances[i] / Chladni.plateSize);
    }

    public float interference(float lamda)
    {
        y = 0.0f;
        for (int i = 0; i < distances.Length; i++)
        {
            y += a[i] * (float)System.Math.Sin(2.0f * Mathf.PI * (distances[i] - (int)(distances[i] / lamda) * lamda) / lamda);
        }
        return y;
    }

    public int getX()
    {
        return x;
    }
    public float getY()
    {
        return y;
    }
    public int getZ()
    {
        return z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}