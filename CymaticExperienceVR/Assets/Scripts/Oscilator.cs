using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    const int WaveDetailLevel = 100;
    public float LineWidth = 0.2f;

    private int _frequency = 1;
    private float _amplitude = 0.1f;
    private float _waveOffsetX = 0.0f;

    public Material LineMaterial;

    private Vector3[] audioWaveVertices = new Vector3[WaveDetailLevel];

    private Vector3 startVertex;
    private Vector3 endVertex;


    // Start is called before the first frame update
    void Start()
    {
        startVertex = transform.position + new Vector3(1.065f, 0, -0.425f);
        endVertex = transform.position + new Vector3(0.06f, 0, -0.425f);
        for (int i = 0; i < audioWaveVertices.Length; i++)
        {
            audioWaveVertices[i] = new Vector3(Mathf.Lerp(startVertex.x, endVertex.x, ((float)i / audioWaveVertices.Length)), 0.6f, startVertex.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < audioWaveVertices.Length; i++)
        {
            float freqOffset = Mathf.Sin((_waveOffsetX + audioWaveVertices[i].x) * _frequency) * _amplitude;
            audioWaveVertices[i].z = startVertex.z + freqOffset;
        }

        _waveOffsetX += 0.01f;
    }

    public void ChangeAmplitude(int pAmplitude)
    {
        _amplitude = (pAmplitude / 20.0f);
    }

    public void ChangeFrequency(int pFrequency)
    { 
        _frequency = pFrequency + 1;
        if(_frequency <= 0)
        {
            _frequency = 1;
        }
    }

    public void DrawLine()
    {
        if (!LineMaterial)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        LineMaterial.SetPass(0);
        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(GL.QUADS);
        for (int i = 0; i < audioWaveVertices.Length-1; i++)
        {
            GL.Vertex(audioWaveVertices[i] + new Vector3(0, 0, LineWidth*0.5f));
            GL.Vertex(audioWaveVertices[i] - new Vector3(0, 0, LineWidth * 0.5f));
            GL.Vertex(audioWaveVertices[i + 1] - new Vector3(0, 0, LineWidth * 0.5f));
            GL.Vertex(audioWaveVertices[i + 1] + new Vector3(0, 0, LineWidth * 0.5f));
        }

        GL.End();

        GL.PopMatrix();
    }

}
