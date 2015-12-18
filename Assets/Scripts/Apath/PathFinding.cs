using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class PathFinding : MonoBehaviour {

	Grid grid;
	PathRequestManager pathManager;

	void Awake(){
		grid = GetComponentInParent<Grid>();
		pathManager = GetComponent<PathRequestManager>();
	}

	public void StartFindPath(Vector3 startPosition, Vector3 targetPosition,Action<Vector3[]> callBack){
		StartCoroutine(FindPath(startPosition,targetPosition));
	}

	IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition){
		yield return new WaitForEndOfFrame();
		Stopwatch sw = new Stopwatch();
		sw.Start();

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPosition(startPosition);
		Node targetNode = grid.NodeFromWorldPosition(targetPosition);
		
			if(startNode.walkable & targetNode.walkable){
				Heap<Node> openSet = new Heap<Node>(grid.maxHeapSize);
				HashSet<Node> closedSet = new HashSet<Node>();
				
				openSet.Add(startNode);
				
				while(openSet.Count >0){
					Node currentNode = openSet.RemoveFirst();
					
					
					closedSet.Add(currentNode);
					
					if(currentNode == targetNode){
						pathSuccess = true;
						break;
					}
					
					foreach(Node neighbour in grid.GetNeighbours(currentNode)){
						if(!neighbour.walkable || closedSet.Contains(neighbour)){
							continue;
						}
						
						int newMovementCostToNeighbour = currentNode.gCost + Getdistance(currentNode,neighbour);
						
						if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Containts(neighbour)){
							neighbour.gCost = newMovementCostToNeighbour;
							neighbour.hCost = Getdistance(neighbour,targetNode);
							neighbour.parent =currentNode;
							
							if(!openSet.Containts(neighbour)){
								openSet.Add(neighbour);
							}else{
								openSet.UpdateItem(neighbour);
							}
							
						}
					}
				}
			}

		yield return null;
			if(pathSuccess){
				waypoints = RetracePath(startNode,targetNode);
			}
			pathManager.FinishedProcessingPath(waypoints,pathSuccess);
		sw.Stop();
		print("Found path in "+ sw.ElapsedMilliseconds + " ms" );
	}

	Vector3[] RetracePath(Node startNode, Node endNode){

		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while(currentNode != startNode){
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] wayPoints = simplifyPath(path);
		Array.Reverse(wayPoints);
		return wayPoints;
	}
	Vector3[] simplifyPath(List<Node> path){
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for(int i = 1; i < path.Count; i++){
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
			if(directionNew != directionOld){
				waypoints.Add(path[i-1].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	int Getdistance(Node nodeA, Node nodeB){
		int disX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int disY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if(disX > disY)
			return 14*disY + 10*(disX - disY);
		return 14*disX + 10*(disY - disX);
	}

}
