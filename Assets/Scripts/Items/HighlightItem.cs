using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HighlightColours
{
    DEFAULT_COLOUR,
    INACTIVE_COLOUR,
    ITEM_USE_COLOUR,
    WRONG_ELEMENT_COLOUR
}

public class HighlightItem : MonoBehaviour
{
    public static readonly Vector4 DEFAULT_OUTLINE_COLOUR = new Vector4(1.0f, 0.641f, 0.0f, 0.184f);
    public static readonly Vector4 INTACTIVE_OUTLINE_COLOUR = new Vector4(0.4f, 0.4f, 0.4f, 0.5f);
    public static readonly Vector4 ITEM_USE_OUTLINE_COLOUR = new Vector4(0.0f, 0.8f, 0.0f, 0.2f);
    public static readonly Vector4 WRONG_ELEMENT_OUTLINE_COLOUR = new Vector4(0.8f, 0.0f, 0.0f, 0.2f);

    public GameObject outline;
    public bool outlineOn;

    public void OutlineOn()
    {
        outline.SetActive(true);
        outlineOn = true;
    }

    public void OutlineOff()
    {
        outline.SetActive(false);
        outlineOn = false;
    }

    public Vector4 GetCurrentColour()
    {
        return outline.GetComponent<MeshRenderer>().material.GetVector("_Color");
    }

    public void SetColour(Vector4 colour)
    {
        outline.GetComponent<MeshRenderer>().material.SetVector("_Color", colour);
    }
}
