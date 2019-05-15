using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    private float _yPoint;
    private float _xPoint;
    private float _frequency = 1;
    private float _amplitude = 1;
    private Vector2 _2dPosition;
    private MeshRenderer _mesh;
    private Texture2D _texture;
    // Start is called before the first frame update
    void Start()
    {
        _mesh = GameObject.Find("Oscilator").GetComponent<MeshRenderer>();
        _texture = new Texture2D(50, 20, TextureFormat.ARGB32, false);
    }

    // Update is called once per frame
    void Update()
    {
        if(_xPoint < 50)
        {
            _xPoint += 0.5f;
        } else
        {
            _xPoint = 0;
        }
        _2dPosition = new Vector2(_xPoint ,CalculateYPoint(_xPoint, _frequency, _amplitude));
        _texture.SetPixel((int)_2dPosition.x, (int)_2dPosition.y + 10, Color.green);
        _texture.Apply();
        _mesh.material.mainTexture = _texture;
        Debug.Log("Set texture" + _2dPosition);
    }

    private float CalculateYPoint(float pXpoint, float pFrequency, float pAmplitude)
    {
       _yPoint = Mathf.Sin(pXpoint * pFrequency) * pAmplitude;
        return _yPoint;
    }

    public void ChangeAmplitude(int pAmplitude)
    {
        _texture = new Texture2D(50, 20, TextureFormat.ARGB32, false);
        _frequency = pAmplitude;
        if(_frequency <= 0)
        {
            _frequency = 1;
        }
    }

    public void ChangeFrequency(int pFrequency)
    {
        _texture = new Texture2D(50, 20, TextureFormat.ARGB32, false);
        _amplitude = pFrequency;
        if(_amplitude <= 0)
        {
            _amplitude = 1;
        }
    }
}
