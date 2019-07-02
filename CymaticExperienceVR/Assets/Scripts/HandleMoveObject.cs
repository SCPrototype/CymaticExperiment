using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMoveObject : MonoBehaviour
{
    public GameObject tableController;
    public GameObject sceneObjects;
    public Camera mainCameraVR;
    public Camera mainCameraSimulator;
    public GameObject allHolder;

    private bool positionHasChanged = false;
    private Vector3 neutralPosTable;
    private bool _automaticSetHeight = false;
    private float _heightSetTime = 0;
    private bool eyeHeightSet = false;
    private float _startTime;
    // Start is called before the first frame update

    void Start()
    {
        neutralPosTable = this.transform.position;
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_automaticSetHeight)
        {
            if (_heightSetTime + 0.5f < Time.time)
            {
                _automaticSetHeight = false;
            }
        }

        if (positionHasChanged)
        {
            sceneObjects.transform.position = new Vector3(neutralPosTable.x, neutralPosTable.y + tableController.transform.localPosition.y, neutralPosTable.z);
            positionHasChanged = false;
        }
        if (!eyeHeightSet && Time.time > _startTime + 0.5f)
        {
            SetEyeState();
            eyeHeightSet = true;
        }
    }

    private void SetEyeState()
    {
        float yPos = 0;
        if (mainCameraSimulator.isActiveAndEnabled == true)
        {
            yPos = mainCameraSimulator.transform.position.y;
        }
        else
        {
            yPos = mainCameraVR.transform.position.y;
        }
        allHolder.transform.position = new Vector3(allHolder.transform.position.x, Mathf.Clamp(yPos / 5.0f, -50.0f, 0.465f), allHolder.transform.position.z);
        neutralPosTable = this.transform.position;
        _automaticSetHeight = true;
        _heightSetTime = Time.time;
    }

    public void ChangeTablePosition(int pValue)
    {
        if (_automaticSetHeight == false)
        {
            positionHasChanged = true;
        }
    }
}
