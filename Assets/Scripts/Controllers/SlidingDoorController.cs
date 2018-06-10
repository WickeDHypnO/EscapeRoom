using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorController : Photon.PunBehaviour, IPunObservable
{
    public float ZOffset = -0.3f;
    public float XOffset = 1.0f;
    public float OpenTime = 0.5f;
    private Vector3 initialPosition;
    private bool opened;
    private bool moving;
    private float elapsedTime;
    private float xDistance;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(opened);
            stream.SendNext(moving);
            stream.SendNext(elapsedTime);
            stream.SendNext(xDistance);
        }
        else
        {
            opened = (bool)stream.ReceiveNext();
            moving = (bool)stream.ReceiveNext();
            elapsedTime = (float)stream.ReceiveNext();
            xDistance = (float)stream.ReceiveNext();
        }
    }

    public void OpenDoor()
    {
        if (moving || opened) return;
        startMoving();
        moveOnZ(1.0f);
    }

    public void CloseDoor()
    {
        if (moving || !opened) return;
        startMoving();
    }

    // Use this for initialization
    void Start ()
    {
        initialPosition = transform.localPosition;
        opened = false;
        moving = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!moving) return;
        Vector3 currentPos = transform.localPosition;
        float delta = Time.deltaTime;
        float dir = (opened ? -1.0f : 1.0f);
        float x = currentPos.x;
        float xInc = dir * (XOffset * delta / OpenTime);
        elapsedTime += delta;
        if (elapsedTime >= OpenTime)
        {
            if (opened)
            {
                xInc = initialPosition.x - x;
            }
            else
            {
                xInc = initialPosition.x + XOffset - x;
            }
            opened = !opened;
            moving = false;
        }
        x += xInc;
        transform.localPosition = new Vector3(x, currentPos.y, currentPos.z);
        if (!moving && !opened)
        {
            moveOnZ(-1.0f);
        }
    }

    private void startMoving()
    {
        moving = true;
        elapsedTime = 0.0f;
    }

    private void moveOnZ(float direction)
    {
        Vector3 currentPos = transform.localPosition;
        float z = currentPos.z + (direction * ZOffset);
        transform.localPosition = new Vector3(currentPos.x, currentPos.y, z);
    }
}
