using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMovementTrigger : MonoBehaviour
{
    public LadderController ladderController;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ladderController.DetachPlayer();
            ladderController.LadderFinished = false;
        }
    }
}
