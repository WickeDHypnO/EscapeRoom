using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsController : Photon.PunBehaviour
{

    private GameObject[] players = { null, null };

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            if (!players[0]) players[0] = other.gameObject;
            else players[1] = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < 2; i++)
            {
                if (players[i])
                {
                    if (other.gameObject == players[i])
                    {
                        players[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                        players[i] = null;
                        break;
                    }
                }
            }
            
        }
    }
    

    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            if (players[i])
            {
                if (players[i].GetComponent<RigidbodyFirstPersonController>().enabled)
                {
                    float rotation = players[i].transform.eulerAngles.y % 360f;
                    if (
                        (Input.GetKey(KeyCode.S) && rotation > 45 && rotation < 135) ||
                        (Input.GetKey(KeyCode.W) && rotation > 225 && rotation < 315) ||
                        (Input.GetKey(KeyCode.A) && (rotation <= 45 || rotation >= 315)) ||
                        (Input.GetKey(KeyCode.D) && rotation > 135 && rotation < 225)
                        )

                    {
                        players[i].transform.position += new Vector3(-Time.deltaTime * 2, Time.deltaTime * 2, 0);
                    }
                    else if (
                        (Input.GetKey(KeyCode.W) && rotation > 45 && rotation < 135) ||
                        (Input.GetKey(KeyCode.S) && rotation > 225 && rotation < 315) ||
                        (Input.GetKey(KeyCode.D) && (rotation <= 45 || rotation >= 315)) ||
                        (Input.GetKey(KeyCode.A) && rotation >= 135 && rotation <= 225)
                        )
                    {
                        players[i].transform.position += new Vector3(Time.deltaTime * 2, -Time.deltaTime * 2, 0);
                    }
                    break;
                }


            }
        }
    }
}
