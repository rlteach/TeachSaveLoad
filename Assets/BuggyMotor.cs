using UnityEngine;
using System.Collections;

public class BuggyMotor : MonoBehaviour {

    [Range(-100f,100f)]
    public float Speed;

    WheelJoint2D[] mWheelJoint;

    // Use this for initialization
    void Start () {
        mWheelJoint = GetComponents<WheelJoint2D>();
    }

    // Fixed Update is called once per physics frame, use this for physics updates
    void FixedUpdate () {
        foreach (WheelJoint2D tWJ in mWheelJoint) {
            JointMotor2D tJM = tWJ.motor;
            tJM.motorSpeed=Speed*10f;
            tWJ.motor = tJM;
        }
    }
}
