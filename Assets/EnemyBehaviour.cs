using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    public float speed = 1f;
    void Start()
    {
       transform.position = new Vector3(Random.Range(-12.0f, -10.0f), 1.5f, Random.Range(-12.0f, 12.0f)); 
       target = GameObject.FindWithTag("Agent").transform;
    }

    // Update is called once per frame
    void Update()
    {
    	transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
    }
}
