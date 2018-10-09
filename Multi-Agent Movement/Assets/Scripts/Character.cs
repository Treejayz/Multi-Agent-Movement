using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public Kinematic staticInfo;
    public SteeringOutput steering;
    public float r;
    public float a;
    public Vector2 l; 

    public struct SteeringOutput
    {
        public float angular;
        public Vector3 linear;
        public SteeringOutput(float _angular, Vector3 _linear)
        {
            this.angular = _angular;
            this.linear = _linear;
        }
    }
    public struct Kinematic
    {
        public Vector3 position;
        public float orientation;
        public Vector3 velocity;
        public float rotation;
        
        public Kinematic(Vector3 _position, float _orientation, Vector3 _velocity, float _rotation)
        {
            this.position = _position;
            this.orientation = _orientation;
            this.velocity = _velocity;
            this.rotation = _rotation;
        }
    }
	// Use this for initialization
	void Start ()
    {
        staticInfo = new Kinematic(transform.position, 0f, new Vector3(0, 0, 0), r);
        steering = new SteeringOutput(a, l);
	}
	
    public void updateSteering(float time)
    {
        staticInfo.position += staticInfo.velocity * time;
        staticInfo.orientation += staticInfo.rotation * time;

        staticInfo.velocity += steering.linear * time;
        staticInfo.orientation += steering.angular * time;
    }

}
