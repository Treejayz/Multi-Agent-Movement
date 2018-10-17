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

    public bool collisionPrediction = true;
    public float avoidanceForce;


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
        transform.position += (Vector3)velocity * Time.fixedDeltaTime;

        // Set the rotation
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), .05f);

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
            float closest = coneRange;
            foreach (GameObject obstacle in obstacles)
            {
                direction = (Vector2)transform.position - (Vector2)obstacle.transform.position;
                // check if the obstacle is in the cone
                if (direction.magnitude < coneRange && direction.magnitude < closest && Vector2.Angle(velocity.normalized, direction.normalized * -1) < coneAngle)
                {
                    coneAvoidance = direction.normalized * Mathf.Pow(((coneRange - direction.magnitude) / coneRange), 2);
                    closest = direction.magnitude;
                }
            }
        }

        // Now we do some collision prediction
        Vector2 collisionAvoidance = Vector2.zero;
        if (collisionPrediction)
        {
            // Only use the one that we will collide with the soonest
            float shortestTime = 1;

            // Check all obstacles inside our triggered area
            foreach (GameObject obstacle in obstacles) {
                Vector2 relativePos = obstacle.transform.position - transform.position;
                Vector2 relativeVel = obstacle.transform.GetComponentInParent<FlockingAvoidance>().velocity - velocity;
                float t = -1 * ((Vector2.Dot(relativePos, relativeVel)) / Mathf.Pow(relativeVel.magnitude, 2));
                float minSep = relativePos.magnitude - (relativeVel.magnitude * t);
                if (t > 0 && t < shortestTime && minSep < .2f)
                {
                    Vector2 targetPos = (Vector2)transform.position + velocity * t;
                    transform.GetChild(2).gameObject.SetActive(true);
                    transform.GetChild(2).position = (Vector3)targetPos;
                    shortestTime = t;
                    if (minSep <= 0 || relativePos.magnitude < .2f)
                    {
                        collisionAvoidance = relativePos.normalized * -1;
                    }
                    else
                    {
                        
                        direction = (Vector2)transform.position - targetPos;

                        collisionAvoidance = direction.normalized;
                    }
                }
            }
        }

        if (collisionAvoidance == Vector2.zero)
        {
            transform.GetChild(2).gameObject.SetActive(false);
        }

        // If both avoidances are checked, we should average the two (otherwise they dont move
        Vector2 avoidance = collisionAvoidance + coneAvoidance;
        if (coneCheck && collisionPrediction)
        {
            avoidance /= 2f;
        }
        avoidance *= avoidanceForce;
        

        //Finally we get the direction we should be heading
        Vector2 align = manager.Direction(group) * alignForce;

        // Sum up and clamp the accelerations
        acceleration = cohesion + repulsion + align + avoidance;
        if (acceleration.magnitude > maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }
        velocity += acceleration * Time.fixedDeltaTime;
	}



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When something enters the trigger, check if it's in our flock or not
        // Friendlies are for separation, and others are for avoidance.

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
        // Remove from the correct list

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
