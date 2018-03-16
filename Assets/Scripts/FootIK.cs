using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour {

    float LFWeight = 1f, RFWeight = 1f;
    Transform leftFoot, rightFoot;
    Vector3 leftFootPos, rightFootPos;
    Quaternion leftFootRot, rightFootRot;
    Animator anim;

	void Start () {
        anim = GetComponent<Animator>();
        leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
    }
	
	void Update () {
        RaycastHit leftHit, rightHit;

        Vector3 lpos = leftFoot.TransformPoint(Vector3.zero);
        Vector3 rpos = rightFoot.TransformPoint(Vector3.zero);

        if (Physics.Raycast(lpos, -Vector3.up, out leftHit, 1))
        {
            leftFootPos = leftHit.point;
            leftFootRot = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
        }
        if (Physics.Raycast(rpos, -Vector3.up, out rightHit, 1))
        {
            rightFootPos = rightHit.point;
            rightFootRot = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
        }
    }

    void OnAnimatorIK()
    {
        if (anim.GetFloat("movementSpeed") > 0.1)
        {
            LFWeight = anim.GetFloat("LFWeight");
            RFWeight = anim.GetFloat("RFWeight");
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, LFWeight);
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, RFWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, LFWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, RFWeight);
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
            anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
        }
    }
}
