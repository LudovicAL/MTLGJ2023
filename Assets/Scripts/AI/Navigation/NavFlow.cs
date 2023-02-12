using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavFlow : MonoBehaviour
{
    NavFlowNode[] nodes;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        nodes = GetComponentsInChildren<NavFlowNode>();

        for (int i = 0; i < nodes.Length-1; ++i)
        {
            if (nodes[i].endAutoNeighbourGeneration)
                break;
            
            nodes[i].AddNeighbour(nodes[i+1]);
            nodes[i+1].AddNeighbour(nodes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.position;
        NavFlowNode closestToPlayer = null;
        float closestSqrDist = float.MaxValue;
        
        foreach (NavFlowNode node in nodes)
        {
            node.ResetScore();

            Vector3 nodePos = node.transform.position;
            float sqrDist = (playerPos - nodePos).sqrMagnitude;

            if (sqrDist < closestSqrDist)
            {
                closestSqrDist = sqrDist;
                closestToPlayer = node;
            }
        }
        
        closestToPlayer.MarkAsBestNode();
    }

    //void OnDrawGizmos()
    //{
    //    if (nodes.Length == 0)
    //        return;
    //    
    //    Gizmos.color = Color.yellow;
    //    foreach (NavFlowNode node in nodes)
    //    {
    //        Vector3 nodePos = node.transform.position;
    //        foreach (NavFlowNode neighbour in node.neighbours)
    //        {
    //            Gizmos.DrawLine(nodePos, neighbour.transform.position);
    //        }
    //    }
    //}
}
