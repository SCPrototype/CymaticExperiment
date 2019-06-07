using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fmodtest : MonoBehaviour
{
    public FMOD.Studio.EventInstance FmodTest;
    // Start is called before the first frame update
    void Start()
    {
        FmodTest = FMODUnity.RuntimeManager.CreateInstance(GLOB.LeverReleaseSound);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            //FmodTest.start();
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayArea/LeverRelease", this.transform.position);
        }
    }
}
