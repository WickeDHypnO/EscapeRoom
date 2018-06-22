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
            /*if (!position)
            {
                StartCoroutine(MoveDown());
            }
            else
            {
                StartCoroutine(MoveUp());
            }*/
            startMoving();
        }

    }
	public override void Use () {
        photonView.RPC("RPCUse", PhotonTargets.All);
    }
	
    public void Reset()
    {
        //StartCoroutine(MoveUp());
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

    /*IEnumerator MoveDown()
    {
        float currentRotation = lever.transform.localEulerAngles.x;
        canUse = false;
        while(lever.transform.localEulerAngles.x > 325f || lever.transform.localEulerAngles.x < 300f)
        {
            yield return new WaitForSeconds(0.01f);
            lever.transform.localEulerAngles = new Vector3(currentRotation, 0, 0);
            currentRotation -= 3.5f;
        }
        canUse = true;
        position = true;
        onLeverDown.Invoke();
    }

    public IEnumerator MoveUp()
    {
        float currentRotation = lever.transform.localEulerAngles.x;
        canUse = false;
        position = false;
        while (lever.transform.localEulerAngles.x < 35f || lever.transform.localEulerAngles.x > 300f)
        {
            yield return new WaitForSeconds(0.01f);
            lever.transform.localEulerAngles = new Vector3(currentRotation, 0, 0);
            currentRotation += 3.5f;
        }
        canUse = true;
        onLeverUp.Invoke();
    }*/

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(position);
            stream.SendNext(canUse);
        }
        else
        {
            position = (bool)stream.ReceiveNext();
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
