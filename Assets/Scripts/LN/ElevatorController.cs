using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : Photon.PunBehaviour, IPunObservable
{
    public float FloorHeight = 2.0f;
    public float MoveTime = 3.5f;
    public int FloorsCount = 3;
    public int CurrentFloor = 1;
    public GameObject[] FloorsDoors;
    private bool moving;
    private float elapsedTime;
    private float moveDirection;
    private float height;
    private float totalMoveTime;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if (moving)
        {
            float time = elapsedTime + Time.deltaTime;
            float distance = Time.deltaTime * height / totalMoveTime;
            if (time >= totalMoveTime)
            {
                distance = (totalMoveTime - elapsedTime) * height / totalMoveTime;
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

    public void ChangeFloor(int floorNumber)
    {
        if (moving) return;
        if ((floorNumber < 1) || (floorNumber > FloorsCount)) return;
        if (floorNumber == CurrentFloor) return;
        closeDoor();
        int floorsDifference = Math.Abs(floorNumber - CurrentFloor);
        height = floorsDifference * FloorHeight;
        totalMoveTime = floorsDifference * MoveTime;
        startMove(floorNumber > CurrentFloor ? 1.0f : -1.0f);
        CurrentFloor = floorNumber;
    }

    private void startMove(float direction)
    {
        moveDirection = direction;
        elapsedTime = 0.0f;
        moving = true;
    }

    /*public void MoveUp()
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
    }*/

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(CurrentFloor);
            stream.SendNext(moving);
            stream.SendNext(elapsedTime);
            stream.SendNext(moveDirection);
            stream.SendNext(height);
            stream.SendNext(totalMoveTime);
        }
        else
        {
            CurrentFloor = (int)stream.ReceiveNext();
            moving = (bool)stream.ReceiveNext();
            elapsedTime = (float)stream.ReceiveNext();
            moveDirection = (float)stream.ReceiveNext();
            height = (float)stream.ReceiveNext();
            totalMoveTime = (float)stream.ReceiveNext();
        }
    }
}
