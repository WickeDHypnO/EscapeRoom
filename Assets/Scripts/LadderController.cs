using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : Photon.PunBehaviour, IPunObservable
{

    public List<GameObject> LadderSteps;
    private int iterator = 0;
    private GameObject player;
    private bool movingOnlyDownOrForward = false;
    public bool LadderFinished { get; set; }
    private bool PlayerOnLadder = false;
    public bool DoorUpOpened { get; set; }
    public void DetachPlayer()
    {
        player.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        player = null;
        PlayerOnLadder = false;

    }

    public void Start()
    {
        LadderFinished = false;
        DoorUpOpened = true;
        foreach(GameObject step in LadderSteps)
        {
            if (step.GetActive())
                iterator++;
            if (iterator == LadderSteps.Count)
                LadderFinished = true;
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
        else if (other.tag == "Player" && LadderFinished && !PlayerOnLadder)
        {
            movingOnlyDownOrForward = false;
            player = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (player && player.transform.position.y < -2)
                player = null;
            else
                movingOnlyDownOrForward = true;
        }
    }
    private void Update()
    {
        if (player && Input.GetKeyDown(KeyCode.O))
        {
            PlayerOnLadder = !PlayerOnLadder;
            if(!PlayerOnLadder)
            {
                DetachPlayer();
            }
            else
            {
                player.GetComponent<Rigidbody>().useGravity = false;
                player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
        }
        if(player && PlayerOnLadder)
        {
            if (Input.GetKey(KeyCode.W) /*&& !movingOnlyDownOrForward*/)
            {
                if (DoorUpOpened && movingOnlyDownOrForward)
                {
                    player.transform.position += new Vector3(0, 0, Time.deltaTime * 2);
                }
                else if (!movingOnlyDownOrForward)
                    player.transform.position += new Vector3(0, Time.deltaTime * 2, 0);
            }
            else if (Input.GetKey(KeyCode.S))
                player.transform.position += new Vector3(0, -Time.deltaTime * 2, 0);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(LadderFinished);
            stream.SendNext(PlayerOnLadder);
            stream.SendNext(DoorUpOpened);
            stream.SendNext(movingOnlyDownOrForward);
        }
        else
        {
            LadderFinished = (bool)stream.ReceiveNext();
            PlayerOnLadder = (bool)stream.ReceiveNext();
            DoorUpOpened = (bool)stream.ReceiveNext();
            movingOnlyDownOrForward = (bool)stream.ReceiveNext();
        }
    }
}
