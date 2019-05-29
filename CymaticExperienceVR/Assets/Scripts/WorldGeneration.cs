using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class WorldGeneration : MonoBehaviour
{
    [Header("Terrain settings")]
    public Material TerrainMaterial;
    public float EdgeLength;
    public Animator _cupulaAnimation;

    [Header("Sound settings")]
    public AudioSource CompletedSound;

    private MeshCollider myColl;

    private Vector3[] poly;  // Initialized in the inspector
    private float[,] _heightMap;
    private float _amplitude = 1;

    private bool _shouldPlayCompletedSound = false;


    [Header("Destructible object settings")]
    public bool PlaceObjects = true;
    public GameObject[] BuildingPool;
    public int BuildingCount = 50;
    public int BuildingGroupSize = 10;
    public int BuildingMaxAngle = 25;
    public int BuildingMaxHeight = 35;
    public GameObject[] ForestPool;
    public int ForestCount = 50;
    public int ForestGroupSize = 10;
    public int ForestMaxAngle = 45;
    public int ForestMaxHeight = 75;

    private List<GameObject> ObjectPool = new List<GameObject>();

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
        rend.material = TerrainMaterial;

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

        PopulateWorld();
    }

    private void PopulateWorld()
    {
        for (int i = 0; i < ObjectPool.Count; i++)
        {
            Destroy(ObjectPool[i]);
        }
        ObjectPool.Clear();

        int crashPrevent = 0;

        int BuildingsLeft = BuildingCount;
        RaycastHit hit;
        Ray newRay;

        while (BuildingsLeft > 0 && crashPrevent < 1000)
        {
            crashPrevent++;
            //Shoot a ray with a random X and Z downwards at our generated world.
            newRay = new Ray(new Vector3(transform.position.x + Random.Range(-250, 250), transform.position.y + 150, transform.position.z + Random.Range(-250, 250)), new Vector3(0, -300, 0));
            //If the ray hit something.
            if (Physics.Raycast(newRay, out hit))
            {
                //If the ray hit our terrain.
                if (hit.transform.gameObject == gameObject)
                {
                    //If the point is below the building height threshold.
                    if (hit.point.y - transform.position.y <= BuildingMaxHeight)
                    {
                        if (Vector3.Angle(transform.up, hit.normal) <= BuildingMaxAngle)
                        {
                            GameObject newBuilding = Instantiate(BuildingPool[Random.Range(0, BuildingPool.Length)], hit.point, new Quaternion(0, 0, 0, 0), transform);
                            newBuilding.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            newBuilding.transform.localScale = new Vector3(newBuilding.transform.localScale.x / transform.lossyScale.x, newBuilding.transform.localScale.y / transform.lossyScale.y, newBuilding.transform.localScale.z / transform.lossyScale.z);
                            newBuilding.transform.position += new Vector3(0, newBuilding.transform.lossyScale.y / 2, 0);
                            ObjectPool.Add(newBuilding);
                            BuildingsLeft--;
                        }
                    }
                }
            }
        }

        crashPrevent = 0;

        int ForestsLeft = ForestCount;

        while (ForestsLeft > 0 && crashPrevent < 1000)
        {
            crashPrevent++;
            //Shoot a ray with a random X and Z downwards at our generated world.
            newRay = new Ray(new Vector3(transform.position.x + Random.Range(-250, 250), transform.position.y + 150, transform.position.z + Random.Range(-250, 250)), new Vector3(0, -300, 0));
            //If the ray hit something.
            if (Physics.Raycast(newRay, out hit))
            {
                //If the ray hit our terrain.
                if (hit.transform.gameObject == gameObject)
                {
                    //If the point is below the forest height threshold.
                    if (hit.point.y - transform.position.y <= ForestMaxHeight)
                    {
                        if (Vector3.Angle(transform.up, hit.normal) <= ForestMaxAngle)
                        {
                            GameObject newForest = Instantiate(ForestPool[Random.Range(0, ForestPool.Length)], hit.point, new Quaternion(0, 0, 0, 0), transform);
                            newForest.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            newForest.transform.localScale = new Vector3(newForest.transform.localScale.x / transform.lossyScale.x, newForest.transform.localScale.y / transform.lossyScale.y, newForest.transform.localScale.z / transform.lossyScale.z);
                            newForest.transform.position += new Vector3(0, newForest.transform.lossyScale.y / 2, 0);
                            ObjectPool.Add(newForest);
                            ForestsLeft--;
                        }
                    }
                }
            }
        }
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
