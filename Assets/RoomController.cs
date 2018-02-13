using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{

    public List<LeverController> levers;
    public List<int> leversCombination;
    public GameObject gem1, gem2;

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
        }
    }

    void Update()
    {

    }
}
