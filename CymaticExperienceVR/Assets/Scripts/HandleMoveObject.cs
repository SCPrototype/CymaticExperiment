using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMoveObject : MonoBehaviour
{
    public GameObject tableController;
    public GameObject sceneObjects;
    public Camera mainCameraVR;


    private bool positionHasChanged = false;
    private Vector3 neutralPosTable;
    // Start is called before the first frame update

    void Start()
    {
        neutralPosTable = this.transform.position;
        Debug.Log(mainCameraVR.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (positionHasChanged)
        {
            sceneObjects.transform.position = new Vector3(neutralPosTable.x, neutralPosTable.y + tableController.transform.localPosition.y, neutralPosTable.z);
            //neutralPosTable = tableObject.transform.position;
            positionHasChanged = false;
        }
        if(Input.GetKeyDown(KeyCode.T))
        {

        }
    }

    public void ChangeTablePosition(int pValue)
    {
        positionHasChanged = true;
    }
}
