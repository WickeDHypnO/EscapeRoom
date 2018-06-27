using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : Photon.PunBehaviour//, IPunObservable
{
    public float FloorHeight = 2.0f;
    public float MoveTime = 3.5f;
    public int FloorsCount = 3;
    public int CurrentFloor = 1;
    public GameObject[] FloorsDoors;
    public GameObject ElevatorDoor;
    private bool moving;
    private float elapsedTime;
    private float moveDirection;
    private float height;
    private float totalMoveTime;
    private ElevatorDoorController elevatorDoorComp;
    //private HashSet<GameObject> objectsInside;
    private PhotonView view;
    private HashSet<int> objectsInsideViewIDs;
    private Transform viewTransform;

    // Use this for initialization
    void Start()
    {
        elevatorDoorComp = ElevatorDoor.GetComponent<ElevatorDoorController>();
        //objectsInside = new HashSet<GameObject>();
        view = GetComponent<PhotonView>();
        view.TransferOwnership(PhotonNetwork.masterClient);
        objectsInsideViewIDs = new HashSet<int>();
        viewTransform = view.transform;
    }

    // Update is called once per frame
    void Update() {
        if (view.ownerId != PhotonNetwork.player.ID) return;
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
            updateObjectsInside(distance);
            elapsedTime = time;
        }
    }

    public void ChangeFloor(int floorNumber)
    {
        view.RPC("rpcChangeFloor", PhotonTargets.MasterClient, floorNumber);
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

    private BaseDoorController getDoorsForFloorComponent(int floorNumber)
    {
        if (FloorsDoors.Length != FloorsCount) return null;
        GameObject fd = FloorsDoors[floorNumber - 1];
        if (fd == null) return null;
        return fd.GetComponent<BaseDoorController>();
    }

    private void openDoor()
    {
        elevatorDoorComp.Open();
        BaseDoorController wd = getDoorsForFloorComponent(CurrentFloor);
        if (wd == null) return;
        wd.Open();
    }

    private void closeDoor()
    {
        elevatorDoorComp.Close();
        BaseDoorController wd = getDoorsForFloorComponent(CurrentFloor);
        if (wd == null) return;
        wd.Close();
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
    }*/

    void OnTriggerEnter(Collider other)
    {
        /*GameObject obj = other.gameObject;
        obj.transform.parent = transform;
        objectsInside.Add(obj);*/
        PhotonView objView = other.gameObject.GetPhotonView();
        int viewId = objView.viewID;
        Transform objTransform = objView.transform;
        if (!objectsInsideViewIDs.Contains(viewId) && (objTransform.parent == null))
        {
            objTransform.SetParent(viewTransform);
            objectsInsideViewIDs.Add(viewId);
        }
    }

    void OnTriggerExit(Collider other)
    {
        /*GameObject obj = other.gameObject;
        if (obj.transform.parent == transform)
        {
            obj.transform.parent = null;
        }
        objectsInside.Remove(obj);*/
        PhotonView objView = other.gameObject.GetPhotonView();
        int viewId = objView.viewID;
        Transform objTransform = objView.transform;
        if (objTransform.parent == viewTransform)
        {
            objTransform.SetParent(null);
        }
        objectsInsideViewIDs.Remove(viewId);
    }

    private void updateObjectsInside(float yDist)
    {
        /*foreach (GameObject obj in objectsInside)
        {
            Vector3 objPos = obj.transform.position;
            float y = objPos.y + yDist;
            obj.transform.position = new Vector3(objPos.x, y, objPos.z);
        }*/
    }

    [PunRPC]
    private void rpcChangeFloor(int floorNumber)
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
}
