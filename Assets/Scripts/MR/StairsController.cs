using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsController : Photon.PunBehaviour {

    private GameObject player = null;

    private void OnTriggerEnter (Collider other) {
        if (other.tag == "Player" && other.GetComponent<PhotonView> ().owner == PhotonNetwork.player) {
            other.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            player = other.gameObject;
        }
    }
    private void OnTriggerExit (Collider other) {
        if (other.tag == "Player" && other.GetComponent<PhotonView> ().owner == PhotonNetwork.player) {
            other.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
            player = null;
        }
    }
    private void Update () {
        if (player) {
            float rotation = player.transform.eulerAngles.y % 360f;
            if (
                (Input.GetKey (KeyCode.S) && rotation > 45 && rotation < 135) ||
                (Input.GetKey (KeyCode.W) && rotation > 225 && rotation < 315) ||
                (Input.GetKey (KeyCode.A) && (rotation <= 45 || rotation >= 315)) ||
                (Input.GetKey (KeyCode.D) && rotation > 135 && rotation < 225)
            ) {
                player.transform.position += new Vector3 (-Time.deltaTime * 2, Time.deltaTime * 2, 0);
            } else if (
                (Input.GetKey (KeyCode.W) && rotation > 45 && rotation < 135) ||
                (Input.GetKey (KeyCode.S) && rotation > 225 && rotation < 315) ||
                (Input.GetKey (KeyCode.D) && (rotation <= 45 || rotation >= 315)) ||
                (Input.GetKey (KeyCode.A) && rotation >= 135 && rotation <= 225)
            ) {
                player.transform.position += new Vector3 (Time.deltaTime * 2, -Time.deltaTime * 2, 0);
            }
        }
    }
}