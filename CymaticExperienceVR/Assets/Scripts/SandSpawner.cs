using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSpawner : MonoBehaviour
{
    public GameObject _sandObject;
    Vector3 _spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.worldToLocalMatrix[1, 1] < 0)
        {

            _spawnPoint = (transform.position + (transform.up * transform.lossyScale.y));
            SpawnSand();
        }
    }

    void SpawnSand()
    {
       // Debug.Log(_spawnPoint);
        Instantiate(_sandObject, _spawnPoint, this.transform.rotation);

    }
}
