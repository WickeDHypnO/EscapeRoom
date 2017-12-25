using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : Photon.PunBehaviour
{
    public bool flashlightEnabled = false;
    bool flashlightOn = false;
    public Light flashlight;
    public Animator playerAnimator;
    public GameObject flashlightInHand;

    void Update()
    {
        if (flashlightEnabled)
        {
            if (Input.GetButtonDown("Flashlight"))
            {
                photonView.RPC("RpcSwitchFlashlight", PhotonTargets.All, flashlightOn = !flashlightOn);
            }
        }
    }

    [PunRPC]
    void RpcSwitchFlashlight(bool switchFlashlight)
    {
        if (switchFlashlight)
        {
            playerAnimator.SetBool("hasFlashlight", true);
            flashlight.enabled = true;
            flashlightInHand.SetActive(true);
        }
        else
        {
            playerAnimator.SetBool("hasFlashlight", false);
            flashlight.enabled = false;
            flashlightInHand.SetActive(false);
        }
    }
}
