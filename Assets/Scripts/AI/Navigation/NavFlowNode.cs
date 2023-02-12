using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NavFlowNode : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public int score;
    public bool endAutoNeighbourGeneration = false;
    
    public List<NavFlowNode> manuallySetNeighbours = new List<NavFlowNode>();
    private List<NavFlowNode> neighbours = new List<NavFlowNode>();
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false; // hide when start

        foreach (NavFlowNode node in manuallySetNeighbours)
        {
            neighbours.Add(node);
        }
    }

    public void AddNeighbour(NavFlowNode node)
    {
        neighbours.Add(node);
    }

    public void ResetScore()
    {
        score = int.MinValue;
    }
    
    public void MarkAsBestNode()
    {
        TrickleDownScore(100, this);
    }

    void TrickleDownScore(int newScore, NavFlowNode previousNode)
    {
        if (score != int.MinValue) // already reached from a different path!
        {
            if (newScore <= score) // done
                return;
        }

        score = newScore;

        for (int i = 0; i < neighbours.Count; ++i)
        {
            if (neighbours[i] == previousNode) // avoid going right back as connections go both ways
                continue;
            
            neighbours[i].TrickleDownScore(score - 10, this);
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 nodePos = transform.position;
        foreach (NavFlowNode neighbour in neighbours)
        {
            Gizmos.DrawLine(nodePos, neighbour.transform.position);
        }
        
        Gizmos.color = Color.Lerp(Color.red, Color.yellow, score/100f);
        Gizmos.DrawSphere(nodePos, 3f);
    }
}
