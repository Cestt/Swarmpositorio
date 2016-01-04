using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Node : IHeapItem<Node>{

	public bool walkable;
	public Vector2 worldPosition;
	public int gCost;
	public int hCost;

	public int gridX;
	public int gridY;

	int heapIndex;

	public Node parent;
	public int grid;

	public Node(bool _walkable, Vector2 _worldPosition, int _gridX, int _gridY,int _grid){

		walkable = _walkable;
		worldPosition = _worldPosition;
		gridX = _gridX;
		gridY = _gridY;
		grid = _grid;
	}
	public int CompareTo(Node nodeToCompare){
		int compare = fcost.CompareTo(nodeToCompare.fcost);
		if(compare == 0){
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}

	public int fcost{
		get{
			return gCost + hCost;
		}
	}
	public int HeapIndex{
		get{
			return heapIndex;
		}
		set{
			heapIndex = value;
		}
	}


 }
