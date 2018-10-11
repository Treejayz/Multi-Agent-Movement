using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAvoidance : MonoBehaviour {


    // These variables are the same as regular Flocking
    public float maxVelocity;
    public float maxAcceleration;

    public float cohesionForce;
    public float separationForce;
    public float alignForce;

    // New public variables needed for avoidance
    [Range(0, 3)]
    public float separationRange = 1.5f;

    public bool coneCheck = true;
    [Range(0, 3)]
    public float coneRange = 2f;
    [Range(0, 60)]
    public float coneAngle = 30f;
    public float coneForce;
    public bool collisionPrediction = true;
    public float predictionForce;


    [HideInInspector]
    public string group = "Boid1";

    [HideInInspector]
    public Vector2 velocity;
    Vector2 acceleration;

    List<GameObject> others = new List<GameObject>();
    List<GameObject> obstacles = new List<GameObject>();

    [HideInInspector]
    public AvoidanceManager manager;

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
        Vector2 direction = manager.Center(group) - (Vector2)transform.position;
        float proximity = direction.magnitude;
        cohesion = direction.normalized * cohesionForce;

        // Next we calculate the repulsion force (if any)
        Vector2 repulsion = Vector2.zero;
        foreach(GameObject other in others)
        {
            // The force is away from the other, and increases when closer to this
            direction = (Vector2)transform.position - (Vector2)other.transform.position;
            if (direction.magnitude < separationRange)
            {
                proximity = (separationRange - direction.magnitude) / separationRange;
                proximity = Mathf.Clamp01(proximity);
                proximity = proximity * proximity;
                repulsion += direction.normalized * proximity * separationForce;
            }
        }

        // Now we do some Object Avoidance stuff
        // First, check if we are doing cone checking
        Vector2 coneAvoidance = Vector2.zero;
        if (coneCheck)
        {
            int numObstacles = 0;
            foreach (GameObject obstacle in obstacles)
            {
                direction = (Vector2)transform.position - (Vector2)obstacle.transform.position;
                // check if the obstacle is in the cone
                if (direction.magnitude < coneRange && Vector2.Angle(velocity.normalized, direction.normalized * -1) < coneAngle)
                {
                    coneAvoidance += direction.normalized * Mathf.Pow(((coneRange - direction.magnitude) / coneRange), 2);
                    numObstacles += 1;
                }
            }
            if (numObstacles > 1)
            {
                coneAvoidance /= numObstacles;
            }
            coneAvoidance *= coneForce;
        }

        // Now we do some collision prediction
        Vector2 collisionAvoidance = Vector2.zero;
        if (collisionPrediction)
        {
            int numObstacles = 0;
            foreach (GameObject obstacle in obstacles) {
                Vector2 relativePos = obstacle.transform.position - transform.position;
                Vector2 relativeVel = obstacle.transform.GetComponentInParent<FlockingAvoidance>().velocity - velocity;
                float t = -1 * ((Vector2.Dot(relativePos, relativeVel)) / Mathf.Pow(relativeVel.magnitude, 2));
                if (t > 0)
                {
                    Vector2 targetPos = (Vector2)transform.position + velocity * t;
                    direction = (Vector2)transform.position - targetPos;
                    if (direction.magnitude < coneRange && Vector2.Angle(velocity.normalized, direction.normalized * -1) < coneAngle)
                    {
                        coneAvoidance += direction.normalized * Mathf.Pow(((coneRange - direction.magnitude) / coneRange), 2);
                        numObstacles += 1;
                    }
                }
            }
            if (numObstacles > 1)
            {
                collisionAvoidance /= numObstacles;
            }
            collisionAvoidance *= predictionForce;
        }

        //Finally we get the direction we should be heading based on the flock and the mouse position
        Vector2 align = manager.Direction(group) * alignForce;

        acceleration = cohesion + repulsion + align + coneAvoidance + collisionAvoidance;
        if (acceleration.magnitude > maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }
        velocity += acceleration * Time.fixedDeltaTime;
	}



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (string.Compare(collision.tag, group) == 0)
        {
            others.Add(collision.gameObject);
        }
        else if (string.Compare(collision.tag, "Untagged") != 0)
        {
            obstacles.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (string.Compare(collision.tag, group) == 0)
        {
            others.Remove(collision.gameObject);
        }
        else if (string.Compare(collision.tag, "Untagged") != 0)
        {
            obstacles.Remove(collision.gameObject);
        }
    }
}
