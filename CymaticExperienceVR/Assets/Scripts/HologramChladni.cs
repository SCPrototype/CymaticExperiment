using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramChladni : MonoBehaviour
{
    [Header("Terrain settings")]
    public Material TerrainMaterial;
    public float EdgeLength;
    public Chladni MyChladni;

    private Vector3[] poly;  // Initialized in the inspector
    private float[,] _heightMap;
    private float _frequency = -1;
    private float _amplitude = 1;

    // Start is called before the first frame update
    void Start()
    {
        CalculateHeight();
        GenerateWorld();
    }

    public void CalculateHeight()
    {
        _heightMap = (float[,])MyChladni.GetVibrations().Clone();
        if(_heightMap == null)
        {
            _heightMap = new float[100,100];
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    _heightMap[i,j] = Random.Range(0.0f, 0.01f);
                }
            }
        }

        poly = new Vector3[_heightMap.LongLength];
        int ArrayLength1 = _heightMap.GetLength(0);
        int ArrayLength2 = _heightMap.GetLength(1);
        int idx = 0;
        for (int i = 0; i < ArrayLength1; i++)
        {
            for (int j = 0; j < ArrayLength2; j++)
            {
                if (i == 0 || i == ArrayLength1 - 1 || j == 0 || j == ArrayLength2 - 1)
                {
                    poly[idx++] = new Vector3(i, -EdgeLength, j);
                }
                else
                {
                    poly[idx++] = new Vector3(i, _heightMap[i, j] * 2.0f * _amplitude, j);
                }
            }
        }
    }

    public void GenerateWorld()
    {
        MeshFilter mf;
        if (gameObject.GetComponent<MeshFilter>() == null)
        {
            mf = gameObject.AddComponent<MeshFilter>();
        } else
        {
            mf = gameObject.GetComponent<MeshFilter>();
        }

        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        Renderer rend;
        if (gameObject.GetComponent<MeshRenderer>() == null)
        {
            rend = gameObject.AddComponent<MeshRenderer>();
        }
        else
        {
            rend = gameObject.GetComponent<MeshRenderer>();
        }
        rend.material = TerrainMaterial;

        Vector3 center = new Vector3(37, 0, 37); //NOTE: This is hardcoded
        //Vector3 center = FindCenter();

        Vector3[] vertices = new Vector3[poly.Length + 1];
        vertices[0] = Vector3.zero;

        for (int i = 0; i < poly.Length; i++)
        {
            vertices[i + 1] = poly[i] - center;
        }

        mesh.vertices = vertices;

        int[] triangles = new int[poly.Length * 6];

        int ArrayLength1 = _heightMap.GetLength(0);
        int ArrayLength2 = _heightMap.GetLength(1);

        for (int i = 0; i < ArrayLength1 - 1; i++)
        {
            for (int j = 0; j < ArrayLength2; j++)
            {
                if (i == 0 && j == 0)
                {
                    triangles[0] = 1;
                    triangles[1] = ArrayLength1 + 1;
                    triangles[2] = ArrayLength1;
                    triangles[3] = ArrayLength1 + 1;
                    triangles[4] = ArrayLength1 * 2;
                    triangles[5] = ArrayLength1;
                }
                else
                {
                    triangles[(6 * ((i * ArrayLength2) + j))] = ((i * ArrayLength2) + j);
                    triangles[(6 * ((i * ArrayLength2) + j)) + 1] = ((i * ArrayLength2) + j) + 1;
                    triangles[(6 * ((i * ArrayLength2) + j)) + 2] = ((i * ArrayLength2) + j) + ArrayLength2 + 1;
                    triangles[(6 * ((i * ArrayLength2) + j)) + 3] = ((i * ArrayLength2) + j);
                    triangles[(6 * ((i * ArrayLength2) + j)) + 4] = ((i * ArrayLength2) + j) + ArrayLength2 + 1;
                    triangles[(6 * ((i * ArrayLength2) + j)) + 5] = ((i * ArrayLength2) + j) + ArrayLength2;
                }
            }
        }

        triangles[(poly.Length - 1) * 6 + 3] = poly.Length;
        triangles[(poly.Length - 1) * 6 + 4] = poly.Length - ArrayLength1;
        triangles[(poly.Length - 1) * 6 + 5] = poly.Length - ArrayLength1 + 1;

        mesh.triangles = triangles;
        mesh.uv = BuildUVs(vertices);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    Vector3 FindCenter()
    {
        Vector3 center = Vector3.zero;
        foreach (Vector3 v3 in poly)
        {
            center += v3;
        }
        center.y = 0;
        Debug.Log(center / poly.Length);
        return center / poly.Length;
    }

    Vector2[] BuildUVs(Vector3[] vertices)
    {
        float xMin = Mathf.Infinity;
        float yMin = Mathf.Infinity;
        float xMax = -Mathf.Infinity;
        float yMax = -Mathf.Infinity;

        foreach (Vector3 v3 in vertices)
        {
            if (v3.x < xMin)
                xMin = v3.x;
            if (v3.y < yMin)
                yMin = v3.y;
            if (v3.x > xMax)
                xMax = v3.x;
            if (v3.y > yMax)
                yMax = v3.y;
        }

        float xRange = xMax - xMin;
        float yRange = yMax - yMin;

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i].x = (vertices[i].x - xMin) / xRange;
            uvs[i].y = (vertices[i].y - yMin) / yRange;

        }
        return uvs;
    }

    public void SetAmplitude(int pAmplitude)
    {
        if (_amplitude != pAmplitude)
        {
            _amplitude = pAmplitude;
            CalculateHeight();
            GenerateWorld();
        }
    }
    public void SetFrequency(int pFrequency)
    {
        if (_frequency != pFrequency)
        {
            _frequency = pFrequency;
            CalculateHeight();
            GenerateWorld();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
