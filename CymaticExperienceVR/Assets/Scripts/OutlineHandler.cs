using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHandler : MonoBehaviour
{
    private MeshRenderer _myRenderer;
    //public Material[] MaterialsToSwap;
    private bool _showOutline = true;

    // Start is called before the first frame update
    void Start()
    {
        _myRenderer = GetComponent<MeshRenderer>();
    }

    public void ToggleOutline(bool pToggle)
    {
        _showOutline = pToggle;
        //for (int i = 0; i < _myRenderer.materials.Length; i++)
        //{
        //    if (pToggle)
        //    {
        //        _myRenderer.materials[i].SetFloat("_ShowOutline", 1);
        //    }
        //    else
        //    {
        //        _myRenderer.materials[i].SetFloat("_ShowOutline", 0);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _myRenderer.materials.Length; i++)
        {
            if (_showOutline)
            {
                _myRenderer.materials[i].SetFloat("_ShowOutline", 1);
            }
            else
            {
                _myRenderer.materials[i].SetFloat("_ShowOutline", 0);
            }
        }
    }
}
