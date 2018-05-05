using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : UsableTarget
{
    public GameObject buttonObject;
    public GameObject highlightObject;
    public float MoveTime = 1.0f;
    public float MoveDistance = 0.0099f;
    public UnityEvent onButtonPushed;
    private bool moving;
    private float elapsedTime;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(moving);
            stream.SendNext(elapsedTime);
        }
        else
        {
            moving = (bool)stream.ReceiveNext();
            elapsedTime = (float)stream.ReceiveNext();
        }
    }

    public override void Use()
    {
        if (moving) return;
        push();
    }

    private void push()
    {
        GetComponent<HighlightItem>().OutlineOff();
        highlightObject.SetActive(false);
        setPosition(MoveDistance);
        elapsedTime = 0.0f;
        moving = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!moving) return;
        float time = elapsedTime + Time.deltaTime;
        float distance = Time.deltaTime * MoveDistance / MoveTime;
        if (time >= MoveTime)
        {
            distance = (MoveTime - elapsedTime) * MoveDistance / MoveTime;
            moving = false;
            highlightObject.SetActive(true);
            GetComponent<HighlightItem>().OutlineOff();
            onButtonPushed.Invoke();
        }
        setPosition(-distance);
        elapsedTime = time;
    }

    private void setPosition(float zOffset)
    {
        Vector3 currentPos = buttonObject.transform.localPosition;
        float z = currentPos.z + zOffset;
        Vector3 newPos = new Vector3(currentPos.x, currentPos.y, z);
        buttonObject.transform.localPosition = newPos;
    }
}
