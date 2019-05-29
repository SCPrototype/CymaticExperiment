using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(ParticleSystem))]
public class DestructibleObject : MonoBehaviour
{
    ParticleSystem myPart;
    MeshRenderer[] childRenderers;

    private bool isDestroyed = false;
    public float DisintegrateSpeed = 0.01f;
    private float disintegrateStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        childRenderers = GetComponentsInChildren<MeshRenderer>();
        myPart = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed)
        {
            disintegrateStep += DisintegrateSpeed;
            for (int i = 0; i < childRenderers.Length; i++)
            {
                for (int j = 0; j < childRenderers[i].materials.Length; j++)
                {
                    childRenderers[i].materials[j].SetFloat("_DisintegrateAmount", disintegrateStep);
                }
            }
            //myRend.material.SetFloat("_DisintegrateAmount", disintegrateStep);
            if (!myPart.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            myPart.Play();
            GetComponent<BoxCollider>().enabled = false;
            isDestroyed = true;
        }
    }
}
