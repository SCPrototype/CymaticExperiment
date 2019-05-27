using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TiltMazeTablet : VR_Object
{
    public Transform TargetTerrain;
    public Vector3 RotationLimit = new Vector3(30, 0, 30);

    [Range(0.1f, 1.0f)]
    public float RotationSpeed = 0.1f;

    private Vector3 startRotation;

    public TextMesh scoreText;
    public TextMesh highscoreText;
    private bool setHighScore = false;
    private int highScore = 0;
    private int currentScore = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        startRotation = TargetTerrain.transform.eulerAngles;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (_isBeingGrabbed)
        {
            Vector3 eulerHolder = transform.eulerAngles;
            if (eulerHolder.x > 180)
            {
                eulerHolder.x -= 360;
            }
            if (eulerHolder.z > 180)
            {
                eulerHolder.z -= 360;
            }

            Vector3 terrainEulerHolder = TargetTerrain.transform.eulerAngles;
            if (terrainEulerHolder.x > 180)
            {
                terrainEulerHolder.x -= 360;
            }
            if (terrainEulerHolder.z > 180)
            {
                terrainEulerHolder.z -= 360;
            }

            //TargetTerrain.transform.eulerAngles = new Vector3(Mathf.Clamp(eulerHolder.x, -RotationLimit.x, RotationLimit.x), startRotation.y, Mathf.Clamp(eulerHolder.z, -RotationLimit.z, RotationLimit.z));
            TargetTerrain.transform.eulerAngles = Vector3.Lerp(terrainEulerHolder, new Vector3(Mathf.Clamp(eulerHolder.x, -RotationLimit.x, RotationLimit.x), terrainEulerHolder.y, Mathf.Clamp(eulerHolder.z, -RotationLimit.z, RotationLimit.z)), 0.1f);
        }
        else if (TargetTerrain.transform.eulerAngles != startRotation)
        {
            Vector3 eulerHolder = TargetTerrain.transform.eulerAngles;
            if (eulerHolder.x > 180)
            {
                eulerHolder.x -= 360;
            }
            if (eulerHolder.z > 180)
            {
                eulerHolder.z -= 360;
            }
            TargetTerrain.transform.eulerAngles = Vector3.Lerp(eulerHolder, startRotation, 0.1f);
            //TargetTerrain.transform.eulerAngles = startRotation;
        }
    }

    public void AddScore(int pAmount)
    {
        currentScore += pAmount;
        scoreText.text = "Huidige Score : " + currentScore;
        if (currentScore > highScore)
        {
            SetNewHighscore();
        }
    }

    private void SetNewHighscore()
    {
        highScore = currentScore;
        highscoreText.text = "Hoogste Score : " + highScore;

        if (!setHighScore)
        {
            //Put any victory effects here

            setHighScore = true;
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        setHighScore = false;
    }
}
