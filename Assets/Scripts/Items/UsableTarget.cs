using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableTarget : Photon.PunBehaviour, IPunObservable
{
    public float UseDistance = UsableTargeter.DefaultItemUseDistance;

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

    public abstract void Use();
}
