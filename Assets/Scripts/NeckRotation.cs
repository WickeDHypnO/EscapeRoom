using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckRotation : MonoBehaviour
{
    public Animator animator;
    public Transform lookPoint;
    public Transform camera;

    void OnAnimatorIK()
    {
        if (lookPoint != null)
        {
            animator.SetLookAtWeight(0.3f);
            animator.SetLookAtPosition(lookPoint.position);
        }
    }
    //void Update()
    //{
    //    if (transform.eulerAngles.x < 45 || transform.eulerAngles.x > 315)
    //    {
    //        neck.rotation = transform.rotation;
    //    }
    //}
}
