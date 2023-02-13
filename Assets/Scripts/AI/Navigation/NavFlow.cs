using System;
using System.Collections.Generic;
using UnityEngine;

public class NavFlow : Singleton<NavFlow>
{
    NavFlowNode[] nodes;

    private Transform player;
    private float refreshTimer;
    
    // Start is called before the first frame update
    void Start() {
        

        nodes = GetComponentsInChildren<NavFlowNode>();

        for (int i = 0; i < nodes.Length-1; ++i)
        {
            if (nodes[i].endAutoNeighbourGeneration)
                break;
            
            nodes[i].AddNeighbour(nodes[i+1]);
            nodes[i+1].AddNeighbour(nodes[i]);
        }
    }

    private void OnEnable() {
        PlayerData.Instance.playerVehicleChanged.AddListener(PlayerVehicleChanged);
    }

    private void OnDisable() {
        PlayerData.Instance.playerVehicleChanged.RemoveListener(PlayerVehicleChanged);
    }

    private void PlayerVehicleChanged(GameObject newPlayerVehicle) {
        player = newPlayerVehicle.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentState != GameState.Started) {
            return;
        }
        if (refreshTimer > 0) // for perf, no need to do this every frame
        {
            refreshTimer -= Time.deltaTime;
            if (refreshTimer > 0)
                return;

            refreshTimer = 0.4f;
        }
        
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

    public Vector3 FindBestPosition(Vector3 querierPos)
    {
        NavFlowNode closestToQuerier = null;
        float closestSqrDist = float.MaxValue;
        
        // could optimize with spatial partition
        foreach (NavFlowNode node in nodes)
        {
            Vector3 candidateNodePos = node.transform.position;
            float sqrDist = (querierPos - candidateNodePos).sqrMagnitude;

            if (sqrDist < closestSqrDist)
            {
                closestSqrDist = sqrDist;
                closestToQuerier = node;
            }
        }

        NavFlowNode wantedNode = closestToQuerier;
        if (closestToQuerier.IsPositionInNode(querierPos))
        {
            wantedNode = closestToQuerier.FindBestNeighbour();
        }

        Vector3 wantedPos = wantedNode.GetRandomPositionInNode();
        return wantedPos;
    }
    
    public void UpdateListOfZombies(List<ZombieBoidInfo> zombies)
    {
        foreach (ZombieBoidInfo info in zombies)
        {
            info.destination = FindBestPosition(info.position);
        }
    }
    
    public Vector3 FindBestPosition(Vector3 querierPos, Vector3 currentDestination)
    {
        // early out, maintain previous destination
        Vector3 querierToDestination = currentDestination - querierPos;
        if (querierToDestination.sqrMagnitude > 0.5f)
            return currentDestination;
        
        NavFlowNode closestToQuerier = null;
        float closestSqrDist = float.MaxValue;
        
        // could optimize with spatial partition
        foreach (NavFlowNode node in nodes)
        {
            Vector3 candidateNodePos = node.transform.position;
            float sqrDist = (querierPos - candidateNodePos).sqrMagnitude;

            if (sqrDist < closestSqrDist)
            {
                closestSqrDist = sqrDist;
                closestToQuerier = node;
            }
        }

        NavFlowNode wantedNode = closestToQuerier;
        if (closestToQuerier.IsPositionInNode(querierPos))
        {
            wantedNode = closestToQuerier.FindBestNeighbour();
        }

        Vector3 wantedPos = wantedNode.GetRandomPositionInNode();
        return wantedPos;
    }
    
    public void UpdateListOfZombies2(List<ZombieBoidInfo> zombies)
    {
        foreach (ZombieBoidInfo info in zombies)
        {
            info.destination = FindBestPosition(info.position, info.destination);
        }
    }
}
