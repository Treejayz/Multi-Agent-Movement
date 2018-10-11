using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematics : MonoBehaviour {

    public GameObject character;
    public Vector3 target;
    public float maxSpeed; 

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
}
