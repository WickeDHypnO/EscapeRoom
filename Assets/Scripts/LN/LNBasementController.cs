using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNBasementController : RoomController
{
    public GameObject Lever;
    public GameObject ElevatorDoors;

    public void EnableLever()
    {
        Lever.GetComponent<LeverController>().canUse = true;
    }

    public void UnlockElevator()
    {
        ElevatorDoors.GetComponent<ElevatorDoorController>().Open();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
