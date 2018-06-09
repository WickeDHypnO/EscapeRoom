using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour {

    public List<GameObject> LadderSteps;
    private int iterator = 0;
    private GameObject player;
    private bool moving = false;
    private bool LadderFinished = false;
    public void Start()
    {
        foreach(GameObject step in LadderSteps)
        {
            if (step.GetActive())
                iterator++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("LadderStep") && iterator < LadderSteps.Count)
        {
            Destroy(other.gameObject);
            LadderSteps[iterator].SetActive(true);
            iterator++;
            if (iterator == LadderSteps.Count)
                LadderFinished = true;
        }
        else if (other.gameObject.GetComponent<RigidbodyFirstPersonController>())
        {
            player = other.gameObject;
            player.GetComponent<Rigidbody>().useGravity = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<RigidbodyFirstPersonController>())
        {
            player.GetComponent<Rigidbody>().useGravity = true;
            player = null;
            moving = false;
        }
    }
    private void Update()
    {
        if(player)
        {
            moving = player.GetComponent<RigidbodyFirstPersonController>().movementSettings.Running;
            if (moving)
                player.transform.position += new Vector3(0, Time.deltaTime * 2, 0);
        }
    }
}
