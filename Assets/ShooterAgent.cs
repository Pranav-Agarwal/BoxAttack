using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ShooterAgent : Agent
{
    Rigidbody rBody;
    public GameObject enemy;
    public int enemyCount = 3;
    //public int maxHp = 100;
    //private int hp = maxHp;

    void Start () {
        rBody = GetComponent<Rigidbody>();
        resetEnemies();
    }

    //public Transform Target;
    public override void OnEpisodeBegin(){
       // If the Agent fell, zero its momentum
	    this.rBody.angularVelocity = Vector3.zero;
	    this.rBody.velocity = Vector3.zero;
	    this.transform.localPosition = new Vector3( 0, 1.5f, 0);
     	//Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity);
	    //this.hp = maxHp;
        // // Move the target to a new spot
        // Target.localPosition = new Vector3(Random.value * 8 - 4,
        //                                    0.5f,
        //                                    Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor){
	    // Agent positions
	    //sensor.AddObservation(rBody.transform.position.x);
	    //sensor.AddObservation(rBody.transform.position.z);
   	 	// Agent velocity
	    //sensor.AddObservation(rBody.velocity.x);
	    //sensor.AddObservation(rBody.velocity.z);
	    //Agent rotation
	    //sensor.AddObservation(rBody.rotation.y);
	    //sensor.AddObservation(rBody.angularVelocity.y);


	}

	public float speed = 10;
	public float rotationSpeed = 10;

	public override void OnActionReceived(ActionBuffers actionBuffers){
	    // Actions, size = 2
	    //Vector3 controlPosition = Vector3.zero;
	    //Vector3 controlRotation = Vector3.zero;
	    //controlPosition.x = actionBuffers.ContinuousActions[0];
	    //controlPosition.z = actionBuffers.ContinuousActions[1];
	    //controlRotation.y = actionBuffers.ContinuousActions[2];
	    //rBody.AddForce(controlPosition * forceMultiplier);
	    //rBody.AddTorque(controlRotation * torqueMultiplier);
	    rBody.velocity = new Vector3(actionBuffers.ContinuousActions[0]*speed,0,actionBuffers.ContinuousActions[1]*speed);
	    rBody.angularVelocity = new Vector3(0,actionBuffers.ContinuousActions[2]*rotationSpeed,0);
	    AddReward(5f);

	    // // Rewards
	    // float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

	    // // Reached target
	    // if (distanceToTarget < 1.42f)
	    // {
	    //     SetReward(1.0f);
	    //     EndEpisode();
	    // }

	    // // Fell off platform
	    // else if (this.transform.localPosition.y < 0)
	    // {
	    //     EndEpisode();
	    // }
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
	    var continuousActionsOut = actionsOut.ContinuousActions;
	    continuousActionsOut[0] = Input.GetAxis("Horizontal");
	    continuousActionsOut[1] = Input.GetAxis("Vertical");
	    continuousActionsOut[2] = Input.GetAxis("Rotate");
	    //Debug.Log(continuousActionsOut[0]+" "+continuousActionsOut[1]+" "+continuousActionsOut[2]);
	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //enemyManager.SetEnemiesActive();
            AddReward(-10000f);
            resetEnemies();
            EndEpisode();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            //enemyManager.SetEnemiesActive();
            AddReward(-10000f);
            resetEnemies();
            EndEpisode();
        }
    }

    public void resetEnemies(){
    	Debug.Log("wtf");
    	GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
   		foreach(GameObject enemy in enemies)
   		GameObject.Destroy(enemy);
   		for(int i=0;i<this.enemyCount;i++){
   			Instantiate(this.enemy, new Vector3(0, 0, 0), Quaternion.identity);
   			Debug.Log("done");
   		}
    }

}
