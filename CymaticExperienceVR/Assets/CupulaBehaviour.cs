using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupulaBehaviour : MonoBehaviour
{
    public Animator _cupulaAnimator;
    private bool cupulaOpen = false;
    private int openBool;
    private int closeBool;
    // Start is called before the first frame update
    void Start()
    {
        openBool = Animator.StringToHash("OpenCupula");
        this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCupulaAnimation()
    {
        Debug.Log("Animation being called " + openBool);
        if (cupulaOpen)
        {
            _cupulaAnimator.SetBool(openBool, true);
            cupulaOpen = true;
        }
        else
        {
            _cupulaAnimator.SetBool(openBool, false);
            cupulaOpen = false;
        }
    }
}
