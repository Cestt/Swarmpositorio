using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;




public class Grid : MonoBehaviour {
		
		
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
		
		int gridSizeX, gridSizeY;
		
		
		void Awake(){
			//CreateGrid();
		}
		public Grid(Node[,] _grid, LayerMask _unwalkableMask, Vector2 _gridWorldSize, float _nodeSize){
			
			grid = _grid;
			unwalkableMask = _unwalkableMask;
			gridWorldSize = _gridWorldSize;
			nodeSize = _nodeSize;
		}
		
		public int maxHeapSize{
			get{
				return gridSizeX * gridSizeY;
			}
		}
		
		public void CreateGrid(){
			Stopwatch sw = new Stopwatch();
			sw.Start();
			nodeSize = nodeTemp;
			gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeSize);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeSize);
			grid = new Node[gridSizeX,gridSizeY];
			Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;
			
			for(int i = 0; i < gridSizeX; i++){
				
				for(int j = 0; j < gridSizeY; j++){
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeSize + (nodeSize/2)) + Vector3.up * (j * nodeSize + (nodeSize/2));
					bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeSize/2,unwalkableMask));
					grid[i,j] = new Node(walkable,worldPoint,i,j);
					
					
				}
				
			}
			sw.Stop();
			print("Created grid in: " +sw.ElapsedMilliseconds+" ms");
			
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
			
			if(grid != null & displayGizmos){
				
				foreach(Node n in grid){
					Gizmos.color = (n.walkable)?Color.white:Color.red;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeSize-.1f));
				}
			}
		}
		
		

	}

