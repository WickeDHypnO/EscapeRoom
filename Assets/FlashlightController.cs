using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : Photon.PunBehaviour
{

    public bool flashlightEnabled = false;
    bool flashlightOn = false;
    public Light flashlight;

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
            flashlight.enabled = true;
        }
        else
        {
            flashlight.enabled = false;
        }
    }
}
