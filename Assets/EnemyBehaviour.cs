using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float speed = 1f;
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
    	transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
    }
}
