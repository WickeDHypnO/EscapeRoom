using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNStartingRoomController : RoomController {

    public bool FloorDestroyed { get; private set; }

	// Use this for initialization
	void Start () {
        FloorDestroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyFloor()
    {
        if (FloorDestroyed) return;
        // TODO
        FloorDestroyed = true;
    }
}
