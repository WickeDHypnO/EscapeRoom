using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobePickup : Photon.PunBehaviour, IPunObservable
{

    public float m_maxHeight;
    public float m_minHeight;

    [SerializeField] private int m_totalWheels;
    [SerializeField] private int m_wheels;

    private bool m_hasWheels;
    private Constraints m_constraints;

    private void Start()
    {
        m_hasWheels = m_totalWheels == m_wheels;
        m_constraints = GetComponent<Constraints>();
    }

    void Update ()
    {
        this.transform.position = new Vector3(this.transform.position.x, Mathf.Max(m_minHeight, Mathf.Min(m_maxHeight, this.transform.position.y)), this.transform.position.z);
	}

    public void SetMinHeight(float height)
    {
        m_minHeight = height;
    }

    public void SetMaxHeight(float height)
    {
        m_maxHeight = height;
    }

    public void AddWheel()
    {
        m_wheels++;
        m_hasWheels = m_wheels == m_totalWheels;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*if (stream.isWriting)
        {
            stream.SendNext(m_currentHeight);
        }
        else
        {
            m_currentHeight = (float)stream.ReceiveNext();
        }*/
    }

    IEnumerator UpdateConstraitsRoutine()
    {
        float timer = 0.0f;
        while (timer <= 0.5f)
        {
            yield return new WaitForSeconds(0.075f);
            m_maxHeight = Mathf.Min(m_maxHeight, this.transform.position.y);
            timer += 0.1f;
        }
    }

    public void UpdateConstraints()
    {
        if(m_hasWheels)
        {
            StartCoroutine(UpdateConstraitsRoutine());
            //m_constraints.m_rotY = false;
            //m_constraints.m_posX = false;
            m_constraints.m_posZ = false;
        }
    }
}
