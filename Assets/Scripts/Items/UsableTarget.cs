using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableTarget : Photon.PunBehaviour, IPunObservable
{
    public float UseDistance = UsableTargeter.DefaultItemUseDistance;

    public Vector4 InactiveOutlineColour = new Vector4(0.4f, 0.4f, 0.4f, 0.5f);

    public Vector4 ItemUseOutlineColour = new Vector4(0.0f, 0.8f, 0.0f, 0.2f);

    public bool UsesItems = false;

    protected Vector4 defaultOutlineColour;

    private bool inactiveOutline = false;

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

    public abstract void Use();

    /*
     * Zwraca true jeśli można użyć dany przedmiot na tym obiekcie.
     */
    public virtual bool CheckItemOnTrace(string itemId)
    {
        return false;
    }

    /*
     * Zwraca true jeśli przedmiot został pomyślnie użyty na tym obiekcie.
     */
    public virtual bool UseItem(string itemId)
    {
        return false;
    }

    public void SetItemUseOutline(bool enable)
    {
        HighlightItem highlight = GetComponent<HighlightItem>();
        if (highlight == null) return;
        if (enable)
        {
            highlight.outline.GetComponent<MeshRenderer>().material.SetVector("_Color", ItemUseOutlineColour);
        }
        else
        {
            highlight.outline.GetComponent<MeshRenderer>().material.SetVector("_Color",
                (inactiveOutline ? InactiveOutlineColour : defaultOutlineColour));
        }
    }

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
            inactiveOutline = false;
        }
        else
        {
            highlight.outline.GetComponent<MeshRenderer>().material.SetVector("_Color", InactiveOutlineColour);
            inactiveOutline = true;
        }
    }

    protected virtual void initialize() {}
}
