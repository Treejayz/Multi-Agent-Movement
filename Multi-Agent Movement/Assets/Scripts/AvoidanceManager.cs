using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceManager : MonoBehaviour {

    public GameObject boid;

    Camera cam;

    public Vector2[] targets1, targets2;
    int index = 0;

    Vector2 mousePos;

    GameObject[] group1, group2;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main;

        group1 = new GameObject[6];
        group2 = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            float x, y;
            x = Random.Range(6 * -0.1f, 6 * 0.1f) - 8f;
            y = Random.Range(6 * -0.1f, 6 * 0.1f) - 3.5f;
            GameObject temp = Instantiate(boid, new Vector3(x, y), Quaternion.identity);
            temp.GetComponent<FlockingAvoidance>().group = "Boid1";
            temp.GetComponent<FlockingAvoidance>().manager = this;
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
            temp.transform.GetChild(0).tag = "Boid2";
            group2[i] = temp;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
}
