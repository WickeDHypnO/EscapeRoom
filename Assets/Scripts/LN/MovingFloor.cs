using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour//Photon.PunBehaviour, IPunObservable
{
    public Vector3 MoveDistance = new Vector3(1.0f, 0.0f, 0.0f);
    public float MoveTime = 1.0f;
    private Vector3 initialPosition;
    private float elapsedTime;
    private float moveDirection;
    private bool moving;

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(elapsedTime);
            stream.SendNext(moveDirection);
            stream.SendNext(moving);
        }
        else
        {
            elapsedTime = (float)stream.ReceiveNext();
            moveDirection = (float)stream.ReceiveNext();
            moving = (bool)stream.ReceiveNext();
        }
    }*/

    public void MoveUp()
    {
        startMoving(1.0f);
    }

    public void MoveDown()
    {
        startMoving(-1.0f);
    }

    // Use this for initialization
    void Start ()
    {
        initialPosition = transform.position;
        moving = false;
        moveDirection = 1.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!moving) return;
        float delta = Time.deltaTime;
        Vector3 currentPosition = transform.position;
        Vector3 inc = moveDirection * (MoveDistance * delta / MoveTime);
        elapsedTime += delta;
        if (elapsedTime >= MoveTime)
        {
            if (moveDirection < 0.0f)
            {
                inc = initialPosition - currentPosition;
            }
            else
            {
                inc = initialPosition + MoveDistance - currentPosition;
            }
            moving = false;
        }
        transform.position = currentPosition + inc;
    }

    private void startMoving(float direction)
    {
        if (moving) return;
        elapsedTime = 0.0f;
        moveDirection = direction;
        moving = true;
    }
}
