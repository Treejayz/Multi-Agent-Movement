﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : MonoBehaviour {

    private Character character;
    private Kinematics kinematics;
    private Collision collision;
    public GameObject target;
    public float rotation;
    public float maxSpeed;
    public float angular;
    public Vector2 linear;
    public float lookAhead;
    public float avoidDistance;

    private GameObject debug;

    private void Start()
    {
        debug = Instantiate(target);
        kinematics = this.GetComponent<Kinematics>();
        kinematics.character = this.gameObject;
        kinematics.maxSpeed = maxSpeed;
        kinematics.target = target.transform.position;

        character = this.GetComponent<Character>();
        character.r = rotation;
        character.a = angular;
        character.l = linear;

        collision = this.GetComponent<Collision>();
        collision.character = character;
        collision.lookAhead = lookAhead;
        collision.avoidDistance = avoidDistance;
        collision.whiskerLookAhead = lookAhead / 2;
    }
    // Update is called once per frame
    void Update ()
    {

        Vector3 newTarget = collision.collisionTarget(kinematics.target);
        
        debug.transform.position = newTarget;
        kinematics.target = newTarget;
        Kinematics.KinematicSteeringOutput steering = kinematics.Seek();

        character.staticInfo.velocity = steering.velocity;
        character.updateSteering(Time.deltaTime);

        this.transform.position = character.staticInfo.position;
        this.transform.rotation = Quaternion.Euler(0, 0, character.staticInfo.orientation * Mathf.Rad2Deg);

        if((character.staticInfo.position - kinematics.target).magnitude < .1) { kinematics.target = target.transform.position; }
        
    }
}
