using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRay : MonoBehaviour {

    private LineRenderer line;
	// Use this for initialization
	void Start ()
    {
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	public void updatePoints(Vector3 _playerPos, Vector3 _otherPos)
    {
        
        line.SetPosition(0, _playerPos);
        line.SetPosition(1, _otherPos);
	}
}
