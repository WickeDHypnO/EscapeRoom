using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleElevatorController : Photon.PunBehaviour, IPunObservable
{

    [SerializeField] private float m_maxHeight;
    [SerializeField] private float m_minHeight;

    [SerializeField] private float m_speed;

    private float m_currentHeight;

    public bool m_isMovingUp;
    public bool m_isMovingDown;

    // Use this for initialization
    void Start ()
    {
        m_isMovingUp = false;
        m_isMovingDown = false;

        m_currentHeight = this.transform.position.y;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID)
        {
            float height = this.transform.position.y;
            if (m_isMovingUp && !m_isMovingDown && GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID)
            {
                height = Mathf.Min(m_maxHeight, this.transform.position.y + Time.fixedDeltaTime * m_speed);
                this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);
            }

            if (!m_isMovingUp && m_isMovingDown)
            {
                height = Mathf.Max(m_minHeight, this.transform.position.y - Time.fixedDeltaTime * m_speed);
                this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);
            }
            m_currentHeight = height;
        }
        else
        {
            float height = Mathf.MoveTowards(this.transform.position.y, m_currentHeight, Time.fixedDeltaTime * m_speed);
            this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);
        }
    }

    public void MoveUp(bool move)
    {
        if (GetComponent<PhotonView>().ownerId != PhotonNetwork.player.ID)
        {
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
        }

        m_isMovingUp = move;
    }

    public void MoveDown(bool move)
    {
        if (GetComponent<PhotonView>().ownerId != PhotonNetwork.player.ID)
        {
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
        }

        m_isMovingDown = move;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(m_currentHeight);
        }
        else
        {
            m_currentHeight = (float)stream.ReceiveNext();
        }
    }
}
