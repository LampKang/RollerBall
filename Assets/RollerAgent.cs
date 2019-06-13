﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MLAgents;

public class RollerAgent : Agent
{
    Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;

    public override void AgentReset(){
        if(this.transform.localPosition.y < 0){
            // if the agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3( 0, 0.5f, 0);
        }
        
        //move the target to a new spot
        Target.localPosition = new Vector3( Random.value * 8 -4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(){
        //Target and Agent positions
        AddVectorObs(Target.localPosition);
        AddVectorObs(this.transform.localPosition);

        //Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
    }

    // Note that the Brain really has no idea what the values in the action array mean.
    public float speed=10;
    public override void AgentAction(float[] vectorAction, string textAction){
        //Actions, size=2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);

        //Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        //Reached target
        if(distanceToTarget < 1.42f){
            SetReward(1.0f);
            Done();
        }

        //Fell off platform
        if(this.transform.localPosition.y<0){
            Done();
        }
    }


}