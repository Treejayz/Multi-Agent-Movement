using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLevelManager : MonoBehaviour {

    public GameObject boid;
    public int numBoids;
    public Vector2 offset = new Vector2(-0.5f, 0.2f);

    public Transform[] path;
    int index = 0;

    List<GameObject> Boids;
    List<GameObject> targets;

    Dictionary<int, GameObject> boidToTarget;

	// Use this for initialization
	void Start () {

        transform.position = path[0].position;
        index = 1;
        Vector2 direction = (Vector2)path[1].position - (Vector2)transform.position;
        direction.Normalize();
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);


        GameObject targetSpawn = new GameObject("Target");
        targets = new List<GameObject>();
        Boids = new List<GameObject>();
        boidToTarget = new Dictionary<int, GameObject>();
        for (int i = 0; i < numBoids; i++) {
            Vector2 tempOffset = offset * ((i / 2) + 1);
            if (i % 2 == 1)
            {
                tempOffset.y = tempOffset.y * -1;
            }
            GameObject target = Instantiate(targetSpawn, transform, false);
            target.transform.localPosition = tempOffset;
            targets.Add(target);

            GameObject temp = Instantiate(boid, target.transform.position, Quaternion.identity);
            temp.GetComponent<TwoLevelFormation>().slot = i;
            temp.GetComponent<TwoLevelFormation>().target = target.transform;
            Boids.Add(temp);
            boidToTarget.Add(i, target);
        }


	}
	
	// Update is called once per frame
	void Update () {
        Vector2 anchorPos = Vector2.zero;
        Vector2 anchorVel = Vector2.zero;

        int i = 0;
        foreach (GameObject boid in Boids)
        {
            anchorVel += boid.GetComponent<TwoLevelFormation>().velocity;
            Vector2 localOffset = (Vector2)boid.GetComponent<TwoLevelFormation>().target.position - (Vector2)boid.transform.position;
            anchorPos += localOffset;
            i += 1;
        }

        anchorPos /= (float)i;
        anchorVel /= (float)i;
        //transform.position -= (Vector3)anchorPos;

        Vector2 target = path[index].position;
        if (index < path.Length - 1 && Vector2.Distance(path[index].position, transform.position) < 0.3f)
        {
            if (anchorPos.magnitude < 1f)
            {
                index += 1;
                target = path[index].position;
            } else
            {
                target = (Vector2)(transform.position);
            }
        }

        
        Vector2 direction = target - (Vector2)transform.position;
        if (direction != Vector2.zero)
        {
            direction.Normalize();
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            transform.position += (Vector3)direction * 1.5f * Time.deltaTime;
        }
    }

    public void Remove(GameObject boid)
    {
        if (!Boids.Contains(boid)) { return; }

        int slot = boid.GetComponent<TwoLevelFormation>().slot;
        Transform target = boid.GetComponent<TwoLevelFormation>().target;
        Boids.Remove(boid);
        Destroy(boid);
        for (int i = 0; i < Boids.Count; i++)
        {
            if (Boids[i].GetComponent<TwoLevelFormation>().slot > slot)
            {
                int temp = Boids[i].GetComponent<TwoLevelFormation>().slot;
                Boids[i].GetComponent<TwoLevelFormation>().slot = slot;
                slot = temp;
                Transform tempTarget = Boids[i].GetComponent<TwoLevelFormation>().target;
                Boids[i].GetComponent<TwoLevelFormation>().target = target;
                target = tempTarget;
            }
        }
    }
}
