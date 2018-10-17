using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableFormation : MonoBehaviour {

    public float characterRadius;
    public float numSlots;
    public float radius;
	

    public Vector3 getSlotLoc(int slotNum)
    {
        float angle = ((slotNum / numSlots) * Mathf.PI * 2) - (Mathf.PI / 2.0f);
        radius = characterRadius / Mathf.Sin(Mathf.PI / numSlots);

        Vector3 location = new Vector3();
        location.x = radius * Mathf.Cos(angle) ;
        location.y = radius * Mathf.Sin(angle) ;

        return location;
    }
}
