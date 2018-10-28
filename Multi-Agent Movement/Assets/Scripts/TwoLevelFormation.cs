using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLevelFormation : MonoBehaviour {

    public int slot;
    public Transform target;
    public Vector2 offset = Vector2.zero;

    [HideInInspector]
    public Vector2 velocity = Vector2.zero;
    Vector2 acceleration = Vector2.zero;

    public LayerMask collisionlayer;

    // Update is called once per frame. Using late update so that this updates after the manager
    void LateUpdate () {

        Arrive();
        velocity += acceleration * Time.deltaTime;
        if (velocity.magnitude > 3.5f)
        {
            velocity = velocity.normalized * 3.5f;
        }
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "ControlledBoid")
        {
            print("hi");
            target.transform.parent.GetComponent<TwoLevelManager>().Remove(this.gameObject);
        }
    }

    public void Arrive()
    {
        /* Get the right direction for the linear acceleration */
        Vector2 targetPos = (Vector2)target.position + offset * target.forward;
        Vector2 targetVelocity = targetPos - (Vector2)transform.position;

        // Now we move our target based on collision detection
        // First determine where to raycast
        Vector3 rayVector = targetVelocity;
        rayVector.Normalize();
        Vector3 whisker_1 = Quaternion.AngleAxis(30, Vector3.up) * rayVector;
        whisker_1.Normalize();
        Vector3 whisker_2 = Quaternion.AngleAxis(330, Vector3.up) * rayVector;
        whisker_2.Normalize();

        // Now check ahead of us, and move the target if something is in the way
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(transform.position, rayVector, .5f, collisionlayer))
        {
            targetPos = targetPos + (hit.point - hit.normal * .25f) / 2f;
            targetVelocity = targetPos - (Vector2)transform.position;
        }

        else if (hit = Physics2D.Raycast(transform.position, whisker_1, .25f, collisionlayer))
        {
            targetPos = targetPos + (hit.point - hit.normal * 0.125f) / 2f;
            targetVelocity = targetPos - (Vector2)transform.position;
        }

        else if (hit = Physics2D.Raycast(transform.position, whisker_2, .25f, collisionlayer))
        {
            targetPos = targetPos + (hit.point - hit.normal * 0.125f) / 2f;
            targetVelocity = targetPos - (Vector2)transform.position;
        }

        /* Get the distance to the target */
        float dist = targetVelocity.magnitude;

        /* If we are within the stopping radius then stop */
        if (dist < .005)
        {
            velocity = Vector2.zero;
            acceleration = Vector2.zero;
        }

        /* Calculate the target speed, full speed at slowRadius distance and 0 speed at 0 distance */
        float targetSpeed;
        if (dist > 1)
        {
            targetSpeed = 3.5f;
        }
        else
        {
            targetSpeed = 3.5f * (dist / 3.5f);
        }

        /* Give targetVelocity the correct speed */
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        /* Calculate the linear acceleration we want */
        acceleration = targetVelocity - velocity;
        acceleration *= 4f;

        /* Make sure we are accelerating at max acceleration */
        if (acceleration.magnitude > 10f)
        {
            acceleration.Normalize();
            acceleration *= 10f;
        }

    }
}
