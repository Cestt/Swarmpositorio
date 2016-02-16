using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;




public class Grid : MonoBehaviour {

	//Tamaño maximo en X de cada sector que compone el grid;
	public int maxSectorSizeX = 50;
	//Tamaño maximo en Y de cada sector que compone el grid;
	public int maxSectorSizeY =  50;
	//Array de guardado de los nodos que componen el grid;
	public Node[,] grid;
	//Capa de colision para los obstaculos;
	public LayerMask unwalkableMask;
	//Control del preview del grid;(Desactivado)
	[HideInInspector]
	public bool displayGizmos;
	//Tamaño total del grid;(manejado por Custom Inspector);
	[HideInInspector]
	public Vector2 gridWorldSize;
	//Tamaño de los nodos que componen el grid;
	[HideInInspector]
	public float nodeSize;
	//Helper para el tamaño del nodo;
	[HideInInspector]
	public float nodeTemp;
	//Lista que almacena los nodos de cada sector;
	private Dictionary<int,Node[]> Grids = new Dictionary<int, Node[]>();
	//Iteraciones en X e Y de cada sector;	
	int gridSizeX, gridSizeY;
	public int index = 0;
	public List<Node> heatNodes = new List<Node>();

		
	void Awake(){
	// Recrear el grid en tiempo de carga;
		StartCreateGrid();
	}
		
	//Tamaño maximo del arbol de busqueda del pathfinding;
	public int maxHeapSize{
		get{
			return Mathf.RoundToInt(gridWorldSize.x/nodeSize) * Mathf.RoundToInt(gridWorldSize.y/nodeSize);
		}
	}

