using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : MonoBehaviour {

    private Character character;
    public Kinematics kinematics;
    private Collision collision;
    public Canvas canvas;
    public Vector3 target;
    public float rotation;
    public float maxSpeed;
    public float angular;
    public Vector2 linear;
    public float lookAhead;
    public float avoidDistance;
    public bool leader;
    public bool collisionDetected = false;
    public bool pause = false;
    
    private PathManager path;
    public bool end = false;

    private GameObject debug;


    // Update is called once per frame
    public void setUp(Vector3 _target)
    {
        canvas.enabled = false;
        target = _target;
        if (leader)
        {
            path = GameObject.Find("PathManager").GetComponent<PathManager>();
        }    
        //debug = Instantiate(target);
        kinematics = this.GetComponent<Kinematics>();
        kinematics.character = this.gameObject;
        kinematics.maxSpeed = maxSpeed;
        kinematics.target = _target;
        

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
    public void setTarget(Vector3 _target)
    {
        target = _target;
        kinematics.target = _target;
    }
    void Update ()
    {
        if (!end)
        {
            if (!pause)
            {
                updateMananger();
            }
        }
        else
        {
            canvas.enabled = true;
        }
        
        
    }

    void updateMananger()
    {
        Vector3 newTarget = collision.collisionTarget(kinematics.target);

        //debug.transform.position = newTarget;
        kinematics.target = newTarget;
        Kinematics.KinematicSteeringOutput steering = kinematics.Seek();

        character.staticInfo.velocity = steering.velocity;
        character.updateSteering(Time.deltaTime);

        this.transform.position = character.staticInfo.position;
        this.transform.rotation = Quaternion.Euler(0, 0, character.staticInfo.orientation * Mathf.Rad2Deg);

        
        if ((character.staticInfo.position - kinematics.target).magnitude < .1) { kinematics.target = target; }
        if ((character.staticInfo.position - target).magnitude < .2 && leader) { path.updatePoint(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject.Find("FormationManager").GetComponent<ScalableFormationManager>().SendMessage("removeBird", this.gameObject);

    }
}
