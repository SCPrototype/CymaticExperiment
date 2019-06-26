using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutworldPopulator : MonoBehaviour
{
    [Header("Target settings")]
    [Tooltip("Scene objects that objects will be placed on.")]
    public GameObject[] Targets;
    [Tooltip("Bottom left of the area the raycasts will be fired in. As long as all 'Targets' fall within these GameObjects it should be fine.")]
    public Transform BottomLeftCorner;
    [Tooltip("Top right of the area the raycasts will be fired in. As long as all 'Targets' fall within these GameObjects it should be fine.")]
    public Transform TopRightCorner;

    [Header("Object settings")]
    public bool PlaceObjects = true;
    [Space(10)]
    [Tooltip("Objects to be placed as a building object.")]
    public GameObject[] BuildingPool;
    [Tooltip("Total amount of building objects to be placed.")]
    public int BuildingCount = 50;
    //[Tooltip("Amount of building objects that should be placed as a group.")]
    //public int BuildingGroupSize = 10;
    [Tooltip("The maximum angle of the surface that is acceptable for a building object.")]
    public int BuildingMaxAngle = 25;
    [Tooltip("The maximum height of the surface that is acceptable for a building object. Value is based on Y distance to this object in world space.")]
    public int BuildingMaxHeight = 35;
    [Space(10)]
    [Tooltip("Objects to be placed as a forest object.")]
    public GameObject[] ForestPool;
    [Tooltip("Total amount of forest objects to be placed.")]
    public int ForestCount = 50;
    //[Tooltip("Amount of forest objects that should be placed as a group.")]
    //public int ForestGroupSize = 10;
    [Tooltip("The maximum angle of the surface that is acceptable for a forest object.")]
    public int ForestMaxAngle = 45;
    [Tooltip("The maximum height of the surface that is acceptable for a forest object. Value is based on Y distance to this object in world space.")]
    public int ForestMaxHeight = 75;
    [Space(10)]
    [Tooltip("Objects to be placed as a misc object.")]
    public GameObject[] MiscPool;
    [Tooltip("Total amount of misc objects to be placed.")]
    public int MiscCount = 50;
    //[Tooltip("Amount of misc objects that should be placed as a group.")]
    //public int MiscGroupSize = 10;
    [Tooltip("The maximum angle of the surface that is acceptable for a misc object.")]
    public int MiscMaxAngle = 45;
    [Tooltip("The maximum height of the surface that is acceptable for a misc object. Value is based on Y distance to this object in world space.")]
    public int MiscMaxHeight = 75;

    [Header("Error settings")]
    [Tooltip("Amount of times placing an object can fail before aborting. Higher number causes a bigger lag spike, but will get closer to the requested amount of objects.")]
    public int CrashPreventCount = 1000;

    private List<GameObject> ObjectPool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (PlaceObjects)
        {
            PopulateWorld();
        }
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
            newRay = new Ray(new Vector3(Random.Range(BottomLeftCorner.position.x, TopRightCorner.position.x), TopRightCorner.position.y, Random.Range(BottomLeftCorner.position.z, TopRightCorner.position.z)), new Vector3(0, BottomLeftCorner.position.y - TopRightCorner.position.y, 0));
            //If the ray hit something.
            if (Physics.Raycast(newRay, out hit))
            {
                for (int i = 0; i < Targets.Length; i++)
                {
                    //If the ray hit our terrain.
                    if (hit.transform.gameObject == Targets[i])
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
                        break;
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
            newRay = new Ray(new Vector3(Random.Range(BottomLeftCorner.position.x, TopRightCorner.position.x), TopRightCorner.position.y, Random.Range(BottomLeftCorner.position.z, TopRightCorner.position.z)), new Vector3(0, BottomLeftCorner.position.y - TopRightCorner.position.y, 0));
            //If the ray hit something.
            if (Physics.Raycast(newRay, out hit))
            {
                for (int i = 0; i < Targets.Length; i++)
                {
                    //If the ray hit our terrain.
                    if (hit.transform.gameObject == Targets[i])
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
                        break;
                    }
                }
            }
        }

        crashPrevent = 0;

        int MiscLeft = MiscCount;

        while (MiscLeft > 0 && crashPrevent < 1000)
        {
            crashPrevent++;
            //Shoot a ray with a random X and Z downwards at our generated world.
            newRay = new Ray(new Vector3(Random.Range(BottomLeftCorner.position.x, TopRightCorner.position.x), TopRightCorner.position.y, Random.Range(BottomLeftCorner.position.z, TopRightCorner.position.z)), new Vector3(0, BottomLeftCorner.position.y - TopRightCorner.position.y, 0));
            //If the ray hit something.
            if (Physics.Raycast(newRay, out hit))
            {
                for (int i = 0; i < Targets.Length; i++)
                {
                    //If the ray hit our terrain.
                    if (hit.transform.gameObject == Targets[i])
                    {
                        //If the point is below the Misc height threshold.
                        if (hit.point.y - transform.position.y <= MiscMaxHeight)
                        {
                            if (Vector3.Angle(transform.up, hit.normal) <= MiscMaxAngle)
                            {
                                GameObject newMisc = Instantiate(MiscPool[Random.Range(0, MiscPool.Length)], hit.point, new Quaternion(0, 0, 0, 0), transform);
                                newMisc.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                                newMisc.transform.localScale = new Vector3(newMisc.transform.localScale.x / transform.lossyScale.x, newMisc.transform.localScale.y / transform.lossyScale.y, newMisc.transform.localScale.z / transform.lossyScale.z);
                                newMisc.transform.position += new Vector3(0, newMisc.transform.lossyScale.y / 2, 0);
                                ObjectPool.Add(newMisc);
                                MiscLeft--;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
