using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial2Controller : Photon.PunBehaviour, IPunObservable
{

    public DoorController DoorUp;
    public DoorController DoorDown;
    public LadderController ladderController;
    public GameObject floorPart;
    public float movingTime;
    public float targetOffset;
    private float floorInitalPos;
    private bool[] Cheat = { false, false, false, false };
    void Start()
    {
        if (!FindObjectOfType<LoadingScreenCanvas>())
        {
            SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        }
        else
        {
            FindObjectOfType<LoadingScreenCanvas>().FinishLoading();
        }
        floorInitalPos = floorPart.transform.position.z;
    }
    [PunRPC]
    public void RPCSwitchPushed()
    {
        DoorUp.Open();
    }
    [PunRPC]
    public void RPCSwitchReleased()
    {
        DoorUp.Close();
    }
    public void SwitchPushed()
    {
        ladderController.DoorUpOpened = true;
        photonView.RPC("RPCSwitchPushed", PhotonTargets.All);
    }

    public void SwitchReleased()
    {
        ladderController.DoorUpOpened = false;
        photonView.RPC("RPCSwitchReleased", PhotonTargets.All);
    }

    [PunRPC]
    public void RPCUpperPlatePressed()
    {
        
        DoorDown.Open();
        StopAllCoroutines();
        StartCoroutine(MoveFloor());
    }
    public void UpperPlatePressed()
    {
        photonView.RPC("RPCUpperPlatePressed", PhotonTargets.MasterClient);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        return;
    }
    IEnumerator MoveFloor()
    {
        float timer = 0f;
        while (!floorPart.transform.localPosition.z.AlmostEquals(floorInitalPos + targetOffset, 0.1f))
        {
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f / movingTime;
            floorPart.transform.position = Vector3.Lerp(floorPart.transform.position, new Vector3(floorPart.transform.position.x, floorPart.transform.position.y, floorInitalPos + targetOffset), timer);
        }
        //targetOffset = 0.0f;
        floorPart.transform.position = new Vector3(floorPart.transform.position.x,
            floorPart.transform.position.y,
            floorInitalPos + targetOffset);
    }
    void CheckCheat()
    {
        for(int i = 0; i < 4; i++)
        {
            if (!Cheat[i])
                break;
            if (i == 3)
            {
                Cheat[0] = Cheat[1] = Cheat[2] = Cheat[3] = false;
                ladderController.DoorUpOpened = true;
                photonView.RPC("RPCSwitchPushed", PhotonTargets.All);
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
            Cheat[0] = true;
        if (Input.GetKeyUp(KeyCode.Z))
            Cheat[1] = true;
        if (Input.GetKeyUp(KeyCode.I))
            Cheat[2] = true;
        if (Input.GetKeyUp(KeyCode.T))
            Cheat[3] = true;
        CheckCheat();
    }
}
