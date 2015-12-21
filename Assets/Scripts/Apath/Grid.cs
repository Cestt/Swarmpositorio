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
	private List<Node[]> Grids = new List<Node[]>();

		
		int gridSizeX, gridSizeY;
		
		
		void Awake(){
			position = new EVector3(transform.position);
			CreateGrid();
		EVector3 hola = (EVector2)position + position2;
		}
		
		
		public int maxHeapSize{
			get{
				return gridSizeX * gridSizeY;
			}
		}
		
		public void StartCreateGrid(){
			StartCoroutine(CreateGrid());
		}	

		IEnumerator CreateGrid(){
		int Xiterations = 0;
		int Yiterations = 0;

		if(gridWorldSize.x > maxGridWorldSizeX){
			Xiterations = (int)(gridWorldSize.x/maxGridWorldSizeX);
		}else{
			maxGridWorldSizeX = (int)gridWorldSize.x;
			Xiterations = 1;
		}
		if(gridWorldSize.y > maxGridWorldSizeY){
			Yiterations = (int)(gridWorldSize.y/maxGridWorldSizeY);
		}else{
			maxGridWorldSizeY = (int)gridWorldSize.y;
			Yiterations = 1;
		}
			
		Stopwatch sw = new Stopwatch();
		sw.Start();
		nodeSize = nodeTemp;
		gridSizeX = Mathf.RoundToInt(maxGridWorldSizeX/nodeSize);
		gridSizeY = Mathf.RoundToInt(maxGridWorldSizeY/nodeSize);
		grid = new Node[gridSizeX * Xiterations,gridSizeY * Yiterations];

		for(int i = 1; i < Xiterations + 1;i++){
			for(int j = 1; j < Yiterations + 1;j++){
				Vector3 worldBottomLeft =(transform.position - new Vector3(gridWorldSize.x/2,gridWorldSize.y/2,0)) +  new Vector3((maxGridWorldSizeX/2)*i,(maxGridWorldSizeY/2)*j,0) - Vector3.right * (maxGridWorldSizeX/2) - Vector3.up * (maxGridWorldSizeY/2);
				print(worldBottomLeft);
				for(int k = gridSizeX * (i-1); k < gridSizeX * i; k++){
					for(int l = gridSizeY * (j-1); l < gridSizeY * j; l++){
						Vector3 worldPoint = worldBottomLeft + Vector3.right * (k * nodeSize + (nodeSize/2)) + Vector3.up * (l * nodeSize + (nodeSize/2));
						bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeSize/2,unwalkableMask));
						grid[k,l] = new Node(walkable,worldPoint,k,l,i);
					}
					
				}
			}
		}


	
			
			
			sw.Stop();
			print("Created grid in: " +sw.ElapsedMilliseconds+" ms");
		print("Grid length "+grid.Length);
			yield return null;
		}
		
		public Node NodeFromWorldPosition(Vector3 worldPosition){
			
			float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
			float percetnY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
			percentX = Mathf.Clamp01(percentX);
			percetnY = Mathf.Clamp01(percetnY);
			
			int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
			int y = Mathf.RoundToInt((gridSizeY - 1) * percetnY);
			
			return grid[x,y];
		}
		
		public List<Node> GetNeighbours(Node node){
			
			List<Node> neighbours = new List<Node>();
			
			for(int x = -1; x <= 1 ; x++){
				for(int y = -1; y <= 1 ; y++){
					
					if( x == 0 & y == 0)
						continue;
					
					int checkX = node.gridX + x;
					int checkY = node.gridY + y;
					
					if(checkX >= 0 & checkX < gridSizeX & checkY >= 0 & checkY < gridSizeY){
						neighbours.Add(grid[checkX,checkY]);
					}
				}
			}
			return neighbours;
		}
		
	void OnDrawGizmos(){
		
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y,1));
		//Comentado por la enorme cantidad de nodos actual.

		if(grid != null & displayGizmos){
			
			foreach(Node n in grid){
					Gizmos.color = (n.walkable)?Color.white:Color.red;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeSize-.1f));
			}
		}
	}
		
		

	}

