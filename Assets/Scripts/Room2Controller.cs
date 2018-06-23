using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2Controller : Photon.PunBehaviour, IPunObservable {
    public List<LeverController> levers;
    public List<int> leversCombination;
    public GameObject gem1, gem2;
    public List<DoorController> doors;
    public bool gem1active, gem2active;
    public bool debugCheck;

    void Update () {
        if (debugCheck) {
            CheckIfRoomComplete ();
            debugCheck = false;
        }
    }

    [PunRPC]
    public void RpcCheckLevers (int leverId) {
        var lever = PhotonView.Find (leverId).GetComponent<LeverController> ();
        bool open = true;
        int index = levers.IndexOf (lever);
        foreach (int i in leversCombination) {
            if (i == index)
                break;
            if (levers[i].position) {
                continue;
            } else {
                foreach (LeverController lev in levers) {
                    //lev.StartCoroutine (lev.MoveUp ());
                    lev.Reset();
                }
                break;
            }
        }
        foreach (LeverController lev in levers) {
            if (!lev.position) {
                open = false;
            }
        }
        if (open) {
            gem1.GetComponent<MeshRenderer> ().material.color = Color.green;
            gem1active = true;
        }
        CheckIfRoomComplete ();
    }
    public void CheckLevers (LeverController lever) {
        photonView.RPC ("RpcCheckLevers", PhotonTargets.All, lever.photonView.viewID);
    }

    public void PlatePressed () {
        gem2.GetComponent<MeshRenderer> ().material.color = Color.green;
        gem2active = true;
        CheckIfRoomComplete ();
    }

    public void PlateReleased () {
        gem2.GetComponent<MeshRenderer> ().material.color = Color.white;
        gem2active = false;
        CloseDoors ();
    }
    void CheckIfRoomComplete () {
        if (gem1active && gem2active) {
            OpenDoors ();
        }
    }

    void OpenDoors () {
        foreach (DoorController door in doors) {
            door.Open ();
        }
    }

    void CloseDoors () {
        foreach (DoorController door in doors) {
            door.Close ();
        }
    }

    public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext (gem1active);
            stream.SendNext (gem2active);
        } else {
            gem1active = (bool) stream.ReceiveNext ();
            if (gem1active) {
                gem1.GetComponent<MeshRenderer> ().material.color = Color.green;
            } else {
                gem1.GetComponent<MeshRenderer> ().material.color = Color.white;
            }
            gem2active = (bool) stream.ReceiveNext ();
            if (gem2active) {
                gem2.GetComponent<MeshRenderer> ().material.color = Color.green;
            } else {
                gem2.GetComponent<MeshRenderer> ().material.color = Color.white;
            }
        }
    }
}