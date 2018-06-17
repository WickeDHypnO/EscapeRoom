using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UsableDoorController : UsableTarget {
    public bool Locked = false;
    public float OpenAngle = 120;
    public float OpenTime = 1.0f;
    private bool opened;
    private bool moving;

    public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext (opened);
        } else {
            opened = (bool) stream.ReceiveNext ();
        }
    }
    public override void Use () {
        if (moving || Locked) return;
        photonView.RPC ("RpcUseDoor", PhotonTargets.MasterClient);
    }

    public void Lock () {
        if (opened) {
            photonView.RPC ("RpcUseDoor", PhotonTargets.MasterClient);
        }
        photonView.RPC ("RpcLockDoor", PhotonTargets.All);
    }

    public void Unlock () {
        photonView.RPC ("RpcUnlockDoor", PhotonTargets.All);
    }

    protected override void initialize () {
        if (Locked) {
            SetOutlineColour (HighlightColours.INACTIVE_COLOUR);
        }
    }

    public void startMoving () {
        transform.DOLocalRotate (opened ? Vector3.zero : new Vector3 (0, OpenAngle, 0), OpenTime).OnComplete (() => opened = !opened);
    }

    [PunRPC]
    public void RpcUseDoor () {
        startMoving ();
    }

    [PunRPC]
    public void RpcUnlockDoor () {
        SetOutlineColour (HighlightColours.DEFAULT_COLOUR);
        Locked = false;
    }

    [PunRPC]
    public void RpcLockDoor () {
        SetOutlineColour (HighlightColours.INACTIVE_COLOUR);
        Locked = true;
    }
}