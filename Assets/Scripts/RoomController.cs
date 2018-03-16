using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<LeverController> levers;
    public List<int> leversCombination;
    public GameObject gem1, gem2;
    public List<DoorController> doors;
    public bool gem1active, gem2active;
    public bool debugCheck;

    void Update()
    {
        if(debugCheck)
        {
            CheckIfRoomComplete();
            debugCheck = false;
        }
    }

    public void CheckLevers(LeverController lever)
    {
        bool open = true;
        int index = levers.IndexOf(lever);
        foreach (int i in leversCombination)
        {
            if (i == index)
                break;
            if (levers[i].position)
            {
                continue;
            }
            else
            {
                foreach (LeverController lev in levers)
                {
                    lev.StartCoroutine(lev.MoveUp());
                }
                break;
            }
        }
        foreach (LeverController lev in levers)
        {
            if (!lev.position)
            {
                open = false;
            }
        }
        if(open)
        {
            gem1.GetComponent<MeshRenderer>().material.color = Color.green;
            gem1active = true;
        }
        CheckIfRoomComplete();
    }

    public void CheckPressurePlate(PressurePlateController plate)
    {
        if(plate.pressed)
        {
            gem2.GetComponent<MeshRenderer>().material.color = Color.green;
            gem2active = true;
        }
        else
        {
            gem2.GetComponent<MeshRenderer>().material.color = Color.white;
            gem2active = false;
        }
        CheckIfRoomComplete();
    }

    void CheckIfRoomComplete()
    {
        if(gem1active && gem2active)
        {
            OpenDoors();
        }
    }

    void OpenDoors()
    {
        foreach(DoorController door in doors)
        {
            door.Open();
        }
    }
}
