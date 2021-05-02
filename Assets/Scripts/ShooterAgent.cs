using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

//Agent Script
public class ShooterAgent : Agent
{
    Rigidbody rBody;
    public GameObject enemy;
    public GameObject shootingPoint;
    public GameObject flash;
    private AudioSource gunSound;
    public int enemyCount = 3;
    public int maxFireDelay = 10;
    private int fireDelay = 0;
    private bool shotAvailable = true;
    public Text scoreText;
    public Text timeText;
    private float startTime;
    private int score = 0;

    void Start () {
        rBody = GetComponent<Rigidbody>();
        gunSound = GetComponent<AudioSource>();
        resetEnemies();
        startTime = Time.time;
    }
    public override void OnEpisodeBegin(){
    	//reset the agent to the center of the arena
	    this.rBody.angularVelocity = Vector3.zero;
	    this.rBody.velocity = Vector3.zero;
	    this.transform.localPosition = new Vector3( 0, 1.5f, 0);
	    this.transform.rotation = Quaternion.Euler(0,0,0);
	    score = 0;
	    startTime = Time.time;
    }

    public override void CollectObservations(VectorSensor sensor){
    	//This observation tells the agent whether a shot is available i.e enough time has passed since the last shot to shoot again
	    sensor.AddObservation(shotAvailable);
	}

	public float speed = 10;
	public float rotationSpeed = 10;

	public override void OnActionReceived(ActionBuffers actionBuffers){
		//Action space is 4 continous actions:
		// 1. X velocity
		// 2. Z velocity
		// 3. Rotation about Y axis
		// 4. Shoot (if recieved action value is > 0, then shoot.)
	    rBody.velocity = new Vector3(actionBuffers.ContinuousActions[0]*speed,0,actionBuffers.ContinuousActions[1]*speed);
	    rBody.angularVelocity = new Vector3(0,actionBuffers.ContinuousActions[2]*rotationSpeed,0);
	    if(actionBuffers.ContinuousActions[3]>0){
	    	shoot();
	    }
	}

    private void FixedUpdate()
    {
        if (!shotAvailable)
        {
            fireDelay--;

            if (fireDelay <= 0)
                shotAvailable = true;
        }
        //Give the agent a minor reward for time survived and update the GUI
        AddReward(0.02f);
        timeText.text = "Time Survived - " + (Time.time-startTime);
        scoreText.text = "Score - "+score;
    }

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		//To manually control the agent via keyboard input for testing
	    var continuousActionsOut = actionsOut.ContinuousActions;
	    continuousActionsOut[0] = Input.GetAxis("Horizontal");
	    continuousActionsOut[1] = Input.GetAxis("Vertical");
	    continuousActionsOut[2] = Input.GetAxis("Rotate");
	    continuousActionsOut[3] = Input.GetKey("i")?1f:-1f;
	}

    private void OnCollisionEnter(Collision other)
    {
    	//Collided with Zombie
        if (other.gameObject.CompareTag("Enemy"))
        {
            AddReward(-100f);
            resetEnemies();
            EndEpisode();
        }

        //Collided with Arena wall
        else if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(-200f);
            resetEnemies();
            EndEpisode();
        }
    }

    //Save stats for the episode to the stat recorder. Then destroy all zombies and respawn them afresh
    public void resetEnemies(){
    	var statsRecorder = Academy.Instance.StatsRecorder;
		statsRecorder.Add("Time survived",(Time.time-startTime),StatAggregationMethod.Average);
		statsRecorder.Add("Score",score,StatAggregationMethod.Average);
    	GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
   		foreach(GameObject enemy in enemies)
   		GameObject.Destroy(enemy);
   		for(int i=0;i<this.enemyCount;i++){
   			Instantiate(this.enemy, new Vector3(0, 0, 0), Quaternion.identity);
   		}
    }

    //Fire a shot
    private void shoot(){
    	if(shotAvailable){
    		fireDelay = maxFireDelay;
    		shotAvailable = false;
    		int layerMask = 1 << 8;
    		gunSound.PlayOneShot(gunSound.clip);
    		Instantiate(this.flash, shootingPoint.transform.position, Quaternion.LookRotation(shootingPoint.transform.right*-1));
    		RaycastHit hit;
    		Ray ray = new Ray(shootingPoint.transform.position, shootingPoint.transform.up);
    		if (Physics.Raycast(ray,out hit, Mathf.Infinity, layerMask)){
    			if(hit.collider.gameObject.CompareTag("Enemy")){
				 	hit.collider.gameObject.SendMessage("die", shootingPoint.transform.up);
				 	//Instantiate a new zombie as soon as one dies
				 	Instantiate(enemy, new Vector3(0, 100f, 0), Quaternion.identity);
				 	AddReward(50f);
				 	score++;
				}
				else{
					//Missed shot penalty
					AddReward(-5f);
				}
			}
			else{
				//Missed shot penalty
				AddReward(-5f);
			}

    	}
    	else{
    		return;
    	}
    }

}
