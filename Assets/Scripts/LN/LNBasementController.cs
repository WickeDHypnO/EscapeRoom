using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNBasementController : RoomController {
    public GameObject ElevatorOpenLever;
    public GameObject ElevatorOpenLeverHandle;
    public GameObject PowerSwitcherLever;
    public GameObject PowerSwitcherLeverHandle;
    private bool puzzleFinished;
    private PhotonView view;

    public void EnableLever (bool enable) {
        if (!puzzleFinished) return;
        ElevatorOpenLever.GetComponent<LeverController> ().canUse = enable;
    }

    public void FinishPuzzle () {
        puzzleFinished = true;
        ElevatorOpenLever.GetComponent<LeverController> ().SetCanUse (true);
    }

    public void DropLeverHandle () {
        view.RPC ("RpcDropLeverHandle", PhotonTargets.All);
    }

    [PunRPC]
    void RpcDropLeverHandle () {
        GameObject.Destroy (PowerSwitcherLever.GetComponent<LeverController> ());
        if(PowerSwitcherLever.GetComponent<HighlightItem> () && PowerSwitcherLever.GetComponent<HighlightItem> ().outline)
        GameObject.Destroy (PowerSwitcherLever.GetComponent<HighlightItem> ().outline);
        if(PowerSwitcherLever.GetComponent<HighlightItem> ())
        GameObject.Destroy (PowerSwitcherLever.GetComponent<HighlightItem> ());
        GameObject.Destroy (PowerSwitcherLeverHandle);
        ElevatorOpenLeverHandle.SetActive (true);
    }

    // Use this for initialization
    void Start () {
        view = GetComponent<PhotonView> ();
        puzzleFinished = false;
    }
}