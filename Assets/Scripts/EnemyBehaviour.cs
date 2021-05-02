using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Zombie Script
public class EnemyBehaviour : MonoBehaviour
{
    public Transform target;
    public float speed;
    Rigidbody rigidbody;
    private bool isDead = false;

    void Start()
    {
    	//randomly spawn this zombie in the outer region of the arena
    	float dir = (Random.Range(0f, 1f));
		if(dir>0.75){
			transform.position = new Vector3(-12f, 1.5f, Random.Range(-12.0f, 12.0f));
		}
		else if(dir >0.5){
			transform.position = new Vector3(12f, 1.5f, Random.Range(-12.0f, 12.0f));
		}
		else if(dir >0.25){
			transform.position = new Vector3(Random.Range(-12.0f, 12.0f), 1.5f,12f );
		}
		else if(dir >=0){
			transform.position = new Vector3(Random.Range(-12.0f, 12.0f), 1.5f, -12f);
		}
       	 
       	target = GameObject.FindWithTag("Agent").transform;
       	rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    	//Constantly move towards the player
    	if(!isDead){
    		transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
    	}
    }

    //Destroy the zombie and play the death animation
    void die(Vector3 direction){
    	isDead = true;
    	Debug.Log("DEAD");
    	rigidbody.constraints = RigidbodyConstraints.None;
    	GetComponent<BoxCollider>().enabled = false;
    	rigidbody.AddForce(direction * 200);
    	rigidbody.AddTorque(transform.right * -2000);
    	Destroy(gameObject,1.0f);
    }
}
