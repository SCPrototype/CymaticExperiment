using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMoveObject : MonoBehaviour
{
    public GameObject tableController;
    public GameObject sceneObjects;
    public Camera mainCameraVR;
    public Camera mainCameraSimulator;


    private bool positionHasChanged = false;
    private Vector3 neutralPosTable;
    // Start is called before the first frame update

    void Start()
    {
        neutralPosTable = this.transform.position;
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (mainCameraSimulator.isActiveAndEnabled == true)
            {
                float yPos = mainCameraSimulator.transform.position.y;
                sceneObjects.transform.position = new Vector3(sceneObjects.transform.position.x, yPos / 5, sceneObjects.transform.position.z);
            }
            else
            {
                float yPos = mainCameraVR.transform.position.y;
                sceneObjects.transform.position = new Vector3(sceneObjects.transform.position.x, yPos / 5, sceneObjects.transform.position.z);
            }
        }
    }

    public void ChangeTablePosition(int pValue)
    {
        positionHasChanged = true;
    }
}
