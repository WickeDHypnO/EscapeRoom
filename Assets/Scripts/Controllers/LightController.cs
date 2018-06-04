using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour/*Photon.PunBehaviour, IPunObservable*/
{
    public bool Enabled = false;
    public Color OffColour = Color.red;
    public Color OnColour = Color.green;
    private Material material;

    // Use this for initialization
    void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
        {
            material = mr.material;
            material.color = (Enabled ? OnColour : OffColour);
        }
    }

    public void TurnOn()
    {
        material.color = OnColour;
        Enabled = true;
    }

    public void TurnOff()
    {
        material.color = OffColour;
        Enabled = false;
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(Enabled);
        }
        else
        {
            Enabled = (bool)stream.ReceiveNext();
        }
    }*/
}
