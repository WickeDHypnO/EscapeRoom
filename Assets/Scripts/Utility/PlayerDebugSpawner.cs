using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugSpawner : MonoBehaviour {
#if UNITY_EDITOR
    public GameObject player;
    public Transform secondPlayerSpawn;
    public bool spawnSecondPlayer;
    public bool onePlayerMode;
    public GameObject firstCharacter, secondCharacter;

    void Start () {
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.CreateRoom ("Offline");
        var inst = PhotonNetwork.Instantiate (player.name, transform.position, Quaternion.identity, 0);
        if (spawnSecondPlayer) {
            var inst2 = PhotonNetwork.Instantiate (player.name, secondPlayerSpawn.position, Quaternion.identity, 0);
            secondCharacter = inst2;
            StartCoroutine (DisablePlayer2 ());
        }
        firstCharacter = inst;
    }

    IEnumerator DisablePlayer2 () {
        yield return new WaitForSeconds (0.2f);
        secondCharacter.GetComponentInChildren<Camera> ().enabled = false;
        secondCharacter.GetComponentInChildren<AudioListener> ().enabled = false;
        secondCharacter.GetComponentInChildren<UsableTargeter> ().enabled = false;
        secondCharacter.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
        secondCharacter.GetComponent<DisablePlayerModel> ().model.SetActive (true);
    }

    void Update () {
        if (onePlayerMode) {
            if (Input.GetKeyDown (KeyCode.Alpha1)) {
                if (firstCharacter) {
                    firstCharacter.GetComponentInChildren<Camera> ().enabled = true;
                    firstCharacter.GetComponentInChildren<AudioListener> ().enabled = true;
                    firstCharacter.GetComponentInChildren<UsableTargeter> ().enabled = true;
                    firstCharacter.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
                    firstCharacter.GetComponent<DisablePlayerModel> ().model.SetActive (false);
                    secondCharacter.GetComponentInChildren<Camera> ().enabled = false;
                    secondCharacter.GetComponentInChildren<AudioListener> ().enabled = false;
                    secondCharacter.GetComponentInChildren<UsableTargeter> ().enabled = false;
                    secondCharacter.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
                    secondCharacter.GetComponent<DisablePlayerModel> ().model.SetActive (true);
                }
            } else if (Input.GetKeyDown (KeyCode.Alpha2)) {
                if (secondCharacter) {
                    secondCharacter.GetComponentInChildren<Camera> ().enabled = true;
                    secondCharacter.GetComponentInChildren<AudioListener> ().enabled = true;
                    secondCharacter.GetComponentInChildren<UsableTargeter> ().enabled = true;
                    secondCharacter.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
                    secondCharacter.GetComponent<DisablePlayerModel> ().model.SetActive (false);
                    firstCharacter.GetComponentInChildren<Camera> ().enabled = false;
                    firstCharacter.GetComponentInChildren<AudioListener> ().enabled = false;
                    firstCharacter.GetComponentInChildren<UsableTargeter> ().enabled = false;
                    firstCharacter.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
                    firstCharacter.GetComponent<DisablePlayerModel> ().model.SetActive (true);
                }
            }
        }
    }
#endif
}