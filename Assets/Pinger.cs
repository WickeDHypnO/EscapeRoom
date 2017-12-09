using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinger : MonoBehaviour
{
    private RaycastHit hit;
    public GameObject debugCube;

    void Update()
    {
        if (Input.GetButtonDown("Ping"))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
            {
                debugCube.transform.parent = null;
                debugCube.transform.position = hit.point;
                debugCube.transform.up = hit.normal;
                debugCube.GetComponent<PingDissapear>().enabled = true;
                debugCube.GetComponent<PingDissapear>().Restart();
                GetComponent<LineRenderer>().SetPosition(0, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z));
                GetComponent<LineRenderer>().SetPosition(1, debugCube.transform.position);
                GetComponent<LineRenderer>().enabled = true;
            }
        }
    }
}
