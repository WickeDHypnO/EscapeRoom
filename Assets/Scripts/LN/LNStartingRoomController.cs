using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNStartingRoomController : RoomController {

    public GameObject[] Lights;
    public GameObject[] MovingFloors;
    public GameObject TrapTrigger;
    public GameObject[] WardrobeDoors;
    private const int GEMS_COUNT = 3;
    private int activatedGems;
    private bool trapActivated;

	// Use this for initialization
	void Start ()
    {
        activatedGems = 0;
        trapActivated = false;
    }
	
	// Update is called once per frame
	void Update () {}

    public void ActivateGem()
    {
        if (trapActivated) return;
        ++activatedGems;
        if (activatedGems >= GEMS_COUNT)
        {
            activateTrap();
            openWardrobeDoors();
        }
    }

    public void DeactivateGem()
    {
        if (trapActivated) return;
        --activatedGems;
    }

    public void DeactivateTrap()
    {
        if (!trapActivated) return;
        foreach (GameObject floor in MovingFloors)
        {
            MovingFloor mf = floor.GetComponent<MovingFloor>();
            mf.MoveDown();
        }
        TrapTrigger.SetActive(false);
    }

    public void EnableLights()
    {
        foreach (GameObject light in Lights)
        {
            light.SetActive(true);
        }
    }

    public void DisableLights()
    {
        foreach (GameObject light in Lights)
        {
            light.SetActive(false);
        }
    }

    private void activateTrap()
    {
        foreach (GameObject floor in MovingFloors)
        {
            MovingFloor mf = floor.GetComponent<MovingFloor>();
            mf.MoveUp();
        }
        TrapTrigger.SetActive(true);
        trapActivated = true;
    }

    private void openWardrobeDoors()
    {
        foreach (GameObject door in WardrobeDoors)
        {
            UsableDoorController udc = door.GetComponent<UsableDoorController>();
            udc.Unlock();
            udc.Use();
        }
    }
}
