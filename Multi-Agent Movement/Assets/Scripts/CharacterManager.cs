using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : MonoBehaviour {

    private Character character;
    private Kinematics kinematics;
    public GameObject target;
    public float rotation;
    public float maxSpeed;
    public float angular;
    public Vector2 linear;

    private void Start()
    {
        kinematics = this.GetComponent<Kinematics>();
        kinematics.character = this.gameObject;
        kinematics.maxSpeed = maxSpeed;
        kinematics.target = target.transform.position;

        character = this.GetComponent<Character>();
        character.r = rotation;
        character.a = angular;
        character.l = linear;
    }
    // Update is called once per frame
    void Update ()
    {
        Kinematics.KinematicSteeringOutput steering = kinematics.Seek();

        character.staticInfo.velocity = steering.velocity;
        character.updateSteering(Time.deltaTime);

        this.transform.position = character.staticInfo.position;
        this.transform.rotation = Quaternion.Euler(0, 0, character.staticInfo.orientation * Mathf.Rad2Deg);
    }
}
