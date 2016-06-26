using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{

    // public Transform seeker, target;                         //ne koristimo vishe zbog korishcenja pathRequestManager-a

    PathRequestManager requestManager;

    Grid grid;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    /*void Update()                                                 //aktiviranje pracenja botova na space
    {
        if (Input.GetButtonDown("Jump"))
        {
            FindPath(seeker.position, target.position);
        }
    }*/

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)  //dodato uz pathRequestManager
    {
            StartCoroutine(FindPath(startPos, targetPos));
        
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)  
    {
        Stopwatch sw = new Stopwatch();                         //shtoperica radi testiranja optimizacije
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;


        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

     
            if (startNode.walkable && targetNode.walkable)
            {                                                    // samo ako su pochetna i krajnja pozicija na prohodnom mestu krece pronalazenje puta

                //List<Node> openSet = new List<Node>();            //lista zamenjena Heap-om

                Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();  // implementiran Heap

                    /*Node currentNode = openSet[0];

                    for (int i = 1; i < openSet.Count; i++)
                    {
                        if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                        {
                            currentNode = openSet[i];
                        }
                    }
                    openSet.Remove(currentNode);*/

                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        sw.Stop();
                        print("Path found: " + sw.ElapsedMilliseconds + " ms");
                        pathSuccess = true;
                        //RetracePath(startNode, targetNode);                           //ubachen u if petlju kasnije
                        break;
                    }

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }
                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        
        yield return null; //cheka 1 frame pre vracanja
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode); 
        }
        requestManager.FinishedProcessingPath(waypoints,pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)     //promenjeno iz voida u V3 niz
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
         
        //grid.path = path;

    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i=1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
                directionOld = directionNew;
            
        }
        return waypoints.ToArray();
    }


    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

}