using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTable : MonoBehaviour
{
    public GameObject PlateFibrationTarget;
    private float _magnitude = 0.0005f;
    private float _magnitudeBase = 0.0005f;
    private float _stepIncrease = 0.00006f;
    private float _frequency = 1;
    private float _index = 1;
    private Vector3 neutralPosPlate;

    // Start is called before the first frame update
    void Start()
    {
        neutralPosPlate = PlateFibrationTarget.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float ShakeX = Random.Range(-1, 1) * _magnitude;
        float ShakeY = Random.Range(-1, 1) * _magnitude;
        PlateFibrationTarget.transform.position = new Vector3(neutralPosPlate.x + ShakeY, PlateFibrationTarget.transform.position.y , neutralPosPlate.z + ShakeX);
    }

    public void ChangeAmplitude(int pValue)
    {
        _magnitude = _magnitudeBase + (pValue * _stepIncrease);
    }

    public void ChangeFrequency(int pValue)
    {
        _frequency = pValue;
    }
}
