using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static int[] GetRandomNumbers(int min, int maxExclusive, int qty)
    {
        if (maxExclusive - min > qty)
        {
            Debug.LogError("Woah! Don't do that");
        }
        
        int[] numbers = new int[qty];
        
        for (int i = 0; i < qty; i++)
        {
            int failsafe = qty;
            int number = Random.Range(min, maxExclusive);
            while (numbers.Contains(number) && failsafe >= 0)
            {
                number = Random.Range(min, maxExclusive);
                failsafe--;
            }

            numbers[i] = number;
        }

        return numbers;
    }
}
