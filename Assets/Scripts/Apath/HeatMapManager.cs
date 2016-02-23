using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CielaSpike;
public class HeatMapManager : MonoBehaviour {

	public Dictionary<int,List<Node>> heatMaps = new Dictionary<int, List<Node>>();
	Grid grid;
	Node tempNode;
	int contains;
	int Xcost;
	int Ycost;
	Node nodeTemp;
	int check;
	bool updating = false;
	public List<HeatMapUpdater> heatmapsList = new List<HeatMapUpdater>();

	void Start () {
		grid = GetComponent<Grid>();
	}

	public  void StartIterateDictionary(int index){
		heatmapsList.Add(new HeatMapUpdater(index,heatMaps[index].ToArray(),grid));
	}

	public void StopIterateDictionary(){
		CancelInvoke();
		StopAllCoroutines();
	}

	IEnumerator IterateDictionary(){
		while(true){
			foreach(KeyValuePair<int,List<Node>> entry in heatMaps){
				if(!updating){
					updating = true;
					this.StartCoroutineAsync(UpdateHeatMap(entry.Key,entry.Value.ToArray()));
					while(updating){
						yield return new WaitForSeconds(0.1f);
					}

				}
			}
			yield return null;
		}
	
	}
	IEnumerator UpdateHeatMap(int index,Node[] nodes){
		foreach(Node node in nodes){
			tempNode = node;
			if(tempNode.heatCost.ContainsKey(index)){
				

				check = tempNode.gridX -1;
				if(check >= 0 & check < (grid.gridWorldSize.x/grid.nodeSize)){
					nodeTemp =grid.grid[tempNode.gridX -1,tempNode.gridY];
					Xcost = (nodeTemp.walkable & nodeTemp.heatCost.TryGetValue(index,out contains) != false) ? nodeTemp.heatCost[index] + nodeTemp.creeps.Count : tempNode.heatCost[index];
				}else{
					Xcost = tempNode.heatCost[index];
				}
				check = tempNode.gridX +1;

				if(check >= 0 & check < (grid.gridWorldSize.x/grid.nodeSize)){
					nodeTemp =grid.grid[tempNode.gridX +1,tempNode.gridY];
					Xcost -= (nodeTemp.walkable & nodeTemp.heatCost.TryGetValue(index,out contains) != false) ? nodeTemp.heatCost[index] + nodeTemp.creeps.Count : tempNode.heatCost[index];
				}else{
					Xcost -= tempNode.heatCost[index];
				}
				check = tempNode.gridY - 1;

				if(check >= 0 & check < (grid.gridWorldSize.y/grid.nodeSize)){
					nodeTemp =grid.grid[tempNode.gridX,tempNode.gridY - 1];
					Ycost =  (nodeTemp.walkable & nodeTemp.heatCost.TryGetValue(index,out contains) != false) ? nodeTemp.heatCost[index] + nodeTemp.creeps.Count : tempNode.heatCost[index];
				}else{
					Ycost = tempNode.heatCost[index];
				}
				check = tempNode.gridY + 1;

				if(check >= 0 & check < (grid.gridWorldSize.y/grid.nodeSize)){
					nodeTemp =grid.grid[tempNode.gridX,tempNode.gridY + 1];
					Ycost -=  (nodeTemp.walkable & nodeTemp.heatCost.TryGetValue(index,out contains) != false) ? nodeTemp.heatCost[index] + nodeTemp.creeps.Count : tempNode.heatCost[index];
				}else{
					Ycost -= tempNode.heatCost[index];
				}
				yield return Ninja.JumpToUnity;
				node.dir[index] = new Vector3(Xcost,Ycost,0);


			}else{
				Debug.Log("Error en keys");
			}
		}
		updating = false;

	}
}
