using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableDoorController : UsableTarget
{
    public bool Locked = false;
    public float OpenAngle = 120;
    public float OpenTime = 1.0f;
    private Vector3 initialRotation;
    private bool opened;
    private float elapsedTime;
    private bool moving;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(opened);
            stream.SendNext(elapsedTime);
            stream.SendNext(moving);
        }
        else
        {
            opened = (bool)stream.ReceiveNext();
            elapsedTime = (float)stream.ReceiveNext();
            moving = (bool)stream.ReceiveNext();
        }
    }
    [PunRPC]
    private void RPCUse()
    {
        if (moving || Locked) return;
        startMoving();
    }
    public override void Use()
    {
        photonView.RPC("RPCUse", PhotonTargets.All);
    }

    public void Lock()
    {
        if (opened)
        {
            startMoving();
        }
        SetOutlineColour(HighlightColours.INACTIVE_COLOUR);
        Locked = true;
    }
    
    public void Unlock()
    {
        SetOutlineColour(HighlightColours.DEFAULT_COLOUR);
        Locked = false;
    }

    // Use this for initialization
    protected override void initialize()
    {
        initialRotation = transform.localRotation.eulerAngles;
        if (Locked)
        {
            SetOutlineColour(HighlightColours.INACTIVE_COLOUR);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!moving) return;
        float delta = Time.deltaTime;
        float dir = (opened ? -1.0f : 1.0f);
        float y = transform.localRotation.eulerAngles.y;
        float yInc = dir * (OpenAngle * delta / OpenTime);
        elapsedTime += delta;
        if (elapsedTime >= OpenTime)
        {
            if (opened)
            {
                yInc = initialRotation.y - y;
            }
            else
            {
                yInc = initialRotation.y + OpenAngle - y;
            }
            opened = !opened;
            moving = false;
        }
        y += yInc;
        transform.localRotation = Quaternion.Euler(initialRotation.x, y, initialRotation.z);
	}

    private void startMoving()
    {
        elapsedTime = 0.0f;
        moving = true;
    }
}
