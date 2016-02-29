using UnityEngine;
using System.Collections;
using System.Threading;

public class HeatMapUpdater {

	Grid grid;
	Node tempNode;
	int contains;
	int Xcost;
	int Ycost;
	Node nodeTemp;
	int check;
	bool updating = false;
	public int index;
	Node[] nodes;
	Thread a;

	public HeatMapUpdater(int _index,Node[] _nodes,Grid _grid){
		index = _index;
		nodes = _nodes;
		grid = _grid;
		a = new Thread(UpdateHeatMap);
		a.Start();
	}

	public void Abort(){
		
		foreach(Node n in nodes){
			n.heatCost[index] = -1;
			n.heated = false;
		}
		a.Abort();
		Debug.Log("End");
	}

	void UpdateHeatMap(){
		float euristica;
		while(true){
			foreach(Node node in nodes){
				euristica = 0;
				tempNode = node;
				if(tempNode.heatCost.ContainsKey(index)){
					check = tempNode.gridX -1;
					if(check >= 0 & check < (grid.gridWorldSize.x/grid.nodeSize)){
						nodeTemp = grid.grid[tempNode.gridX -1,tempNode.gridY];
						euristica+= nodeTemp.creeps.Count;
						Xcost = (nodeTemp.walkable & nodeTemp.heatCost.TryGetValue(index,out contains) != false) ? nodeTemp.heatCost[index] + nodeTemp.creeps.Count : tempNode.heatCost[index];
					}else{
						Xcost = tempNode.heatCost[index];
					}
					check = tempNode.gridX +1;

					if(check >= 0 & check < (grid.gridWorldSize.x/grid.nodeSize)){
						nodeTemp =grid.grid[tempNode.gridX +1,tempNode.gridY];
						euristica+= nodeTemp.creeps.Count;
						Xcost -= (nodeTemp.walkable & nodeTemp.heatCost.TryGetValue(index,out contains) != false) ? nodeTemp.heatCost[index] + nodeTemp.creeps.Count : tempNode.heatCost[index];
					}else{
						Xcost -= tempNode.heatCost[index];
					}
					check = tempNode.gridY - 1;

					if(check >= 0 & check < (grid.gridWorldSize.y/grid.nodeSize)){
						nodeTemp = grid.grid[tempNode.gridX,tempNode.gridY - 1];
						euristica+= nodeTemp.creeps.Count;
						Ycost =  (nodeTemp.walkable & nodeTemp.heatCost.TryGetValue(index,out contains) != false) ? nodeTemp.heatCost[index] + nodeTemp.creeps.Count : tempNode.heatCost[index];
					}else{
						Ycost = tempNode.heatCost[index];
					}
					check = tempNode.gridY + 1;

					if(check >= 0 & check < (grid.gridWorldSize.y/grid.nodeSize)){
						nodeTemp =grid.grid[tempNode.gridX,tempNode.gridY + 1];
						euristica+= nodeTemp.creeps.Count;
						Ycost -=  (nodeTemp.walkable & nodeTemp.heatCost.TryGetValue(index,out contains) != false) ? nodeTemp.heatCost[index] + nodeTemp.creeps.Count : tempNode.heatCost[index];
					}else{
						Ycost -= tempNode.heatCost[index];
					}
					if(euristica > 0){
						node.dir[index] = new Vector3(Xcost,Ycost,0)/(euristica);	
					}else{
						node.dir[index] = new Vector3(Xcost,Ycost,0);
					}



				}else{
					Debug.Log("Error en keys");
				}
			}
			Thread.Sleep(1);
		}

		updating = false;

	}
}
