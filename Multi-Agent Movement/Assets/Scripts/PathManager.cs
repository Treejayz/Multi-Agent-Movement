using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {

    public GameObject[] points;
    public GameObject character;
    private int index = 0;


    private void Start()
    {
        character.GetComponent<CharacterManager>().setUp(points[index]);
        
    }
    public void updatePoint()
    {
        index++;

        if(index == points.Length)
        {
            //done
            character.GetComponent<CharacterManager>().end = true;
        }
        else
        {
            character.GetComponent<CharacterManager>().setTarget(points[index]);
        }
        

    }
}
