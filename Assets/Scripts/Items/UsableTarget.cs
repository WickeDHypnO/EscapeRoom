using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableTarget : Photon.PunBehaviour, IPunObservable
{
    public static readonly Color DisabledOutlineColour = Color.gray;

    public float UseDistance = UsableTargeter.DefaultItemUseDistance;

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

    public abstract void Use();

    protected virtual void enableOutline()
    {
        HighlightItem highlight = GetComponent<HighlightItem>();
        if (highlight == null) return;
        highlight.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.643f, 0.0f, 0.184f);
    }

    protected virtual void disableOutline()
    {
        HighlightItem highlight = GetComponent<HighlightItem>();
        if (highlight == null) return;
        highlight.GetComponent<MeshRenderer>().material.color = DisabledOutlineColour;
    }
}
