using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartridge : MonoBehaviour
{
    public Camera mainCamera;
    private Texture2D _screenShot;
    private float[,] _heightMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPattern(Chladni pChladni)
    {
        _heightMap = (float[,])pChladni.GetVibrations().Clone();

        RenderTexture rt = new RenderTexture(1920, 1080, 24);
        mainCamera.targetTexture = rt;
        _screenShot = new Texture2D(1080, 1080, TextureFormat.RGB24, false);
        mainCamera.Render();
        RenderTexture.active = rt;
        _screenShot.ReadPixels(new Rect(420, 0, 1080, 1080), 0, 0);
        _screenShot.Apply();
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        string filename = "meh";

        //byte[] bytes = _screenShot.EncodeToPNG();
        //System.IO.File.WriteAllBytes(filename, bytes);

        Debug.Log(string.Format("Took screenshot to: {0}", filename));

        GameObject.Find("Cartridge").GetComponent<MeshRenderer>().material.mainTexture = _screenShot;
    }

    public float[,] GetHeightMap()
    {
        return _heightMap;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
