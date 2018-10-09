using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockControl : MonoBehaviour {

    public GameObject boid;
    [Range(5, 50)]
    public int numBoids;

    Camera cam;

    [HideInInspector]
    static public Vector2 center;
    [HideInInspector]
    static public Vector2 target;


    Vector2 mousePos;

    GameObject[] boids;

	// Use this for initialization
	void Start () {
        cam = Camera.main;

        boids = new GameObject[numBoids];
        for (int i = 0; i < numBoids; i++)
        {
            float x, y;
            x = Random.Range(numBoids * -0.05f, numBoids * 0.05f);
            y = Random.Range(numBoids * -0.05f, numBoids * 0.05f);
            GameObject temp = Instantiate(boid, new Vector3(x, y), Quaternion.identity);
            boids[i] = temp;
        }
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 sum = Vector2.zero;
		foreach (GameObject boid in boids)
        {
            sum += (Vector2)boid.transform.position;
        }
        center = sum / numBoids;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        target = mousePos - center;

	}
}
