using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float Distance = 0.2f;
    public float Speed = 0.005f;
    public float RandomSpeedRange = 0.001f;
    public bool UseSine = false;

    private float lerpValue = 0;
    private bool goingUp = true;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        DoHover();
    }

    private void DoHover()
    {
        if (UseSine)
        {
            if (goingUp)
            {
                lerpValue = Mathf.Clamp(lerpValue + (Speed + Random.Range(-RandomSpeedRange, RandomSpeedRange)), 0.0f, Mathf.PI);
                if (lerpValue >= Mathf.PI)
                {
                    goingUp = false;
                }
            }
            else
            {
                lerpValue = Mathf.Clamp(lerpValue - (Speed + Random.Range(-RandomSpeedRange, RandomSpeedRange)), 0.0f, Mathf.PI);
                if (lerpValue <= 0)
                {
                    goingUp = true;
                }
            }
            transform.localPosition = startPosition + new Vector3(0, Mathf.Lerp(0.0f, Distance, (Mathf.Sin(lerpValue - (Mathf.PI/2)) + 1) / 2), 0);
        }
        else
        {
            if (goingUp)
            {
                lerpValue = Mathf.Clamp(lerpValue + (Speed + Random.Range(-RandomSpeedRange, RandomSpeedRange)), 0.0f, 1.0f);
                if (lerpValue >= 1)
                {
                    goingUp = false;
                }
            }
            else
            {
                lerpValue = Mathf.Clamp(lerpValue - (Speed + Random.Range(-RandomSpeedRange, RandomSpeedRange)), 0.0f, 1.0f);
                if (lerpValue <= 0)
                {
                    goingUp = true;
                }
            }

            transform.localPosition = startPosition + new Vector3(0, Mathf.Lerp(0.0f, Distance, lerpValue), 0);
        }
    }
}
