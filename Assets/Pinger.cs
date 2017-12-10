using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinger : MonoBehaviour
{
    private RaycastHit hit;
    public GameObject debugCube;
    public float delay = 3.5f;
    bool canPing;
    float timer = 3.6f;

    void Update()
    {
        if (!canPing)
        {
            timer += Time.deltaTime;
            if(timer >= delay)
            {
                canPing = true;
            }
        }
        if (Input.GetButtonDown("Ping") && canPing)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
            {
                timer = 0;
                canPing = false;
                debugCube.transform.parent = null;
                debugCube.transform.position = hit.point;
                debugCube.transform.up = hit.normal;
                debugCube.SetActive(true);
                debugCube.GetComponent<PingDissapear>().Restart();
                GetComponent<LineRenderer>().SetPosition(0, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z));
                GetComponent<LineRenderer>().SetPosition(1, debugCube.transform.position);
                GetComponent<LineRenderer>().enabled = true;
            }
        }
    }
}
