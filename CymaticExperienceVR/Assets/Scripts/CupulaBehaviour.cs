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
    // Start is called before the first frame update
    void Start()
    {
        openBool = Animator.StringToHash("OpenCupula");
        _cupulaAnimator = this.gameObject.GetComponent<Animator>();
        _cupulaSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _cupulaSound.Event = GLOB.DomeOpeningSound;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCupulaAnimation()
    {
        if (!cupulaOpen)
        {
            _cupulaAnimator.SetBool(openBool, true);
            cupulaOpen = true;
        }
    }
}
