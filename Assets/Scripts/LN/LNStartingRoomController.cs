using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNStartingRoomController : RoomController {

    public GameObject[] Lights;
    private int activatedGems;
    private bool lightsEnabled;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateGem()
    {
        ++activatedGems;
        if (activatedGems >= 3)
        {
            // Uruchomienie pułapki
        }
    }

    public void DeactivateGem()
    {
        --activatedGems;
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
}
