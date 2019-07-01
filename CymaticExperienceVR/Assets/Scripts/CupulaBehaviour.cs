using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupulaBehaviour : MonoBehaviour
{
    public Animator _cupulaAnimator;
    private bool cupulaOpen = false;
    private int openBool;
    private int closeBool;
    private FMODUnity.StudioEventEmitter _cupulaSound;
    private FMODUnity.StudioEventEmitter _outsideBackGroundSound;
    // Start is called before the first frame update
    void Start()
    {
        openBool = Animator.StringToHash("OpenCupula");
        _cupulaAnimator = this.gameObject.GetComponent<Animator>();
        _cupulaSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _cupulaSound.Event = GLOB.DomeOpeningSound;
        _outsideBackGroundSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _outsideBackGroundSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(new Vector3(-8,0,0)));
        _outsideBackGroundSound.Event = GLOB.OutsideWavesSound;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCupulaAnimation()
    {
        if (!cupulaOpen)
        {
            _cupulaSound.Play();
            _cupulaAnimator.SetBool(openBool, true);
            cupulaOpen = true;
            _outsideBackGroundSound.Play();
        }
    }
}
