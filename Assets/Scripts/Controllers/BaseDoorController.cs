using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDoorController : Photon.PunBehaviour, IPunObservable
{
    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

    public abstract void Open();

    public abstract void Close();
}
