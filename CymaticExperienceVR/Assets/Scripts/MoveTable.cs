using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTable : MonoBehaviour
{
    public GameObject tableController;
    public GameObject tableObject;
    public GameObject tableController2;
    private bool positionHasChanged = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (positionHasChanged)
        {
            if (tableController.transform.childCount != 0)
            {
                //Left one is selected.
                tableController2.transform.position = new Vector3(tableController2.transform.position.x, tableController.transform.position.y, tableController2.transform.position.z);
            }
            else
            {
                //Right one is selected.
                tableController.transform.position = new Vector3(tableController.transform.position.x, tableController2.transform.position.y, tableController.transform.position.z);
            }
            tableObject.transform.position = new Vector3(tableObject.transform.position.x, tableController.transform.position.y, tableObject.transform.position.z);

            positionHasChanged = false;
        }
    }

    public void ChangeTablePosition(int pValue)
    {
        positionHasChanged = true;
    }
}
