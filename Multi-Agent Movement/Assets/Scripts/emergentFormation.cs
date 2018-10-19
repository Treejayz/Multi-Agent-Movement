using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emergentFormation : MonoBehaviour {

    public List<GameObject> boids;
    public float dist = 1f;
    private List<FormationInfo> boidTree;

    public struct FormationInfo
    {
        public List<int> children;
        public int parent;
    }
    public void setUp(List<GameObject> _boids)
    {
        boids = _boids;
        boidTree = new List<FormationInfo>();

        for(int i = 0; i < boids.Count; i++)
        {
            if(boidTree.Count == 0)
            {
                FormationInfo info = new FormationInfo();
                info.parent = -1;
                info.children = new List<int>();
                boids[i].GetComponent<Character>().staticInfo.position = boids[i].transform.position;
                boidTree.Add(info);
            }
            else
            {
                for(int j = 0; j < i; j++)
                {
                    if(boidTree[j].children.Count < 2)
                    {
                        FormationInfo info = new FormationInfo();
                        info.parent = j;
                        info.children = new List<int>();
                        boidTree[j].children.Add(i);
                        boids[i].GetComponent<CharacterManager>().setUp(boids[j].transform.position);
                        boids[i].GetComponent<CharacterManager>().target = boids[j].transform.position;
                        boids[i].GetComponent<Character>().staticInfo.position = boids[i].transform.position;
                        boids[i].GetComponent<CharacterManager>().mode = "arrive";
                        boidTree.Add(info);
                    }
                }
            }
        }

    }

    public Vector3 parentLocationRight(int _childIndex)
    {
        GameObject parent = boids[boidTree[_childIndex].parent];

        Vector3 right = parent.transform.position + (parent.transform.right * dist);

        return right;
    }
    public Vector3 parentLocationLeft(int _childIndex)
    {
        GameObject parent = boids[boidTree[_childIndex].parent];

        Vector3 left = parent.transform.position + (-parent.transform.right * dist);
        
        return left;
    }


}
