using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

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

    public NavFlowNode FindBestNeighbour()
    {
        int bestScore = int.MinValue;
        NavFlowNode bestNeighbour = null;
        
        for (int i = 0; i < neighbours.Count; ++i)
        {
            if (neighbours[i].score > bestScore)
            {
                bestScore = neighbours[i].score;
                bestNeighbour = neighbours[i];
            }
        }

        return bestNeighbour;
    }

    public bool IsPositionInNode(Vector3 position)
    {
        Vector3 nodePos = transform.position;
        Vector3 nodeScale = transform.localScale;
        Vector3 toNode = nodePos - position;
        bool inNode = Mathf.Abs(toNode.x) <= nodeScale.x/2f && Mathf.Abs(toNode.y) <= nodeScale.y;
        return inNode;
    }

    public Vector3 GetRandomPositionInNode()
    {
        Vector3 nodePos = transform.position;
        Vector3 nodeScale = transform.localScale;
        float randomX = Random.Range(0, nodeScale.x);
        float randomY = Random.Range(0, nodeScale.y);
        Vector3 wantedPos = nodePos + new Vector3(randomX - nodeScale.x/2f, randomY - nodeScale.y/2f);
        return wantedPos;
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
