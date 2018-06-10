using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleDoorsController : BaseDoorController
{
    public GameObject[] Doors;
    private List<BaseDoorController> doorsControllers;

    public override void Close()
    {
        foreach (BaseDoorController doorController in doorsControllers)
        {
            doorController.Close();
        }
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}

    public override void Open()
    {
        foreach (BaseDoorController doorController in doorsControllers)
        {
            doorController.Open();
        }
    }

    // Use this for initialization
    void Start ()
    {
        doorsControllers = new List<BaseDoorController>();
        foreach (GameObject door in Doors)
        {
            if (door == null) continue;
            BaseDoorController doorController = door.GetComponent<BaseDoorController>();
            if (doorController == null) continue;
            doorsControllers.Add(doorController);
        }
	}
}
