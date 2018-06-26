using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNStartingRoomController : RoomController {

    public GameObject[] LampLights;
    public GameObject[] LightsToDisable;
    public GameObject[] MovingFloors;
    public GameObject TrapTrigger;
    public GameObject[] WardrobeDoors;
    public GameObject[] HalfSpheres;
    private const int GEMS_COUNT = 3;
    private int activatedGems;
    private bool trapActivated;
    private Material halfSpheresShaderMaterial;

    // Use this for initialization
    void Start ()
    {
        activatedGems = 0;
        trapActivated = false;
        halfSpheresShaderMaterial = Material.Instantiate(HalfSpheres[0].GetComponent<Renderer>().sharedMaterial);
        foreach (GameObject obj in HalfSpheres)
        {
            obj.GetComponent<Renderer>().sharedMaterial = halfSpheresShaderMaterial;
        }
    }

    public void ActivateGem()
    {
        activateGem(true);
    }

    public void DeactivateGem()
    {
        activateGem(false);
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

    public void EnableOutsideLights(bool enable)
    {
        setObjectsActive(enable, LightsToDisable);
        modifyHalfSpheresMaterial(enable);
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

    private void activateGem(bool activate)
    {
        if (trapActivated) return;
        if (activate)
        {
            ++activatedGems;
            if (activatedGems >= GEMS_COUNT)
            {
                activateTrap();
                openWardrobeDoors();
            }
        }
        else
        {
            if (trapActivated) return;
            --activatedGems;
        }
    }

    private void modifyHalfSpheresMaterial(bool enable)
    {
        halfSpheresShaderMaterial.SetColor(
            "_Color",
            enable ? new Color(1.0f, 1.0f, 1.0f, 1.0f) : new Color(0.0f, 0.0f, 0.0f, 1.0f));
        halfSpheresShaderMaterial.SetColor(
            "_EmissionColor",
            enable ? new Color(1.0f, 1.0f, 1.0f, 1.0f) : new Color(0.0f, 0.0f, 0.0f, 0.0f));
    }
}
