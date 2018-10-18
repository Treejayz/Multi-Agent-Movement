using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematics : MonoBehaviour {

    public GameObject character;
    public Vector3 target;
    public float maxSpeed;
    public float targetRadius;
    public float slowRadius;
    public float timeToTarget;
    public float maxAcceleration;

    public struct KinematicSteeringOutput
    {
        public Vector3 velocity;
        public float rotation;

        public KinematicSteeringOutput(Vector3 _velocity, float _rotation)
        {
            this.velocity = _velocity;
            this.rotation = _rotation;

        }
    }
	
    public float getNewOrientation(float currentOrientation, Vector3 velocity)
    {
        if(velocity.magnitude > 0)
        {
            return Mathf.Atan2(-velocity.x, velocity.y);
        }
        else
        {
            return currentOrientation;
        }
    }

    //seeking
    public KinematicSteeringOutput Seek()
    {
        KinematicSteeringOutput steering = new KinematicSteeringOutput(new Vector3(0, 0, 0), 0f);
        //direction toward target. 
        steering.velocity = target - character.GetComponent<Character>().staticInfo.position;

        //normalize velocity
        steering.velocity.Normalize();
        steering.velocity *= maxSpeed;

        //face the direction we want to move. 
        character.GetComponent<Character>().staticInfo.orientation = getNewOrientation(character.GetComponent<Character>().staticInfo.orientation, steering.velocity);

        steering.rotation = 0; 
        return steering; 

    }

    public KinematicSteeringOutput Arrive()
    {
        KinematicSteeringOutput steering = new KinematicSteeringOutput();

        Vector3 direction = target - character.GetComponent<Character>().staticInfo.position;
        float distance = direction.magnitude;

        float targetSpeed;
        if (distance < targetRadius)
        {

            steering.velocity = new Vector3(0, 0, 0);
            return steering;
        }

        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }

        else
        {
            targetSpeed = maxSpeed * distance / slowRadius;
        }

        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        character.GetComponent<Character>().steering.linear = targetVelocity - character.GetComponent<Character>().staticInfo.velocity;
        character.GetComponent<Character>().steering.linear /= timeToTarget;

        if (character.GetComponent<Character>().steering.linear.magnitude > maxAcceleration)
        {
            character.GetComponent<Character>().steering.linear.Normalize();
            character.GetComponent<Character>().steering.linear *= maxAcceleration;
        }

        character.GetComponent<Character>().steering.angular = 0;
        steering.velocity = targetVelocity;
        return steering;
    }
}
