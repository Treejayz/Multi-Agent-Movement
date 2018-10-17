using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledBoid : MonoBehaviour {

    public float maxVelocity;
    public float maxAcceleration;

    Vector2 velocity;
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector2 Acceleration;
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Acceleration = (mousePos - (Vector2)transform.position);
            Acceleration.Normalize();
        }
        else
        {
            Acceleration = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Acceleration.Normalize();
        }
        if (Acceleration.magnitude > 0.05f)
        {
            Acceleration *= maxAcceleration;
        }
        else
        {
            Acceleration = velocity * -0.5f * maxAcceleration;
        }
        velocity += Acceleration * Time.fixedDeltaTime;
        if (velocity.magnitude > maxVelocity) { velocity = velocity.normalized * maxVelocity; }

        transform.position += (Vector3)(velocity * Time.fixedDeltaTime);

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), .15f);

    }


}
