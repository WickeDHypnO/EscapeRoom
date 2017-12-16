using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinger : Photon.PunBehaviour
{
    private RaycastHit hit;
    public GameObject ping;
    public float delay = 3.5f;
    public PingIndicator indicator;
    bool canPing;
    float timer = 3.6f;

    void Update()
    {
        if (!photonView.isMine)
        {
            return;
        }
        if (!canPing)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                canPing = true;
            }
        }
        if (Input.GetButtonDown("Ping") && canPing)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
            {
                timer = 0;
                PhotonNetwork.Instantiate(ping.name, hit.point, Quaternion.LookRotation(hit.normal), 0);
                canPing = false;
                GetComponent<LineRenderer>().SetPosition(0, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z));
                GetComponent<LineRenderer>().SetPosition(1, hit.point);
                GetComponent<LineRenderer>().enabled = true;
            }
        }
    }
}

