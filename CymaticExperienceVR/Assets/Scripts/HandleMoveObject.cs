using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMoveObject : MonoBehaviour
{
    public GameObject tableController;
    public GameObject tableObject;

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
            tableObject.transform.position = new Vector3(neutralPosTable.x, tableController.transform.localPosition.y, neutralPosTable.z);
            neutralPosTable = tableObject.transform.position;
            positionHasChanged = false;
        }
    }

    public void ChangeTablePosition(int pValue)
    {
        positionHasChanged = true;
    }
}
