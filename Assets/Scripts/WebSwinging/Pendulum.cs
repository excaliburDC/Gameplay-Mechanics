using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pendulum
{
    public Transform bob_tr;
    public Tether tether;
    public Arm arm;
    public Bob bob;

    private Vector3 previousPosition;

    public void Init()
    {
        bob_tr.transform.parent = tether.tether_tr;
        arm.armLength = Vector3.Distance(bob_tr.transform.position, tether.tetherPos);
    }

    public Vector3 MoveBob(Vector3 pos,float time)
    {
        bob.velocity += GetConstrainedVelocity(pos, previousPosition, time);

        bob.ApplyGravity();
        bob.ApplyDamping();
        bob.ClampMaxSpeed();

        pos += bob.velocity * time;

        if(Vector3.Distance(pos,tether.tetherPos)<arm.armLength)
        {
            pos = Vector3.Normalize(pos - tether.tetherPos) * arm.armLength;
            arm.armLength = Vector3.Distance(pos, tether.tetherPos);
            return pos;
        }

        previousPosition = pos;

        return pos;
    }

    public Vector3 MoveBob(Vector3 pos, Vector3 previousPos, float time)
    {
        bob.velocity += GetConstrainedVelocity(pos, previousPos, time);

        bob.ApplyGravity();
        bob.ApplyDamping();
        bob.ClampMaxSpeed();

        pos += bob.velocity * time;

        if (Vector3.Distance(pos, tether.tetherPos) < arm.armLength)
        {
            pos = Vector3.Normalize(pos - tether.tetherPos) * arm.armLength;
            arm.armLength = Vector3.Distance(pos, tether.tetherPos);
            return pos;
        }

        previousPosition = pos;

        return pos;
    }

    public Vector3 GetConstrainedVelocity(Vector3 currentPos, Vector3 prevPos,float time)
    {
        float distToTether;
        Vector3 constrainedPos;
        Vector3 predictedPos;

        distToTether = Vector3.Distance(currentPos, tether.tetherPos);

        if(distToTether>arm.armLength)
        {
            constrainedPos = Vector3.Normalize(currentPos - tether.tetherPos) * arm.armLength;
            predictedPos = (constrainedPos - prevPos) / time;
            return predictedPos;
        }

        return Vector3.zero;
    }

    ///<summary> 
    ///Switches to new Position to swing on from 
    ///</summary>
    public void SwitchTetherPos(Vector3 newPos)
    {
        bob_tr.transform.parent = null;
        tether.tether_tr.position = newPos;
        bob_tr.transform.parent = tether.tether_tr;
        tether.tetherPos = tether.tether_tr.InverseTransformPoint(newPos);
        arm.armLength = Vector3.Distance(bob_tr.transform.localPosition, tether.tetherPos);
    }

    public Vector3 FallWhenNotSwinging(Vector3 pos, float time)
    {

        bob.ApplyGravity();
        bob.ApplyDamping();
        bob.ClampMaxSpeed();

        pos += bob.velocity * time;

        return pos;
    }
}
