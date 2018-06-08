using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableTarget : Photon.PunBehaviour, IPunObservable
{
    public float UseDistance = UsableTargeter.DefaultItemUseDistance;
    public Vector4 InactiveOutlineColour = HighlightItem.INTACTIVE_OUTLINE_COLOUR;
    public Vector4 ItemUseOutlineColour = HighlightItem.ITEM_USE_OUTLINE_COLOUR;
    public bool UsesItems = false;
    protected Vector4 defaultOutlineColour;
    private HighlightColours currentOutlineState;
    private HighlightColours previousOutlineState;

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

    public void SetOutlineColour(HighlightColours colourType)
    {
        HighlightItem highlight = GetComponent<HighlightItem>();
        if (highlight == null) return;
        Vector4 colour = getHighlightColour(colourType);
        highlight.SetColour(colour);
        previousOutlineState = currentOutlineState;
        currentOutlineState = colourType;
    }

    public void RevertOutlineColour()
    {
        if (previousOutlineState != HighlightColours.ITEM_USE_COLOUR)
        {
            SetOutlineColour(previousOutlineState);
        }
    }

    protected virtual void initialize() {}

    void Start()
    {
        HighlightItem highlight = GetComponent<HighlightItem>();
        if (highlight != null)
        {
            currentOutlineState = HighlightColours.DEFAULT_COLOUR;
            previousOutlineState = HighlightColours.DEFAULT_COLOUR;
            defaultOutlineColour = highlight.GetCurrentColour();
        }
        initialize();
    }

    private Vector4 getHighlightColour(HighlightColours colourType)
    {
        switch (colourType)
        {
            case HighlightColours.DEFAULT_COLOUR:
                return defaultOutlineColour;
            case HighlightColours.INACTIVE_COLOUR:
                return InactiveOutlineColour;
            case HighlightColours.ITEM_USE_COLOUR:
                return ItemUseOutlineColour;
            default:
                throw new NotImplementedException();
        }
    }
}
