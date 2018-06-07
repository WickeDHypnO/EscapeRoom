using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour {

    public GameObject StartingRoom;

    void OnTriggerExit(Collider other)
    {
        LNStartingRoomController sr = StartingRoom.GetComponent<LNStartingRoomController>();
        sr.DeactivateTrap();
    }
}
