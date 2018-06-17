using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleElementPlaceholder : Photon.PunBehaviour {
    public string PuzzleElementId;
    // Obiekt jaki ma się odsłonić po ułożeniu elementu na właściwym miejscu
    public GameObject ActualObject;
    public UnityEvent OnElementPlaced;
    private HighlightItem highlight;
    private HighlightColours currentOutlineState;
    private HighlightColours previousOutlineState;

    /*
     * Zwraca true jeśli to pasujący element.
     */
    public virtual bool CheckElementOnTrace (string itemId) {
        return (!string.IsNullOrEmpty (itemId) && !string.IsNullOrEmpty (PuzzleElementId) && (itemId == PuzzleElementId));
    }

    [PunRPC]
    public void RPCPlaceElement (int viewID) {
        ActualObject.SetActive (true);
        OnElementPlaced.Invoke ();
        if (PhotonNetwork.isMasterClient) {
            var go = PhotonView.Find (viewID);
            PhotonNetwork.Destroy (go);
            PhotonNetwork.Destroy (photonView);
        }
    }

    public void PlaceElement (GameObject elementObject) {
        photonView.RPC ("RPCPlaceElement", PhotonTargets.All, elementObject.GetPhotonView ().viewID);
    }

    public void SetOutlineColour (HighlightColours colourType) {
        Vector4 colour = getHighlightColour (colourType);
        highlight.SetColour (colour);
        previousOutlineState = currentOutlineState;
        currentOutlineState = colourType;
    }

    // Use this for initialization
    void Start () {
        highlight = GetComponent<HighlightItem> ();
        highlight.SetColour (HighlightItem.INTACTIVE_OUTLINE_COLOUR);
        highlight.OutlineOn ();
    }

    private Vector4 getHighlightColour (HighlightColours colourType) {
        switch (colourType) {
            case HighlightColours.INACTIVE_COLOUR:
                return HighlightItem.INTACTIVE_OUTLINE_COLOUR;
            case HighlightColours.ITEM_USE_COLOUR:
                return HighlightItem.ITEM_USE_OUTLINE_COLOUR;
            case HighlightColours.WRONG_ELEMENT_COLOUR:
                return HighlightItem.WRONG_ELEMENT_OUTLINE_COLOUR;
            default:
                throw new NotImplementedException ();
        }
    }
}