using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotatingHandleController : ConstantUsableTarget
{
    [System.Serializable]
    public class HandleRotationEvent : UnityEvent<float> {};

    public float MaximumAngle = 1080.0f;
    public float Turn90Time = 1.0f;
    public float ReverseTimeMultiplier = 4.0f;
    public bool ClockwiseRotate;
    public GameObject HighlightObject;
    public HandleRotationEvent OnHandleRotate;
    private bool used;
    private float startingAngle;
    private float maximumAngle;
    private bool canMoveUp;
    private bool canMoveDown;
    private float angleZ;
    private PhotonView view;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(used);
            stream.SendNext(angleZ);
        }
        else
        {
            used = (bool)stream.ReceiveNext();
            angleZ = (float)stream.ReceiveNext();
        }
    }

    public override void Use()
    {
        if (view.ownerId != PhotonNetwork.player.ID)
        {
            view.TransferOwnership(PhotonNetwork.player.ID);
        }
        used = true;
    }

    // Use this for initialization
    void Start ()
    {
        startingAngle = transform.rotation.eulerAngles.z;
        maximumAngle = startingAngle + MaximumAngle;
        canMoveUp = true;
        angleZ = startingAngle;
        view = GetComponent<PhotonView>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID)
        {
            float clockwise = (ClockwiseRotate ? -1.0f : 1.0f);
            if (used)
            {
                if (canMoveUp)
                {
                    rotateHandle(clockwise * 1.0f);
                }
                canMoveDown = true;
                used = false;
            }
            // Powrót do pozycji początkowej
            else
            {
                if (canMoveDown)
                {
                    rotateHandle(-clockwise * ReverseTimeMultiplier);
                }
                canMoveUp = true;
            }
        }
	}

    /*
     * direction:
     * > 0 -> obracanie w kierunku przeciwnym do kierunku ruchu wskazówek zegara
     * < 0 -> obracacanie w kierunku zgodnym z kierunkiem ruchu wskazówek zegara
     */
    private void rotateHandle(float direction)
    {
        Vector3 currentRot = transform.rotation.eulerAngles;
        float rotation = (Time.deltaTime * 90.0f / Turn90Time) * direction;
        angleZ = angleZ + rotation;
        if (angleZ > maximumAngle)
        {
            angleZ = maximumAngle;
            canMoveUp = false;
        }
        else if (angleZ < startingAngle)
        {
            angleZ = startingAngle;
            canMoveDown = false;
        }
        transform.rotation = Quaternion.Euler(currentRot.x, currentRot.y, angleZ);
        if (HighlightObject != null)
        {
            HighlightObject.transform.rotation = Quaternion.Euler(currentRot.x, currentRot.y, angleZ);
        }
        OnHandleRotate.Invoke(angleZ);
    }
}
