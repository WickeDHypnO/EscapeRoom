using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstancer : MonoBehaviour {

	public Color materialColor;
	public Material defaultMaterial;
	
	[ContextMenu("Set new material")]
	void MakeMaterialInstance()
	{
		var newMaterial = new Material(defaultMaterial);
		newMaterial.color = materialColor;
		DestroyImmediate(GetComponent<MeshRenderer>().material);
		GetComponent<MeshRenderer>().material = newMaterial;
	}
}
