using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleElevatorController : Photon.PunBehaviour, IPunObservable
{

    [SerializeField] private float m_maxHeight;
    [SerializeField] private float m_minHeight;

    [SerializeField] private float m_speed;

    private bool m_isMovingUp;
    private bool m_isMovingDown;

    // Use this for initialization
    void Start ()
    {
        m_isMovingUp = false;
        m_isMovingDown = false;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if(m_isMovingUp && !m_isMovingDown)
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

    public void MoveUp(bool move)
    {
        m_isMovingUp = move;
    }

    public void MoveDown(bool move)
    {
        m_isMovingDown = move;
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
