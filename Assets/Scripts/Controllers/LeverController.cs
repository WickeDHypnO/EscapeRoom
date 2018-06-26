using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class LeverController : UsableTarget {

    public bool position;
    public bool canUse;
    public GameObject lever;
    public bool standalone;
    public float DownAngle = -35.0f;    // angle on lever down
    public float MoveTime = 0.25f;
    public UnityEvent onLeverDown;
    public UnityEvent onLeverUp;
    [PunRPC]
    private void RPCUse()
    {
        if (canUse)
        {
            startMoving();
        }

    }
	public override void Use () {
        photonView.RPC("RPCUse", PhotonTargets.All);
    }
	
    public void Reset()
    {
        position = true;
        startMoving();
    } 

    private void startMoving()
    {
        lever.transform.DOLocalRotate(position ? new Vector3(-DownAngle, 0.0f, 0.0f) : new Vector3(DownAngle, 0.0f, 0.0f), MoveTime).OnComplete(() => {
            position = !position;
            if (position) onLeverDown.Invoke();
            else onLeverUp.Invoke();
            });
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //stream.SendNext(position);
            stream.SendNext(canUse);
        }
        else
        {
            //position = (bool)stream.ReceiveNext();
            canUse = (bool)stream.ReceiveNext();
        }
    }

    public void SetCanUse(bool canUse)
    {
        this.canUse = canUse;
        SetOutlineColour(canUse ? HighlightColours.DEFAULT_COLOUR : HighlightColours.INACTIVE_COLOUR);
    }

    protected override void initialize()
    {
        if (!canUse)
        {
            SetOutlineColour(HighlightColours.INACTIVE_COLOUR);
        }
    }
}
