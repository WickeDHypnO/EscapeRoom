using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1Controller : MonoBehaviour {

    public int platesPressed = 0;
    public DoorController door;

	public void platePressed()
    {
        platesPressed++;
        if (platesPressed == 2)
        {
            door.Open();
        }
    }
	
    public void plateReleased()
    {
        platesPressed--;
        door.Close();
    }
}
