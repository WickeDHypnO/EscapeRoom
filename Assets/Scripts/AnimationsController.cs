using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour {

    public float movementSpeed;
    public RigidbodyFirstPersonController playerRigidbody;
    public Animator playerAnimator;
	
	void FixedUpdate () {
        movementSpeed = playerRigidbody.Velocity.magnitude;
        playerAnimator.SetFloat("movementSpeed", movementSpeed);
	}
}
