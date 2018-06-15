using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constraints : Photon.PunBehaviour
{
    public bool m_posX;
    public bool m_posY;
    public bool m_posZ;

    public bool m_rotX;
    public bool m_rotY;
    public bool m_rotZ;

    private Vector3 m_originalPosition;
    private Vector3 m_originalAngles;

	// Use this for initialization
	void Start ()
    {
        m_originalPosition = transform.position;
        m_originalAngles = transform.eulerAngles;
	}

    // Update is called once per frame
    //void FixedUpdate()
    void Update ()
    {
        float posX, posY, posZ, rotX, rotY, rotZ;

        if (m_posX)
            posX = m_originalPosition.x;
        else
            posX = transform.position.x;

        if (m_posY)
            posY = m_originalPosition.y;
        else
            posY = transform.position.y;

        if (m_posZ)
            posZ = m_originalPosition.z;
        else
            posZ = transform.position.z;

        if (m_rotX)
            rotX = m_originalAngles.x;
        else
            rotX = transform.eulerAngles.x;

        if (m_rotY)
            rotY = m_originalAngles.y;
        else
            rotY = transform.eulerAngles.y;

        if (m_rotZ)
            rotZ = m_originalAngles.z;
        else
            rotZ = transform.eulerAngles.z;

        transform.position = new Vector3(posX, posY, posZ);
        transform.eulerAngles = new Vector3(rotX, rotY, rotZ);
	}
}
