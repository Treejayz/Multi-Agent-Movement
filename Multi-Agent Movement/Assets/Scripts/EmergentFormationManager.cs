using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergentFormationManager : MonoBehaviour {

    public List<GameObject> boids;
    public GameObject leader;
    public GameObject tunnelPoint;
    public GameObject pastThinTunnel;
    public GameObject thickTunnelPoint;
    public GameObject pastThickTunnel;
    public GameObject evilBoid;
    private float maxSpeed;
    private emergentFormation formation;
    private bool thinTunnel = false;
    private bool thickTunnel = false;
    private int tunnelIndex = 0;
    private int tunnelIndex2 = 0;
    private bool thru = false;
	// Use this for initialization
	void Start ()
    {
        formation = this.GetComponent<emergentFormation>();
        formation.setUp(boids);
        maxSpeed = leader.GetComponent<CharacterManager>().maxSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float maxDist = 0;
		for(int i = 1; i < boids.Count; i++)
        {
            if(i % 2 == 0)
            {
                boids[i].GetComponent<CharacterManager>().target = formation.parentLocationRight(i);
            }
            else
            {
                boids[i].GetComponent<CharacterManager>().target = formation.parentLocationLeft(i);
            }
            
            if ((leader.transform.position - boids[i].transform.position).magnitude >= maxDist)
            {
                maxDist = (leader.transform.position - boids[i].transform.position).magnitude;
            }
        }

        if(maxDist >= 4.5f && !thinTunnel && !thickTunnel)
        {
            leader.GetComponent<Kinematics>().maxSpeed *= .2f;
        }
        else
        {
            leader.GetComponent<Kinematics>().maxSpeed = maxSpeed;
        }

        if(leader.GetComponent<CharacterManager>().target == tunnelPoint.transform.position && !thinTunnel)
        {
            float far = 0;
            leader.GetComponent<CharacterManager>().pause = true;
            for (int i = 1; i < boids.Count; i++)
            {
                if((boids[i].transform.position - leader.transform.position).magnitude > far)
                {
                    far = (boids[i].transform.position - leader.transform.position).magnitude;
                }
            }

            if(far <= 1.2f)
            {
                thinTunnel = true;
                for (int i = 0; i < boids.Count; i++)
                {
                    boids[i].GetComponent<CharacterManager>().pause = true;
                    boids[i].GetComponent<Kinematics>().target = tunnelPoint.transform.position;
                    leader.GetComponent<CharacterManager>().pause = false;
                }
            }
            

        }
        if (thinTunnel)
        {
            boids[tunnelIndex].GetComponent<CharacterManager>().pause = false;
            
            if((boids[tunnelIndex].transform.position - tunnelPoint.transform.position).magnitude <= .3f)
            {
                boids[tunnelIndex].GetComponent<Kinematics>().target = boids[tunnelIndex].GetComponent<CharacterManager>().target;
                tunnelIndex++;                
            }

            if (tunnelIndex == boids.Count)
            {
               
                thinTunnel = false;
                leader.GetComponent<CharacterManager>().pause = false;
                tunnelIndex = 0;
            }
        }
        if(leader.GetComponent<CharacterManager>().target == thickTunnelPoint.transform.position  && !thickTunnel)
        {
            float far = 0;
            leader.GetComponent<CharacterManager>().pause = true;
            for (int i = 1; i < boids.Count; i++)
            {
                if ((boids[i].transform.position - leader.transform.position).magnitude > far)
                {
                    far = (boids[i].transform.position - leader.transform.position).magnitude;
                }
            }

            if (far <= 1.2f)
            {
                thickTunnel = true;
                tunnelIndex = -1;
                for (int i = 0; i < boids.Count; i++)
                {
                    boids[i].GetComponent<CharacterManager>().pause = true;
                    boids[i].GetComponent<Kinematics>().target = thickTunnelPoint.transform.position;
                    //leader.GetComponent<CharacterManager>().pause = false;
                }
            }
        }

        if (thickTunnel)
        {
            if(boids.Count % 3 == 0)
            {
                //Debug.Log(tunnelIndex2);
                if(tunnelIndex != tunnelIndex2)
                {
                    boids[tunnelIndex2].GetComponent<CharacterManager>().pause = false;
                    boids[tunnelIndex2 + 1].GetComponent<CharacterManager>().pause = false;
                    boids[tunnelIndex2 + 2].GetComponent<CharacterManager>().pause = false;
                    tunnelIndex = tunnelIndex2;
                }
                
                if ((boids[tunnelIndex2].transform.position - thickTunnelPoint.transform.position).magnitude  <= .5f )
                {
                    boids[tunnelIndex2].GetComponent<CharacterManager>().pause = true;
                    
                }
                if ((boids[tunnelIndex2 + 1].transform.position - thickTunnelPoint.transform.position).magnitude <= .5f)
                {
                    boids[tunnelIndex2 + 1].GetComponent<CharacterManager>().pause = true;
                }
                if ((boids[tunnelIndex2 + 2].transform.position - thickTunnelPoint.transform.position).magnitude <= .5f)
                {
                    boids[tunnelIndex2 + 2].GetComponent<CharacterManager>().pause = true;
                }
                if(boids[tunnelIndex2 + 2].GetComponent<CharacterManager>().pause && boids[tunnelIndex2 + 1].GetComponent<CharacterManager>().pause && boids[tunnelIndex2].GetComponent<CharacterManager>().pause )
                {
                    boids[tunnelIndex2].GetComponent<Kinematics>().target = boids[tunnelIndex].GetComponent<CharacterManager>().target;
                    boids[tunnelIndex2 + 1].GetComponent<Kinematics>().target = boids[tunnelIndex + 1].GetComponent<CharacterManager>().target;
                    boids[tunnelIndex2 + 2].GetComponent<Kinematics>().target = boids[tunnelIndex + 2].GetComponent<CharacterManager>().target;
                    boids[tunnelIndex2].GetComponent<CharacterManager>().pause = false;
                    boids[tunnelIndex2 + 1].GetComponent<CharacterManager>().pause = false;
                    boids[tunnelIndex2 + 2].GetComponent<CharacterManager>().pause = false;
                    tunnelIndex2 += 3;
                }

            }
            else if(boids.Count % 2 == 0)
            {
                if (tunnelIndex != tunnelIndex2)
                {
                    boids[tunnelIndex2].GetComponent<CharacterManager>().pause = false;
                    boids[tunnelIndex2 + 1].GetComponent<CharacterManager>().pause = false;
                    tunnelIndex = tunnelIndex2;
                }

                if ((boids[tunnelIndex2].transform.position - thickTunnelPoint.transform.position).magnitude <= .5f)
                {
                    boids[tunnelIndex2].GetComponent<CharacterManager>().pause = true;

                }
                if ((boids[tunnelIndex2 + 1].transform.position - thickTunnelPoint.transform.position).magnitude <= .5f)
                {
                    boids[tunnelIndex2 + 1].GetComponent<CharacterManager>().pause = true;
                }

                if (boids[tunnelIndex2 + 1].GetComponent<CharacterManager>().pause && boids[tunnelIndex2].GetComponent<CharacterManager>().pause)
                {
                    boids[tunnelIndex2].GetComponent<Kinematics>().target = boids[tunnelIndex].GetComponent<CharacterManager>().target;
                    boids[tunnelIndex2 + 1].GetComponent<Kinematics>().target = boids[tunnelIndex + 1].GetComponent<CharacterManager>().target;
                    boids[tunnelIndex2].GetComponent<CharacterManager>().pause = false;
                    boids[tunnelIndex2 + 1].GetComponent<CharacterManager>().pause = false;
                    tunnelIndex2 += 2;
                }
            }
            else
            {
                boids[tunnelIndex].GetComponent<CharacterManager>().pause = false;
                if ((boids[tunnelIndex].transform.position - thickTunnelPoint.transform.position).magnitude <= .3f)
                {
                    boids[tunnelIndex].GetComponent<Kinematics>().target = boids[tunnelIndex].GetComponent<CharacterManager>().target;
                    tunnelIndex++;
                }

            }

            if(tunnelIndex2 == boids.Count)
            {
                
                thickTunnel = false;
                //leader.GetComponent<CharacterManager>().pause = false;
            }
        }
        if(leader.GetComponent<CharacterManager>().target == pastThinTunnel.transform.position && (thinTunnel))
        {
            leader.GetComponent<CharacterManager>().pause = true;
        }

        foreach(GameObject boid in boids)
        {
            if((boid.transform.position - evilBoid.transform.position).magnitude <= .1f)
            {
                killBoid(boid);
                break;
            }
        }

    }

    private void killBoid(GameObject _boid)
    {
        if(boids.IndexOf(_boid) == 0)
        {
            boids[1].GetComponent<CharacterManager>().leader = true;
        }
        boids.Remove(_boid);

        formation.setUp(boids);
        Destroy(_boid);
    }
}
