using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeSound : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter _chimeSound;
    private FMOD.Studio.EventInstance _chimeInstance;
    public AudioSource _testSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _chimeSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        //_chimeSound.Event = GLOB.CelebrationSound;
        _chimeInstance = FMODUnity.RuntimeManager.CreateInstance(GLOB.CelebrationSound);
       
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_chimeInstance, this.gameObject.transform, GetComponent<Rigidbody>());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F6))
        {
            _chimeInstance.start();
        }
        if(Input.GetKeyDown(KeyCode.F7))
        {
            _testSource.Play();
        }
    }
}
