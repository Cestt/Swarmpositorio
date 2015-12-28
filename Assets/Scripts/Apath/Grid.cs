using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;




public class Grid : MonoBehaviour {
		
	public int maxGridWorldSizeX = 50;
	public int maxGridWorldSizeY =  50;
	public Node[,] grid;
	public LayerMask unwalkableMask;
	[HideInInspector]
	public bool displayGizmos;
	[HideInInspector]
	public Vector2 gridWorldSize;
	[HideInInspector]
	public float nodeSize;
	[HideInInspector]
	public float nodeTemp;
	private EVector3 position = new EVector3(2f,3f,4f);
	private EVector2 position2 = new EVector2(2f,3f);
	private Dictionary<int,Node[]> Grids = new Dictionary<int, Node[]>();
		
		int gridSizeX, gridSizeY;
		
		
		void Awake(){
			position = new EVector3(transform.position);
		StartCreateGrid();
		EVector3 hola = (EVector2)position + position2;
		}
		
		
		public int maxHeapSize{
			get{
				return Mathf.RoundToInt(gridWorldSize.x/nodeSize) * Mathf.RoundToInt(gridWorldSize.y/nodeSize);
			}
		}
		
		public void StartCreateGrid(){
			StartCoroutine(CreateGrid());
		}	

		IEnumerator CreateGrid(){
		Stopwatch sw = new Stopwatch();
		sw.Start();

		int Xiterations = 0;
		int Yiterations = 0;

		if(gridWorldSize.x > maxGridWorldSizeX){
			Xiterations = Mathf.RoundToInt(gridWorldSize.x/maxGridWorldSizeX);
		}else{
			maxGridWorldSizeX = Mathf.RoundToInt(gridWorldSize.x);
			Xiterations = 1;
		}
		if(gridWorldSize.y > maxGridWorldSizeY){
			Yiterations = Mathf.RoundToInt(gridWorldSize.y/maxGridWorldSizeY);
		}else{
			maxGridWorldSizeY = Mathf.RoundToInt(gridWorldSize.y);
			Yiterations = 1;
		}
			
		nodeSize = nodeTemp;
		gridSizeX = Mathf.RoundToInt(maxGridWorldSizeX/nodeSize);
		gridSizeY = Mathf.RoundToInt(maxGridWorldSizeY/nodeSize);
		grid = new Node[gridSizeX * Xiterations,gridSizeY * Yiterations];

		Vector3 worldBottomLeft =(transform.position - new Vector3(gridWorldSize.x/2,gridWorldSize.y/2,0));
		int gridNum = 0;
		for(int i = 1; i < Xiterations + 1;i++){
			for(int j = 1; j < Yiterations + 1;j++){
				List<Node> temp = new List<Node>();
				for(int k = gridSizeX * (i-1); k < (gridSizeX * i); k++){

					for(int l = gridSizeY * (j-1); l < (gridSizeY * j); l++){

						Vector3 worldPoint = worldBottomLeft + Vector3.right * (k * nodeSize + (nodeSize/2)) + Vector3.up * (l * nodeSize + (nodeSize/2));
						bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeSize/2,unwalkableMask));
						grid[k,l] =  new Node(walkable,worldPoint,k,l,gridNum);
						temp.Add(grid[k,l]);
					}

				}
				Grids[gridNum] = temp.ToArray();
				gridNum++;

			}


		}
		sw.Stop();
		print("Created grid in: " +sw.ElapsedMilliseconds+" ms");
		System.GC.Collect();
		System.GC.WaitForPendingFinalizers();
		yield return null;
			
		}

	public void StartRebuildGrid(Vector3 worldPoint){
		StartCoroutine(RebuildGrid(worldPoint));
	}


	IEnumerator RebuildGrid(Vector3 worldPosition){
		Stopwatch sw = new Stopwatch();
		sw.Start();

		Node[] temp = GridFromWorldPosition(worldPosition);

		for(int i = 0;i < temp.Length - 1; i++){
			bool walkable = !(Physics2D.OverlapCircle(temp[i].worldPosition,nodeSize/2,unwalkableMask));
			temp[0].walkable = walkable;
		}

		sw.Stop();
		print("Recreated grid of size "+temp.Length+" in: "+sw.ElapsedMilliseconds+" ms");
		System.GC.Collect();
		System.GC.WaitForPendingFinalizers();
		yield return null;
	}

		
	public Node NodeFromWorldPosition(Vector3 worldPosition){
		
		
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percetnY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percetnY = Mathf.Clamp01(percetnY);
		
		int x = Mathf.RoundToInt(((gridWorldSize.x/nodeSize) - 1) * percentX);
		int y = Mathf.RoundToInt(((gridWorldSize.y/nodeSize) - 1) * percetnY);

		return grid[x,y];
	}

	public Node[] GridFromWorldPosition(Vector3 worldPosition){
		
		
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percetnY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percetnY = Mathf.Clamp01(percetnY);
		
		int x = Mathf.RoundToInt(((gridWorldSize.x/nodeSize) - 1) * percentX);
		int y = Mathf.RoundToInt(((gridWorldSize.y/nodeSize) - 1) * percetnY);
		
		return Grids[grid[x,y].grid];
	}
		
	public List<Node> GetNeighbours(Node node){
			
		List<Node> neighbours = new List<Node>();
		
		for(int x = -1; x <= 1 ; x++){
			for(int y = -1; y <= 1 ; y++){
				
				if( x == 0 & y == 0)
					continue;
				
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				
				if(checkX >= 0 & checkX < (gridWorldSize.x/nodeSize) & checkY >= 0 & checkY < (gridWorldSize.y/nodeSize)){
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}
		return neighbours;
	}


		

	void OnDrawGizmos(){
		
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y,1));
		//Comentado por la enorme cantidad de nodos actual.
		/*
		if(grid != null & displayGizmos){
			
			foreach(Node n in Nodes){
					Gizmos.color = (n.walkable)?Color.white:Color.red;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeSize-.1f));
			}
		}*/
	}
		
		

	}

