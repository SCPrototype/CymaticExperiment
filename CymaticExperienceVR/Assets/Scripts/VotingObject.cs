using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VotingObject : VR_Object
{
    public FeedbackHandler MyFeedbackHandler;
    public BoxCollider[] VotingStands;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        base.Update();
    }

    public void ForceRespawn()
    {
        HandleRespawn();
    }

    protected virtual void HandleRespawn(Transform pTarget = null)
    {
        rb.isKinematic = true;
        if (pTarget == null)
        {
            pTarget = RespawnPoint;
        }
        //Set object back to respawn point.
        transform.position = pTarget.position;
        transform.rotation = pTarget.rotation;
        _isOnSpawn = true;
        //Re-enable velocity.
        rb.isKinematic = false;
        _spawnTime = Time.time;
    }

    protected override void ObjectReleased(object sender, InteractableObjectEventArgs e)
    {
        for (int i = 0; i < VotingStands.Length; i++)
        {
            if (VotingStands[i].bounds.Intersects(GetComponent<Collider>().bounds))
            {
                HandleRespawn(VotingStands[i].transform);
                MyFeedbackHandler.StoreAnswer(i);
                return;
            }
        }

        base.ObjectReleased(sender, e);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < VotingStands.Length; i++)
        {
            if (other == VotingStands[i])
            {
                VotingStands[i].GetComponentInChildren<MeshRenderer>().enabled = true;
                break;
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < VotingStands.Length; i++)
        {
            if (other == VotingStands[i])
            {
                VotingStands[i].GetComponentInChildren<MeshRenderer>().enabled = false;
                break;
            }
        }
    }
}
