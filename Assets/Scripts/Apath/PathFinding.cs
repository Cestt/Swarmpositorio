using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


public class PathFinding : MonoBehaviour {

	Grid grid;
	PathRequestManager pathManager;
	Queue<ApathQueue> queueFindPaths = new Queue<ApathQueue>();
	bool running = false;
	HeatMapManager heatmanager;

	void Awake(){
		heatmanager = GetComponent<HeatMapManager>();
		grid = GetComponentInParent<Grid>();
		pathManager = GetComponent<PathRequestManager>();
	}

	public void StartFindPathHeat(Vector3 startPosition, Node targetNode,Node[] _path ,Action<Vector3,Node[]> callBack,int index){
		StartCoroutine(FindPathHeat(startPosition,targetNode,_path,callBack,index));
	}
	public void StartFindPath(Vector3 startPosition, Vector3 targetPosition,Action<Vector3[]> callBack){
		StartCoroutine(FindPath(startPosition,targetPosition,callBack));
	}

	IEnumerator FindPathHeat(Vector3 startPosition, Node targetNode,Node[] _path ,Action<Vector3,Node[]> callBack ,int index){
		running = true;
		heatmanager.StopIterateDictionary();
		yield return new WaitForEndOfFrame();
		Stopwatch sw = new Stopwatch();
		sw.Start();
		bool pathSuccess = false;
		Node startNode = grid.NodeFromWorldPosition(startPosition);
		if(startNode != targetNode){
			
			if(startNode.walkable & targetNode.walkable){
				Heap<Node> openSet = new Heap<Node>(grid.maxHeapSize);
				HashSet<Node> closedSet = new HashSet<Node>();

				openSet.Add(startNode);

				while(openSet.Count >0){
					Node currentNode = openSet.RemoveFirst();


					closedSet.Add(currentNode);

					if(currentNode == targetNode){
						print("Path succes");
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
		}
			

		if(pathSuccess){
			Node[] waypoints = RetracePathHeat(startNode,targetNode,_path);
			GenerateHeatMap(waypoints,index);
			heatmanager.StartIterateDictionary(index);
			callBack(waypoints[waypoints.Length-1].worldPosition,waypoints);
		}

		sw.Stop();
		running = false;

		print("Found path in "+sw.ElapsedMilliseconds+" ms");

	}

	IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition,Action<Vector3[]> callBack ){
		running = true;
		yield return new WaitForEndOfFrame();
		Stopwatch sw = new Stopwatch();
		sw.Start();
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;
		Node startNode = grid.NodeFromWorldPosition(startPosition);
		Node targetNode = grid.NodeFromWorldPosition(targetPosition);

		if(startNode != targetNode){
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
		}



		if(pathSuccess){
			waypoints = RetracePath(startNode,targetNode);
		}
		callBack(waypoints);
		sw.Stop();
		running = false;
		print("Found path in "+sw.ElapsedMilliseconds+" ms");


	}

	Node[] RetracePathHeat(Node startNode, Node endNode,Node[] _path){
		if(_path != null)
			print("AWDWADA "+grid.index);
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		while(currentNode != startNode){
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		if(_path != null){
			Node[] temp = new Node[path.Count + _path.Length];
			_path.CopyTo(temp,0);
			path.CopyTo(temp,_path.Length);
			return temp;
		}else{
			
			return path.ToArray();
		}

		//Vector3[] wayPoints = simplifyPath(path);
		//Array.Reverse(path.ToArray());


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

	void GenerateHeatMap(Node[] nodes,int index){
		for(int i = 0; i < nodes.Length - 1;i++){
			grid.SetNeighboursHeatMap(nodes[i],index);
		}
		heatmanager.heatMaps[grid.index] = new List<Node>(grid.heatNodes);
		grid.heatNodes.Clear();
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
