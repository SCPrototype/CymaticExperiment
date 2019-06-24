using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFade : MonoBehaviour
{
    public float FadeTime;

    public MeshRenderer MeshToFade;
    private Color baseColor;
    private bool meshIsVisible = false;
    private float fadeStartTime;
    private float meshStartAlpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        baseColor = MeshToFade.material.color;
        hideMesh();
    }

    public void ShowMesh()
    {
        if (!meshIsVisible)
        {
            MeshToFade.enabled = true;
            MeshToFade.material.color = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a);
            meshIsVisible = true;
            fadeStartTime = Time.time;
        }
        else
        {
            MeshToFade.material.color = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a);
            fadeStartTime = Time.time;
        }
    }

    private void hideMesh()
    {
        MeshToFade.enabled = false;
        meshIsVisible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (meshIsVisible)
        {
            if (MeshToFade.material.color.a > 0)
            {
                MeshToFade.material.color = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * (1 - ((Time.time - fadeStartTime) / FadeTime)));
            }
            else
            {
                MeshToFade.enabled = false;
                meshIsVisible = false;
            }
        }
    }
}
