using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightMountController : MonoBehaviour {

    [SerializeField] private Transform m_followTarget;

	void FixedUpdate ()
    {
        if (m_followTarget)
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, m_followTarget.transform.eulerAngles.y, this.transform.eulerAngles.z);
	}
}
