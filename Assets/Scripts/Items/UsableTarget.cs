using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableTarget : Photon.PunBehaviour, IPunObservable
{
    public float UseDistance = UsableTargeter.DefaultItemUseDistance;

    public Vector4 InactiveOutlineColour = new Vector4(0.4f, 0.4f, 0.4f, 0.5f);

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

    public abstract void Use();

    protected Vector4 defaultOutlineColour;

    void Start()
    {
        HighlightItem highlight = GetComponent<HighlightItem>();
        if (highlight != null)
        {
            defaultOutlineColour = highlight.outline.GetComponent<MeshRenderer>().material.GetVector("_Color");
        }
        initialize();
    }

    protected void setOutlineActive(bool active)
    {
        HighlightItem highlight = GetComponent<HighlightItem>();
        if (highlight == null) return;
        if (active)
        {
            highlight.outline.GetComponent<MeshRenderer>().material.SetVector("_Color", defaultOutlineColour);
        }
        else
        {
            highlight.outline.GetComponent<MeshRenderer>().material.SetVector("_Color", InactiveOutlineColour);
        }
    }

    virtual protected void initialize() {}
}
