using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour {

    public GameObject StartingRoom;
    private PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void OnTriggerExit(Collider other)
    {
        view.RPC("deactivateTrap", PhotonTargets.All);
    }

    [PunRPC]
    private void deactivateTrap()
    {
        LNStartingRoomController sr = StartingRoom.GetComponent<LNStartingRoomController>();
        sr.DeactivateTrap();
    }
}
