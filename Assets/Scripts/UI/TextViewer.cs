using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextViewer : UsableTarget {

    public Canvas canvas;
    public string text;
    private bool isReading = false;
    private RigidbodyFirstPersonController playerRigidbody;
    public void SetPlayerRigidbody(RigidbodyFirstPersonController _playerRigidbody)
    {
        playerRigidbody = _playerRigidbody;
    }
    public override void Use()
    {
        if (!isReading)
        {
            isReading = true;
            playerRigidbody.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            playerRigidbody.mouseEnabled = false;
            canvas.GetComponentInChildren<Text>().text = text;
            canvas.gameObject.SetActive(true);
        }
        else
        {
            isReading = false;
            playerRigidbody.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            playerRigidbody.mouseEnabled = true;
            canvas.gameObject.SetActive(false);
        }
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        return;
    }
}
