using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleElevatorController : Photon.PunBehaviour, IPunObservable
{

    [SerializeField] private float m_maxHeight;
    [SerializeField] private float m_minHeight;

    [SerializeField] private float m_speed;

    public bool m_isMovingUp;
    public bool m_isMovingDown;

    // Use this for initialization
    void Start ()
    {
        m_isMovingUp = false;
        m_isMovingDown = false;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if(m_isMovingUp && !m_isMovingDown && GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID)
        {
            float height = Mathf.Min(m_maxHeight, this.transform.position.y + Time.fixedDeltaTime * m_speed);
            this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);
        }

        if (!m_isMovingUp && m_isMovingDown)
        {
            float height = Mathf.Max(m_minHeight, this.transform.position.y - Time.fixedDeltaTime * m_speed);
            this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);
        }
    }
    [PunRPC]
    public void RPCMoveUp(bool move)
    {
        m_isMovingUp = move;
    }

    [PunRPC]
    public void RPCMoveDown(bool move)
    {
        m_isMovingDown = move;
    }

    public void MoveUp(bool move)
    { 
        if (GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID)
        {
            photonView.RPC("RPCMoveUp", PhotonTargets.All, move);
        }
        else
        {
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
            photonView.RPC("RPCMoveUp", PhotonTargets.All, move);
        }
    }

    public void MoveDown(bool move)
    {
        if (GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID)
        {
            photonView.RPC("RPCMoveDown", PhotonTargets.All, move);
        }
        else
        {
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
            photonView.RPC("RPCMoveDown", PhotonTargets.All, move);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(m_isMovingUp);
            stream.SendNext(m_isMovingDown);
        }
        else
        {
            m_isMovingUp = (bool)stream.ReceiveNext();
            m_isMovingDown = (bool)stream.ReceiveNext();
        }
    }
}
