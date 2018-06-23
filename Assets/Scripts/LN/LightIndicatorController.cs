using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIndicatorController : MonoBehaviour
{
    public GameObject LightBulb;
    public Material OffMaterial;
    public Material OnMaterial;
    public bool On;

    public void SetLight(bool on)
    {
        On = on;
        LightBulb.GetComponent<Renderer>().material = (On ? OnMaterial : OffMaterial);
    }

	// Use this for initialization
	void Start ()
    {
        LightBulb.GetComponent<Renderer>().material = (On ? OnMaterial : OffMaterial);
	}
}
