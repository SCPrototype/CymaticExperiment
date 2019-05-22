using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTable : MonoBehaviour
{
    public GameObject tableController;
    public GameObject tableObject;
    public GameObject tableHeight;
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
            //Easy way, dragging the Y of the full table.
            tableObject.transform.position = new Vector3(tableObject.transform.position.x, tableController.transform.position.y, tableObject.transform.position.z);
            positionHasChanged = false;
            /*
            You use AnimationState.time to jump to a specific time in an animation.
            animation["MyAnimation"].time = 5.0;
            Idea jelmer had:
            Get value from ChanggeTablePosition(int)
            Then use that INT to jump to a specific frame within the animation. 
            Might get some complications if there's some frames which don't line up in height in the animation.
            */
        }
    }

    public void ChangeTablePosition(int pValue)
    {
        positionHasChanged = true;
    }
}
