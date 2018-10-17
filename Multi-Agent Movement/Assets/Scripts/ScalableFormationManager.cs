using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableFormationManager : MonoBehaviour {

    public GameObject leader;
    
    public CharacterManager[] followers;
    private ScalableFormation formation;
    public float radius;
    public Transform startLoc;
    public GameObject tunnel;
    public GameObject pastTunnel;
    public GameObject wideTunnel;
    public GameObject pastWideTunnel;
    private bool once = false;
    private float maxSpeed;
    private int tunnelIndex = 0;
    private bool allArrivedFirst = true;
    private bool allArrivedSecond = true;
	// Use this for initialization
	void Start ()
    {
        formation = this.GetComponent<ScalableFormation>();
        formation.numSlots = followers.Length + 1;
        formation.characterRadius = radius;

        

        for (int i = 0; i < followers.Length; i++)
        {
            followers[i].setUp(formation.getSlotLoc(i + 1) + startLoc.position);
            followers[i].GetComponent<Character>().staticInfo.position = formation.getSlotLoc(i + 1) + startLoc.position ;
            followers[i].gameObject.transform.position = formation.getSlotLoc(i + 1) + startLoc.position;
            followers[i].target = formation.getSlotLoc(i + 1) + startLoc.position;

        }
        leader.transform.position = formation.getSlotLoc(0) + startLoc.position;
        leader.GetComponent<Character>().staticInfo.position = leader.transform.position + startLoc.position;
        maxSpeed = leader.GetComponent<Kinematics>().maxSpeed;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (leader.GetComponent<CharacterManager>().target == tunnel.transform.position)
        {
            allArrivedFirst = false;
            foreach(CharacterManager boid in followers)
            {
                boid.pause = true;
            }
        }
        else if (!allArrivedFirst)
        {
            
            if(tunnelIndex != followers.Length)
            {
                followers[tunnelIndex].pause = false;

                followers[tunnelIndex].kinematics.target = tunnel.transform.position;
                if((followers[tunnelIndex].transform.position - tunnel.transform.position).magnitude <= .5f)
                {
                    tunnelIndex += 1;
                }
                

            }
            else { allArrivedFirst = true; tunnelIndex = 0; }
            
        }
        else if(leader.GetComponent<CharacterManager>().target == wideTunnel.transform.position)
        {
            allArrivedSecond = false;
            foreach (CharacterManager boid in followers)
            {
                boid.pause = true;
            }
            if (followers.Length + 1 % 3 == 0)
            {
                followers[0].kinematics.target = wideTunnel.transform.position;
                followers[1].kinematics.target = wideTunnel.transform.position;
                followers[0].pause = false;
                followers[1].pause = false;
                //tunnelIndex = 2;
            }
            else
            {
                followers[0].kinematics.target = wideTunnel.transform.position;
                followers[0].pause = false;
               // tunnelIndex = 1;
            }
        }
        else if (!allArrivedSecond)
        {
            if (tunnelIndex < followers.Length)
            {
                if((followers[tunnelIndex].transform.position - wideTunnel.transform.position).magnitude <= 1f)
                {
                    if(followers.Length + 1 % 3 == 0)
                    {
                        if(tunnelIndex == 0)
                        {
                            tunnelIndex += 2;
                            if(tunnelIndex < followers.Length)
                            {
                                followers[tunnelIndex].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex + 1].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex + 2].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex].pause = false;
                                followers[tunnelIndex + 1].pause = false;
                                followers[tunnelIndex + 2].pause = false;
                            }
                            
                        }
                        else
                        {
                            tunnelIndex += 3;
                            if(tunnelIndex < followers.Length)
                            {
                                followers[tunnelIndex].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex + 1].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex + 2].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex].pause = false;
                                followers[tunnelIndex + 1].pause = false;
                                followers[tunnelIndex + 2].pause = false;
                            }
                            
                        }
                    }
                    else
                    {
                        if (tunnelIndex == 0)
                        {
                            tunnelIndex += 1;
                            if(tunnelIndex < followers.Length)
                            {
                                followers[tunnelIndex].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex + 1].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex].pause = false;
                                followers[tunnelIndex + 1].pause = false;
                            }
                            
                        }
                        else
                        {
                            tunnelIndex += 2;
                            if(tunnelIndex < followers.Length)
                            {
                                followers[tunnelIndex].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex + 1].kinematics.target = wideTunnel.transform.position;
                                followers[tunnelIndex].pause = false;
                                followers[tunnelIndex + 1].pause = false;
                            }
                            
                        }
                    }
                }
            }
            else
            {
                allArrivedSecond = true;
            }
        }
        
        float maxDist = 0;
        for (int i = 0; i < followers.Length; i++)
        {
            followers[i].target = formation.getSlotLoc(i + 1) + startLoc.position;
            followers[i].transform.rotation = Quaternion.Euler(0, 0, leader.GetComponent<Character>().staticInfo.orientation * Mathf.Rad2Deg);

            if ((leader.transform.position - followers[i].transform.position).magnitude > maxDist )
            {
                maxDist = (leader.transform.position - followers[i].transform.position).magnitude;
            }

        }

            

        if (maxDist > formation.radius * 2.1f && allArrivedFirst && allArrivedSecond)
        {
            leader.GetComponent<Kinematics>().maxSpeed *= .2f;
        }
        else
        {
            leader.GetComponent<Kinematics>().maxSpeed = maxSpeed;
        }
        

        if (leader.GetComponent<CharacterManager>().target == pastTunnel.transform.position && !allArrivedFirst)
        {
            leader.GetComponent<CharacterManager>().pause = true;
        }
        else { leader.GetComponent<CharacterManager>().pause = false; }
        if (leader.GetComponent<CharacterManager>().target == pastWideTunnel.transform.position && !allArrivedSecond)
        {
            leader.GetComponent<CharacterManager>().pause = true;
        }
        else { leader.GetComponent<CharacterManager>().pause = false; }

        if (leader.GetComponent<CharacterManager>().collisionDetected)
        {
            
                if (roomForFormation() && !once)
                {
                    leader.GetComponent<CharacterManager>().kinematics.target *= formation.radius;
                    leader.GetComponent<CharacterManager>().collisionDetected = false;
                    once = true;
                }
                
        }

        
        
    }

    private bool roomForFormation()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(startLoc.position, formation.radius);
        if (hits.Length != 0)
        {
            return false;
        }
        return true;
    }

}
