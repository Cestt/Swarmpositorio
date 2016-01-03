using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {


	//Lista prefabs de Creeps;
	public List<GameObject> Creeps = new List<GameObject>();
	//Lista prefabs de Humanos;
	public List<GameObject> Humanos = new List<GameObject>();
	//Capacidad pool tier 0
	const int Tier0Cuantity = 1000;
	//Pool Creep 0;
	[HideInInspector]
	public CreepScript[] Creep0 = new CreepScript[Tier0Cuantity];


	void Awake(){
		CreatePool();
	}

	void CreatePool(){
		foreach(GameObject tempCreep in Creeps){
			
			Creep creepTempScript = tempCreep.GetComponent<Creep>();
			GameObject clone;
			switch(creepTempScript.tier){
				
			case 0:
				
				for(int i = 0;i < Tier0Cuantity;i++){
					clone = Instantiate(tempCreep,Vector3.zero,Quaternion.identity) as GameObject;
					clone.name = "Creep";
					CreepScript tempScript = new CreepScript(clone,creepTempScript);
					Creep0[i] = tempScript;
				}
				break;
			
			}
		}
	}

}
