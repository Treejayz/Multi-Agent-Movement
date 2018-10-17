using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceManager : MonoBehaviour {

    public GameObject boid;

    public Vector2[] targets1, targets2;
    int index = 0;


    GameObject[] group1, group2;

    bool coneCheck = true;
    bool colPredict = true;

    // Use this for initialization
    void Start()
    {
        // 2 separate flocks
        group1 = new GameObject[6];
        group2 = new GameObject[6];
        //Instantiate the flocks
        for (int i = 0; i < 6; i++)
        {
            float x, y;
            x = Random.Range(6 * -0.1f, 6 * 0.1f) - 8f;
            y = Random.Range(6 * -0.1f, 6 * 0.1f) - 3.5f;
            GameObject temp = Instantiate(boid, new Vector3(x, y), Quaternion.identity);
            temp.GetComponent<FlockingAvoidance>().group = "Boid1";
            temp.GetComponent<FlockingAvoidance>().manager = this;
            temp.GetComponent<FlockingAvoidance>().coneCheck = coneCheck;
            temp.GetComponent<FlockingAvoidance>().collisionPrediction = colPredict;
            temp.transform.GetChild(0).tag = "Boid1";
            group1[i] = temp;
        }
        for (int i = 0; i < 6; i++)
        {
            float x, y;
            x = Random.Range(6 * -0.1f, 6 * 0.1f) - 8f;
            y = Random.Range(6 * -0.1f, 6 * 0.1f) + 3.5f;
            GameObject temp = Instantiate(boid, new Vector3(x, y), Quaternion.identity);
            temp.GetComponent<FlockingAvoidance>().group = "Boid2";
            temp.GetComponent<FlockingAvoidance>().manager = this;
            temp.GetComponent<FlockingAvoidance>().coneCheck = coneCheck;
            temp.GetComponent<FlockingAvoidance>().collisionPrediction = colPredict;
            temp.transform.GetChild(0).tag = "Boid2";
            group2[i] = temp;
        }

        toggleCone();
    }

    // Update is called once per frame
    void Update()
    {
        // This sets the targets so that the groups follow a path
        if (index < targets1.Length - 1)
        {
            float distance1 = (Center("Boid1") - targets1[index]).magnitude;
            float distance2 = (Center("Boid2") - targets2[index]).magnitude;
            if (distance1 < 1f && distance2 < 1f)
            {
                index += 1;
            }
        }
    }

    // Calculates the center for each individual group
    public Vector2 Center(string group)
    {

        Vector2 sum = Vector2.zero;
        if (string.Compare(group, "Boid1") == 0)
        {
            foreach (GameObject boid in group1)
            {
                sum += (Vector2)boid.transform.position;
            }
        } else
        {
            foreach (GameObject boid in group2)
            {
                sum += (Vector2)boid.transform.position;
            }
        }

        return (sum / 6);
    }

    // Calculates the flock direction for each group
    public Vector2 Direction(string group)
    {
        if (string.Compare(group, "Boid1") == 0)
        {
            Vector2 center = Center(group);
            return (targets1[index] - center).normalized;
        }
        else
        {
            Vector2 center = Center(group);
            return (targets2[index] - center).normalized;
        }
    }

    // Sets cone check for the boids
    public void SetConeCheck()
    {
        coneCheck = !coneCheck;
        for (int i = 0; i < 6; i++)
        {
            group1[i].GetComponent<FlockingAvoidance>().coneCheck = coneCheck;
            group2[i].GetComponent<FlockingAvoidance>().coneCheck = coneCheck;
        }
        toggleCone();
    }

    public void toggleCone()
    {
        for (int i = 0; i < 6; i++)
        {
            group1[i].transform.GetChild(1).gameObject.SetActive(coneCheck);
            group2[i].transform.GetChild(1).gameObject.SetActive(coneCheck);
        }
    }

    // Sets collision prediction for each boid
    public void SetCollisionPrediction()
    {
        colPredict = !colPredict;
        for (int i = 0; i < 6; i++)
        {
            group1[i].GetComponent<FlockingAvoidance>().collisionPrediction = colPredict;
            group2[i].GetComponent<FlockingAvoidance>().collisionPrediction = colPredict;
        }
    }

    // Just resets the velocities and positions of the boids in each group
    public void Reset()
    {
        index = 0;
        for (int i = 0; i < 6; i++)
        {
            float x, y;
            x = Random.Range(6 * -0.1f, 6 * 0.1f) - 8f;
            y = Random.Range(6 * -0.1f, 6 * 0.1f) - 3.5f;
            group1[i].transform.position = new Vector3(x, y);
            group1[i].GetComponent<FlockingAvoidance>().velocity = Vector2.zero;
        }
        for (int i = 0; i < 6; i++)
        {
            float x, y;
            x = Random.Range(6 * -0.1f, 6 * 0.1f) - 8f;
            y = Random.Range(6 * -0.1f, 6 * 0.1f) + 3.5f;
            group2[i].transform.position = new Vector3(x, y);
            group2[i].GetComponent<FlockingAvoidance>().velocity = Vector2.zero;
        }
    }
}
