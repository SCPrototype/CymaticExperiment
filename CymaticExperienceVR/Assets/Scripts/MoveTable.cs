using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTable : MonoBehaviour
{
    public GameObject tableController;
    public GameObject tableObject;
    private bool positionHasChanged = false;
    private float _startingPOS = 0.7112985f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (positionHasChanged)
        { 
            tableObject.transform.position = new Vector3(-0.108901f, (float) tableController.transform.position.y - _startingPOS, 0.02523238f); 
            positionHasChanged = false;    
        }
    }

    public void ChangeTablePosition(int pValue)
    {
        positionHasChanged = true;
    }
}
