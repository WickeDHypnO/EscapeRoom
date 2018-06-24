using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstantButtonController : ConstantUsableTarget {

    public float MoveDistance = 0.0099f;
    /*
     * Complex == true -> obiekt, do którego podpięty jest skrypt nie jest prawdziwym przyciskiem, a jest nim obiekt przypisany pod ActualButtonObject
     * Complex == false -> obiekt, do którego podpięty jest ten skrypt jest przyciskiem
     */
    public bool Complex = false;
    public GameObject ActualButtonObject;

    public bool synchronizeAction = true;

    public UnityEvent OnButtonPushed;
    public UnityEvent OnButtonReleased;
    private Vector3 startingPosition;
    private bool pressed;
    private bool released;

    // Use this for initialization
    void Start () {
        if (Complex)
        {
            startingPosition = ActualButtonObject.transform.localPosition;
        }
        else
        {
            startingPosition = transform.localPosition;
        }
        pressed = false;
        released = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (pressed)
        {
            push();
            pressed = false;
        }
        else if (!released)
        {
            release();
        }
	}

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*if (stream.isWriting)
        {
            stream.SendNext(pressed);
        }
        else
        {
            pressed = (bool)stream.ReceiveNext();
        }*/
    }

    public override void Use()
    {
        pressed = true;
    }

    public void push()
    {
        float y = startingPosition.y - MoveDistance;
        Vector3 newPosition = new Vector3(startingPosition.x, y, startingPosition.z);
        if (Complex)
        {
            ActualButtonObject.transform.localPosition = newPosition;
        }
        else
        {
            transform.localPosition = newPosition;
        }
        if (released)
        {
            if(synchronizeAction)
            {
                photonView.RPC("RPCPush", PhotonTargets.All);
            }
            else
            {
                RPCPush();
            }
        }
    }

    public void release()
    {
        if (Complex)
        {
            ActualButtonObject.transform.localPosition = startingPosition;
        }
        else
        {
            transform.localPosition = startingPosition;
        }
        if(synchronizeAction)
        {
            photonView.RPC("RPCRelease", PhotonTargets.All);
        }
        else
        {
            RPCRelease();
        }

    }

    [PunRPC]
    public void RPCPush()
    {
        released = false;
        OnButtonPushed.Invoke();
    }

    [PunRPC]
    public void RPCRelease()
    {
        released = true;
        OnButtonReleased.Invoke();
    }
}
