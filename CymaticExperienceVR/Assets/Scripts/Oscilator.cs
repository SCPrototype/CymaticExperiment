using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    private float _yPoint;
    private float _xPoint;
    private int _frequency = 1;
    private float _amplitude = 0.1f;
    private Vector2 _2dPosition;
    private MeshRenderer _mesh;
    private Texture2D _texture;

    private float WaveOffsetX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = GameObject.Find("Oscilator").GetComponent<MeshRenderer>();
        _texture = new Texture2D(1920, 20, TextureFormat.ARGB32, false);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.SetFloat("_WaveOffsetX", WaveOffsetX);
        WaveOffsetX += 0.01f;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeAmplitude((int)(_amplitude * 10) + 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeAmplitude((int)(_amplitude * 10) - 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeFrequency(_frequency + 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeFrequency(_frequency - 1);
        }
        //if (_xPoint < 1920)
        //{
        //    _xPoint += 0.2f;
        //} else
        //{
        //    _texture = new Texture2D(1920, 20, TextureFormat.ARGB32, false);
        //    _xPoint = 0;
        //}
        //_2dPosition = new Vector2(_xPoint ,CalculateYPoint(_xPoint, _frequency, _amplitude));
        //_texture.SetPixel((int)_2dPosition.x, (int)_2dPosition.y + 10, Color.green);
        //_texture.Apply();
        //_mesh.material.mainTexture = _texture;
    }

    private float CalculateYPoint(float pXpoint, float pFrequency, float pAmplitude)
    {
       _yPoint = Mathf.Sin(pXpoint * pFrequency) * pAmplitude;
        return _yPoint;
    }

    public void ChangeAmplitude(int pAmplitude)
    {
        _amplitude = (pAmplitude / 10.0f);
        //if(_amplitude <= 0)
        //{
        //    _amplitude = 0.1f;
        //}
        GetComponent<Renderer>().material.SetFloat("_Amplitude", _amplitude);
        Debug.Log(_amplitude + " amplitude");
    }

    public void ChangeFrequency(int pFrequency)
    { 
        _frequency = pFrequency + 1;
        if(_frequency <= 0)
        {
            _frequency = 1;
        }
        GetComponent<Renderer>().material.SetFloat("_Frequency", _frequency);
        Debug.Log(_frequency + " frequency");
    }
}
