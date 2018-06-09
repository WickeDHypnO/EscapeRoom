using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNStartingRoomController : RoomController {

    public GameObject[] LampLights;
    public GameObject[] SpotLights;
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

    public void EnableLampLights(bool enable)
    {
        setObjectsActive(enable, LampLights);
    }

    public void EnableSpotLights(bool enable)
    {
        setObjectsActive(enable, SpotLights);
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

    private void setObjectsActive(bool active, GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(active);
        }
    }
}
