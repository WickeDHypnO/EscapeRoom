using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MultiSwitchController : UsableTarget
{

    public bool position;
    public bool canUse;
    public GameObject mSwitch;
    public bool standalone;
    public UnityEvent onMSwitchDown;
    public UnityEvent onMSwitchUp;

    public override void Use()
    {
        if (canUse)
        {
            if (!position)
            {
                StartCoroutine(MoveDown());
            }
            else
            {
                StartCoroutine(MoveUp());
            }
        }
    }

    IEnumerator MoveDown()
    {
        float currentPosition = mSwitch.transform.localPosition.z;
        canUse = false;
        while (mSwitch.transform.localPosition.z < 0.03f)
        {
            yield return new WaitForSeconds(0.01f);
            mSwitch.transform.localPosition = new Vector3(mSwitch.transform.localPosition.x, mSwitch.transform.localPosition.y, currentPosition);
            currentPosition += 0.005f;
        }
        canUse = true;
        position = true;
        onMSwitchDown.Invoke();
    }

    public IEnumerator MoveUp()
    {
        float currentPosition = mSwitch.transform.localPosition.z;
        canUse = false;
        while (mSwitch.transform.localPosition.z > -0.03f)
        {
            yield return new WaitForSeconds(0.01f);
            mSwitch.transform.localPosition = new Vector3(mSwitch.transform.localPosition.x, mSwitch.transform.localPosition.y, currentPosition);
            currentPosition -= 0.005f;
        }
        canUse = true;
        position = false;
        onMSwitchUp.Invoke();
    }

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


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