	/// <summary>
	/// Metodo para acceder a la corutina desde otras clases.
	/// </summary>
	public void StartCreateGrid(){
		StartCoroutine(CreateGrid());
	}	
	/// <summary>
	/// Crea el grid
	/// </summary>
	/// <returns>Null</returns>
	IEnumerator CreateGrid(){
	Stopwatch sw = new Stopwatch();
	sw.Start();

	int Xiterations = 0;
	int Yiterations = 0;

	if(gridWorldSize.x > maxSectorSizeX){
		Xiterations = Mathf.RoundToInt(gridWorldSize.x/maxSectorSizeX);
	}else{
		maxSectorSizeX = Mathf.RoundToInt(gridWorldSize.x);
		Xiterations = 1;
	}
	if(gridWorldSize.y > maxSectorSizeY){
		Yiterations = Mathf.RoundToInt(gridWorldSize.y/maxSectorSizeY);
	}else{
		maxSectorSizeY = Mathf.RoundToInt(gridWorldSize.y);
		Yiterations = 1;
	}
		
	nodeSize = nodeTemp;
	gridSizeX = Mathf.RoundToInt(maxSectorSizeX/nodeSize);
	gridSizeY = Mathf.RoundToInt(maxSectorSizeY/nodeSize);
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
					nodesAll.Add(grid[k,l]);
					temp.Add(grid[k,l]);
				}

			}
			Grids[gridNum] = temp.ToArray();
			gridNum++;

		}


	}
	sw.Stop();
	print("Created grid in: " +sw.ElapsedMilliseconds+" ms");
	yield return null;
		
	}

	/// <summary>
	/// Metodo para llamar la corutina desde otrs clases.
	/// </summary>
	/// <param name="worldPoint">World point.</param>
	public void StartRebuildGrid(Vector3 worldPoint){
		StartCoroutine(RebuildGrid(worldPoint));
	}

	/// <summary>
	/// Reconstruye un sector;
	/// </summary>
	/// <returns>Null</returns>
	/// <param name="worldPosition">World position.</param>
	IEnumerator RebuildGrid(Vector3 worldPosition){

		Node[] temp = GridFromWorldPosition(worldPosition);

		for(int i = 0;i < temp.Length - 1; i++){
			bool walkable = !(Physics2D.OverlapCircle(temp[i].worldPosition,nodeSize/2,unwalkableMask));
			temp[0].walkable = walkable;
		}
		
		System.GC.Collect();
		System.GC.WaitForPendingFinalizers();
		yield return null;
	}

	/// <summary>
	/// Obtiene un nodo dada una posicion;
	/// </summary>
	/// <returns>Nodo.</returns>
	/// <param name="worldPosition">World position.</param>	
	public Node NodeFromWorldPosition(Vector3 worldPosition){
		
		
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percetnY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percetnY = Mathf.Clamp01(percetnY);
		
		int x = Mathf.RoundToInt(((gridWorldSize.x/nodeSize) - 1) * percentX);
		int y = Mathf.RoundToInt(((gridWorldSize.y/nodeSize) - 1) * percetnY);

		return grid[x,y];
	}

	/// <summary>
	/// Obtiene el sector dada una posicion
	/// </summary>
	/// <returns>Nodos que pertenecen a ese sector Node[].</returns>
	/// <param name="worldPosition">World position.</param>
	public Node[] GridFromWorldPosition(Vector3 worldPosition){
		
		
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percetnY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percetnY = Mathf.Clamp01(percetnY);
		
		int x = Mathf.RoundToInt(((gridWorldSize.x/nodeSize) - 1) * percentX);
		int y = Mathf.RoundToInt(((gridWorldSize.y/nodeSize) - 1) * percetnY);
		
		return Grids[grid[x,y].grid];
	}

	/// <summary>
	/// Obtiene los nodos adyacentes al actual
	/// </summary>
	/// <returns>List<node></returns>
	/// <param name="node">Node.</param>
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

	public void SetNeighboursHeatMap(Node node){
		int actCost = node.heatCost[index];
		for(int x = -3; x <= 3 ; x++){
			for(int y = -3; y <= 3 ; y++){

				if( x == 0 & y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX >= 0 & checkX < (gridWorldSize.x/nodeSize) & checkY >= 0 & checkY < (gridWorldSize.y/nodeSize)){
						int costNode = actCost + Mathf.Max(Mathf.Abs(x),Mathf.Abs(y));
					int contains;


					if(grid[checkX,checkY].heatCost.TryGetValue(index,out contains) == false|| grid[checkX,checkY].heatCost[index] > costNode){
						grid[checkX,checkY].heatCost[index] = costNode;
						heatNodes.Add(grid[checkX,checkY]);
						grid[checkX,checkY].heated = true;
					}
				}
			}
		}
	}


		
		
	/// <summary>
	/// Obtiene todos los creeps dadas una posicion y un rango
	/// </summary>
	/// <returns>Listado de los creeps encontrados</returns>
	/// <param name="pos">Position.</param>
	/// <param name="range">Range.</param>
	public Creep[] GetCreepsArea(Vector3 pos, float range){
		if (grid == null)
			return null;
		List<Creep> creeps = new List<Creep> ();
		int numNodes = (int)Mathf.Ceil (range / nodeSize);
		Node actualNode = NodeFromWorldPosition (pos);
		int i = actualNode.gridX - numNodes;
		if (i < 0)
			i = 0;
		int maxI = actualNode.gridX + numNodes;
		if (maxI >= gridWorldSize.x / nodeSize)
			maxI = (int)(gridWorldSize.x / nodeSize) - 1;
		
		int maxJ = actualNode.gridY + numNodes;
		if (maxJ >= gridWorldSize.y / nodeSize)
			maxJ = (int)(gridWorldSize.y / nodeSize) - 1;
		for (; i <= maxI; i++) {
			int j = actualNode.gridY - numNodes;
			if (j < 0)
				j = 0;
			for (; j <= maxJ; j++) {
				foreach (Creep c in grid[i,j].creeps) {
					creeps.Add (c);
				}
			}
		}
		if (creeps.Count > 0)
			return creeps.ToArray ();
		else
			return null;
	}



	public List<Node> nodesAll = new List<Node>();
	void OnDrawGizmos(){

		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y,1));
		//Comentado por la enorme cantidad de nodos actual.

		if(nodesAll != null & displayGizmos){

			foreach(Node n in nodesAll){
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				Gizmos.color = (n.heated)?Color.blue:Color.white;
				Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeSize-0.3f));
			}

		}
	}

}

