using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {

    public float maxVelocity;
    public float maxAcceleration;

    public float cohesionForce;
    public float separationForce;
    public float alignForce;


    Vector2 velocity;
    Vector2 acceleration;

    List<GameObject> others = new List<GameObject>();



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        // First we just translate based on our velocity
        transform.Translate(velocity * Time.fixedDeltaTime);

        // Add our acceleration
        velocity += acceleration * Time.fixedDeltaTime;
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }


        // Now we calculate the force towards the center of the flock
        Vector2 cohesion;
        Vector2 direction = FlockControl.center - (Vector2)transform.position;
        float proximity = direction.magnitude;
        cohesion = direction.normalized * cohesionForce;

        // Next we calculate the repulsion force (if any)
        Vector2 repulsion = Vector2.zero;
        foreach(GameObject other in others)
        {
            // The force is away from the other, and increases when closer to this
            direction = (Vector2)other.transform.position - (Vector2)transform.position;
            proximity = (GetComponent<CircleCollider2D>().radius - direction.magnitude) / GetComponent<CircleCollider2D>().radius;
            repulsion += direction.normalized * proximity * separationForce;
        }

        //Finally we get the direction we should be heading based on the flock and the mouse position
        Vector2 align = FlockControl.target.normalized * alignForce;

        acceleration = cohesion + repulsion + align;
        if (acceleration.magnitude > maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }
        velocity += acceleration * Time.fixedDeltaTime;
	}



    private void OnTriggerEnter2D(Collider2D collision)
    {
        others.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        others.Remove(collision.gameObject);
    }
}
