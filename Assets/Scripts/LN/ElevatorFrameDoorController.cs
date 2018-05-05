using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorFrameDoorController : ElevatorDoorController
{
    public GameObject ElevatorObject;
    public int FloorNumber;
    private ElevatorController elevatorController;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        if (ElevatorObject != null)
        {
            elevatorController = ElevatorObject.GetComponent<ElevatorController>();
        }
    }

    public void Use()
    {
        if ((elevatorController != null) && (elevatorController.CurrentFloor != FloorNumber)) return;
        if (Opened)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
