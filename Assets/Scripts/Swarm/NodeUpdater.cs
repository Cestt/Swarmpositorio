using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeUpdater : MonoBehaviour {
	public List<Creep> creeps = new List<Creep>();
	Grid grid;
	Node _node;

	void Awake(){
		grid = GameObject.Find("GameManager/PathFinder").GetComponent<Grid>();
		StartCoroutine(CheckGridPosition());
	}

	IEnumerator CheckGridPosition(){
		while(true){
			for(int i = 0; i < creeps.Count - 1; i++){
				_node = grid.NodeFromWorldPosition((creeps[i].thisTransform.position));
					if(creeps[i] != null){
						if(creeps[i].node != null){
							if(creeps[i].node != _node){
								creeps[i].node.creeps.Remove(creeps[i]);
								creeps[i].node = _node;
								creeps[i].node.creeps.Add(creeps[i]);
							}
						}else{
							creeps[i].node = _node;
							creeps[i].node.creeps.Add(creeps[i]);
						}
					}

			}
	    	yield return null;
		}

	}

}
