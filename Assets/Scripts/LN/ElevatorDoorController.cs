using System;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorController : Photon.PunBehaviour, IPunObservable
{
    public bool Opened { get; private set; }
    public BoxCollider wholeDoorCollider;
    protected const string OPENED_STATE_NAME = "DoorOpened";
    protected const string OPEN_PARAMETER_NAME = "OpenDoor";
    protected bool isOpening;
    protected Animator animatorComponent;

    // Use this for initialization
    protected virtual void Start ()
    {
        animatorComponent = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        if (isOpening && /*animatorComponent.IsInTransition(0) && */animatorComponent.GetCurrentAnimatorStateInfo(0).IsName(OPENED_STATE_NAME))
        {
            isOpening = false;
            Opened = true;
            if (wholeDoorCollider != null)
            {
                wholeDoorCollider.enabled = false;
            }
        }
    }

    public void Open()
    {
        if (Opened) return;
        isOpening = true;
        animatorComponent.SetBool(OPEN_PARAMETER_NAME, true);
    }

    public void Close()
    {
        if (!Opened && !isOpening) return;
        if (wholeDoorCollider != null)
        {
            wholeDoorCollider.enabled = true;
        }
        Opened = false;
        animatorComponent.SetBool(OPEN_PARAMETER_NAME, false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(Opened);
            stream.SendNext(isOpening);
        }
        else
        {
            Opened = (bool)stream.ReceiveNext();
            isOpening = (bool)stream.ReceiveNext();
        }
    }
}
