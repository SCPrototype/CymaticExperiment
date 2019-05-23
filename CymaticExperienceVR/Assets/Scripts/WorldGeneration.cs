using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class WorldGeneration : MonoBehaviour
{
    public Material mat;
    public AudioSource CompletedSound;
    public float EdgeLength;

    private MeshCollider myColl;

    private Vector3[] poly;  // Initialized in the inspector
    private float[,] _heightMap;
    private float _amplitude = 1;

    private bool _shouldPlayCompletedSound = false;

    // Start is called before the first frame update
    void Start()
    {
        myColl = GetComponent<MeshCollider>();

        InputCartridge();
        GenerateWorld();
    }

    public void InputCartridge(Cartridge pCartridge = default(Cartridge))
    {
        if (pCartridge != default(Cartridge))
        {
            _heightMap = pCartridge.GetHeightMap();
        } else
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
        if (_heightMap == null)
        {
            InputCartridge();
            return;
        }

        poly = new Vector3[_heightMap.LongLength];
        int idx = 0;
        for (int i = 0; i < _heightMap.GetLength(0); i++)
        {
            for (int j = 0; j < _heightMap.GetLength(1); j++)
            {
                if (i == 0 || i == _heightMap.GetLength(0) - 1 || j == 0 || j == _heightMap.GetLength(1) - 1)
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
        rend.material = mat;

        Vector3 center = FindCenter();

        Vector3[] vertices = new Vector3[poly.Length + 1];
        vertices[0] = Vector3.zero;

        for (int i = 0; i < poly.Length; i++)
        {
            vertices[i + 1] = poly[i] - center;
        }

        mesh.vertices = vertices;

        int[] triangles = new int[poly.Length * 6];

        for (int i = 0; i < _heightMap.GetLength(0)-1; i++)
        {
            for (int j = 0; j < _heightMap.GetLength(1); j++)
            {
                if (i == 0 && j == 0)
                {
                    triangles[0] = 1;
                    triangles[1] = _heightMap.GetLength(0) + 1;
                    triangles[2] = _heightMap.GetLength(0);
                    triangles[3] = _heightMap.GetLength(0) + 1;
                    triangles[4] = _heightMap.GetLength(0) * 2;
                    triangles[5] = _heightMap.GetLength(0);
                }
                else
                {
                    triangles[(6 * ((i * _heightMap.GetLength(1)) + j))] = ((i * _heightMap.GetLength(1)) + j);
                    triangles[(6 * ((i * _heightMap.GetLength(1)) + j)) + 1] = ((i * _heightMap.GetLength(1)) + j) + 1;
                    triangles[(6 * ((i * _heightMap.GetLength(1)) + j)) + 2] = ((i * _heightMap.GetLength(1)) + j) + _heightMap.GetLength(1) + 1;
                    triangles[(6 * ((i * _heightMap.GetLength(1)) + j)) + 3] = ((i * _heightMap.GetLength(1)) + j);
                    triangles[(6 * ((i * _heightMap.GetLength(1)) + j)) + 4] = ((i * _heightMap.GetLength(1)) + j) + _heightMap.GetLength(1) + 1;
                    triangles[(6 * ((i * _heightMap.GetLength(1)) + j)) + 5] = ((i * _heightMap.GetLength(1)) + j) + _heightMap.GetLength(1);
                }
            }
        }

        triangles[(poly.Length - 1) * 6 + 3] = poly.Length;
        triangles[(poly.Length - 1) * 6 + 4] = poly.Length - _heightMap.GetLength(0);
        triangles[(poly.Length - 1) * 6 + 5] = poly.Length - _heightMap.GetLength(0) + 1;

        mesh.triangles = triangles;
        mesh.uv = BuildUVs(vertices);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        if (_shouldPlayCompletedSound)
        {
            CompletedSound.Play();
        } else
        {
            _shouldPlayCompletedSound = true;
        }

        myColl.sharedMesh = mesh;
    }

    Vector3 FindCenter()
    {
        Vector3 center = Vector3.zero;
        foreach (Vector3 v3 in poly)
        {
            center += v3;
        }
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
        _amplitude = pAmplitude;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
