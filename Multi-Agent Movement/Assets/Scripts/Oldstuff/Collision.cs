using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    public float avoidDistance;
    public float lookAhead;
    public float whiskerLookAhead;
    public Character character;
    public GameObject rayDraw;
    public GameObject whisker_1Draw;
    public GameObject whisker_2Draw;


    public struct CollisionData
    {
        public Vector3 position;
        public Vector3 normal;

        public CollisionData(Vector3 _position, Vector3 _normal)
        {
            this.position = _position;
            this.normal = _normal;
        }
    }

    public CollisionData getCollision(Vector3 position, Vector3 moveAmount, Vector3 currentTarget, float _lookAhead)
    {
        
        RaycastHit2D hit;
        Debug.DrawRay(position, moveAmount);
        if(hit = Physics2D.Raycast(position, moveAmount))
        {
            if(hit.transform != this.transform)
            {

            }
            if(hit.distance <= _lookAhead)
            {
                CollisionData returnData = new CollisionData(hit.transform.position, hit.normal);
                return returnData;
            }
        }
        return new CollisionData(currentTarget, new Vector3(0,0));
    }
	public Vector3 collisionTarget(Vector3 currentTarget)
    {

        Vector3 rayVector = character.staticInfo.velocity;
        rayVector.Normalize();
        rayVector *= lookAhead;

        Vector3 whisker_1 = Quaternion.AngleAxis(30, Vector3.up) * character.staticInfo.velocity ;
        whisker_1.Normalize();
        whisker_1 *= whiskerLookAhead;
        Vector3 whisker_2 = Quaternion.AngleAxis(330, Vector3.up) * character.staticInfo.velocity;
        whisker_2.Normalize();
        whisker_2 *= whiskerLookAhead;

        whisker_1Draw.GetComponent<DrawRay>().updatePoints(character.staticInfo.position, character.staticInfo.position + whisker_1);
        whisker_2Draw.GetComponent<DrawRay>().updatePoints(character.staticInfo.position, character.staticInfo.position + whisker_2);
        rayDraw.GetComponent<DrawRay>().updatePoints(character.staticInfo.position, character.staticInfo.position + rayVector);



        Vector2 newTarget = currentTarget;
        RaycastHit2D hit; 
        if(hit = Physics2D.Raycast(character.staticInfo.position, rayVector))
        {
            if(hit.transform != this.transform)
            {
                if(hit.distance <= lookAhead)
                {
                    newTarget = hit.point + hit.normal * avoidDistance;
                    gameObject.GetComponent<CharacterManager>().collisionDetected = true;
                }
                
            }
        }
      

        else if(hit = Physics2D.Raycast(character.staticInfo.position, whisker_1))
        {
            if(hit.transform != this.transform)
            {
                if(hit.distance <= whiskerLookAhead)
                {
                    newTarget = hit.point + hit.normal * avoidDistance;
                    gameObject.GetComponent<CharacterManager>().collisionDetected = true;
                }
                
            }
        }

        else if (hit = Physics2D.Raycast(character.staticInfo.position, whisker_2))
        {
            if (hit.transform != this.transform)
            {
                if (hit.distance <= whiskerLookAhead)
                {
                    newTarget = hit.point + hit.normal * avoidDistance;
                    gameObject.GetComponent<CharacterManager>().collisionDetected = true;
                }
            }
        }


        return newTarget;
    }
}
