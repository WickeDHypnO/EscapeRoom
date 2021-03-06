﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableDrawerController : UsableTarget
{
    public bool Locked = false;
    public float OpenDistance = 1.0f;
    public float OpenTime = 1.0f;
    private Vector3 initialPosition;
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

    // Use this for initialization
    protected override void initialize()
    {
        initialPosition = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!moving) return;
        float delta = Time.deltaTime;
        float dir = (opened ? -1.0f : 1.0f);
        float z = transform.localPosition.z;
        float zInc = dir * (OpenDistance * delta / OpenTime);
        elapsedTime += delta;
        if (elapsedTime >= OpenTime)
        {
            if (opened)
            {
                zInc = initialPosition.z - z;
            }
            else
            {
                zInc = initialPosition.z + OpenDistance - z;
            }
            opened = !opened;
            moving = false;
        }
        z += zInc;
        transform.localPosition = new Vector3(initialPosition.x, initialPosition.y, z);
    }

    private void startMoving()
    {
        elapsedTime = 0.0f;
        moving = true;
    }
}
