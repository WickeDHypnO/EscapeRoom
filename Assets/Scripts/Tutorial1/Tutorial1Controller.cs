using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1Controller : Photon.PunBehaviour, IPunObservable {

    public DoorController door;
    public GameObject blockingVolume;
    private bool leverDown = false;
    private bool[] platesPressed = { false, false };
    void Start () {
        if (!FindObjectOfType<LoadingScreenCanvas> ()) {
            SceneManager.LoadSceneAsync ("LoadingScreen", LoadSceneMode.Additive);
        } else {
            FindObjectOfType<LoadingScreenCanvas> ().FinishLoading ();
        }
    }

    [PunRPC]
    private void RPCCloseDoor () {
        if (!leverDown)
            door.Close ();
    }

    public void PlatePressed (int id) {
        if (PhotonNetwork.isMasterClient) {
            platesPressed[id] = true;
            if (platesPressed[0] && platesPressed[1]) {
                door.Open ();
            }
        }
    }

    [PunRPC]
    private void RPCLeverSetDown () {
        leverDown = true;
        blockingVolume.SetActive (false);
    }
    public void LeverSetDown () {
        if (PhotonNetwork.isMasterClient) {
            photonView.RPC ("RPCLeverSetDown", PhotonTargets.All);
        }
    }

    public void PlateReleased (int id) {
        if (PhotonNetwork.isMasterClient) {
            platesPressed[id] = false;
            photonView.RPC ("RPCCloseDoor", PhotonTargets.All);
        }
    }

    public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext (platesPressed[0]);
            stream.SendNext (platesPressed[1]);
            stream.SendNext (leverDown);
        } else {
            platesPressed[0] = (bool) stream.ReceiveNext ();
            platesPressed[1] = (bool) stream.ReceiveNext ();
            leverDown = (bool) stream.ReceiveNext ();
        }
    }
}