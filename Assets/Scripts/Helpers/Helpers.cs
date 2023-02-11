using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static bool HasLineOfSight(Vector3 origin, Vector3 direction, string targetTag)
    {
        bool hasHit = Physics.Raycast(origin,
            direction, out RaycastHit hit, Mathf.Infinity);
        
        
        //Debug.DrawRay(origin, direction * 1000, Color.yellow);
        
        return hasHit && hit.transform.CompareTag(targetTag);
    }
}
