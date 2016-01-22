using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using CielaSpike;

public class PathFinding : MonoBehaviour {

	Grid grid;
	PathRequestManager pathManager;
	Queue<ApathQueue> queueFindPaths = new Queue<ApathQueue>();
	bool running = false;
	Task task = null;

	void Awake(){
		grid = GetComponentInParent<Grid>();
		pathManager = GetComponent<PathRequestManager>();
	}
	/// <summary>
	/// Lanza la busqueda del path
	/// </summary>
	/// <param name="startPosition">Start position.</param>
	/// <param name="targetPosition">Target position.</param>
	/// <param name="callBack">Call back.</param>
	public void StartFindPath(Vector3 startPosition, Vector3 targetPosition,Action<Vector3[]> callBack){
		queueFindPaths.Enqueue(new ApathQueue(startPosition,targetPosition,callBack));
		StartCoroutine(CheckQueue());

	}
	/// <summary>
	/// Obtienes el path entre dos posiciones
	/// </summary>
	/// <returns>El path al callback</returns>
	/// <param name="startPosition">Posicion inicial</param>
	/// <param name="targetPosition">Posiion final</param>
	/// <param name="callBack">Call back.</param>
	IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition,Action<Vector3[]> callBack ){
		running = true;
		yield return new WaitForEndOfFrame();
		yield return Ninja.JumpToUnity;
		Stopwatch sw = new Stopwatch();
		sw.Start();
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;
		Node startNode = grid.NodeFromWorldPosition(startPosition);
		Node targetNode = grid.NodeFromWorldPosition(targetPosition);
		yield return Ninja.JumpBack;
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
				
			
		yield return Ninja.JumpToUnity;
		if(pathSuccess){
			waypoints = RetracePath(startNode,targetNode);
		}
		callBack(waypoints);
		sw.Stop();
		running = false;
		StartCoroutine(CheckQueue());

		print("Found path in "+sw.ElapsedMilliseconds+" ms");


	}
	/// <summary>
	/// Da la vuelta al path para ordenarlo de inicio a final
	/// </summary>
	/// <returns>Path ordenado</returns>
	/// <param name="startNode">Start node.</param>
	/// <param name="endNode">End node.</param>
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
	/// <summary>
	/// Simplifica el path.
	/// </summary>
	/// <returns>Devuelve el path simplificado</returns>
	/// <param name="path">Recive el path complejo</param>
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
	/// <summary>
	/// Obtiene la distancia especificada entre dos Nodos
	/// </summary>
	/// <param name="nodeA">Node a.</param>
	/// <param name="nodeB">Node b.</param>
	int Getdistance(Node nodeA, Node nodeB){
		int disX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int disY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if(disX > disY)
			return 14*disY + 10*(disX - disY);
		return 14*disX + 10*(disY - disX);
	}
	/// <summary>
	/// Comprueba si hay otros hilos ejecutandose y espera a que termine
	/// </summary>
	IEnumerator CheckQueue(){
		if(queueFindPaths.Count > 0){
			
			if(task != null){
				if(task.State != TaskState.Running){
					ApathQueue temp = queueFindPaths.Dequeue();
					this.StartCoroutineAsync(FindPath(temp.startPosition,temp.endPosition,temp.callback),out task);
				}
			}else{
				ApathQueue temp = queueFindPaths.Dequeue();
				this.StartCoroutineAsync(FindPath(temp.startPosition,temp.endPosition,temp.callback),out task);
			}
		}
		yield return null;
	}

}
