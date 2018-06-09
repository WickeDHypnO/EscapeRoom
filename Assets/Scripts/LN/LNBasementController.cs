using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNBasementController : RoomController
{
    public GameObject ElevatorOpenLever;
    public GameObject ElevatorOpenLeverHandle;
    public GameObject PowerSwitcherLever;
    public GameObject PowerSwitcherLeverHandle;
    private bool puzzleFinished;

    public void EnableLever(bool enable)
    {
        if (!puzzleFinished) return;
        ElevatorOpenLever.GetComponent<LeverController>().canUse = enable;
    }

    public void FinishPuzzle()
    {
        puzzleFinished = true;
        ElevatorOpenLever.GetComponent<LeverController>().canUse = true;
    }

    public void DropLeverHandle()
    {
        GameObject.Destroy(PowerSwitcherLever.GetComponent<LeverController>());
        GameObject.Destroy(PowerSwitcherLever.GetComponent<HighlightItem>().outline);
        GameObject.Destroy(PowerSwitcherLever.GetComponent<HighlightItem>());
        GameObject.Destroy(PowerSwitcherLeverHandle);
        ElevatorOpenLeverHandle.SetActive(true);
    }

	// Use this for initialization
	void Start ()
    {
        puzzleFinished = false;
    }
}
