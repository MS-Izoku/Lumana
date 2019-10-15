using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorMirror : RotatorNode
{
    public override void Setup(){
        target = transform.rotation * (Vector3.forward * -10f);   // GUI
        adjustedPos = transform.position + (Vector3.up * yAdjust);
        Quaternion minAngleRotationEuler = Quaternion.Euler(0 , minAngle , 0);
        Quaternion maxAngleRotationEuler = Quaternion.Euler(0 , maxAngle , 0);

        minAngleRotation = Quaternion.AngleAxis(minAngle , Vector3.up) * transform.rotation;
        maxAngleRotation = Quaternion.AngleAxis(maxAngle , Vector3.up) * transform.rotation;
        minTarget = minAngleRotation * target + adjustedPos;   // GUI
        maxTarget = maxAngleRotation * target + adjustedPos; // GUI
    }
}
