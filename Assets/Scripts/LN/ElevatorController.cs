using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {

    public float FloorHeight = 2.0f;
    public float MoveTime = 3.5f;
    public int FloorsCount = 3;
    public int CurrentFloor = 2;
    public GameObject[] FloorsDoors;
    private bool moving;
    private float elapsedTime;
    private float moveDirection;

	// Use this for initialization
	void Start () {
        moving = false;
        elapsedTime = 0.0f;
        moveDirection = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
		if (moving)
        {
            float time = elapsedTime + Time.deltaTime;
            float distance = Time.deltaTime * FloorHeight / MoveTime;
            if (time >= MoveTime)
            {
                distance = (MoveTime - elapsedTime) * FloorHeight / MoveTime;
                moving = false;
                openDoor();
            }
            distance *= moveDirection;
            Vector3 currentPos = transform.position;
            float y = currentPos.y + distance;
            transform.position = new Vector3(currentPos.x, y, currentPos.z);
            elapsedTime = time;
        }
	}

    private void startMove(float direction)
    {
        moveDirection = direction;
        elapsedTime = 0.0f;
        moving = true;
    }

    public void MoveUp()
    {
        if (moving) return;
        if (CurrentFloor >= FloorsCount) return;
        closeDoor();
        ++CurrentFloor;
        startMove(1.0f);
    }

    public void MoveDown()
    {
        if (moving) return;
        if (CurrentFloor <= 1) return;
        closeDoor();
        --CurrentFloor;
        startMove(-1.0f);
    }

    private WallWithHiddenDoors getDoorForFloor(int floorNumber)
    {
        if (FloorsDoors.Length != FloorsCount) return null;
        GameObject fd = FloorsDoors[floorNumber - 1];
        if (fd == null) return null;
        WallWithHiddenDoors wd = fd.GetComponent<WallWithHiddenDoors>();
        return wd;
    }

    private void openDoor()
    {
        WallWithHiddenDoors wd = getDoorForFloor(CurrentFloor);
        if (wd == null) return;
        wd.Open();
    }

    private void closeDoor()
    {
        WallWithHiddenDoors wd = getDoorForFloor(CurrentFloor);
        if (wd == null) return;
        wd.Close();
    }
}
