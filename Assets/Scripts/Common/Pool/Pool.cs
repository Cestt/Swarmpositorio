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
	//Capacidad pool tier 1
	const int Tier1_1Cuantity = 500;
	//Pool Creep 0;
	[HideInInspector]
	public CreepScript[] Creep0 = new CreepScript[Tier0Cuantity];
	public CreepScript[] Creep1 = new CreepScript[Tier1_1Cuantity];

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
					clone.transform.parent = transform;
					clone.name = "Creep0_"+i;
					CreepScript tempScript = new CreepScript(clone,creepTempScript);
					Creep0[i] = tempScript;
				}
				break;
			
			case 1:
				for (int i = 0; i <Tier1_1Cuantity; i++){
					clone = Instantiate(tempCreep,Vector3.zero,Quaternion.identity) as GameObject;
					clone.transform.parent = transform;
					clone.name = "Creep1_"+i;
					CreepScript tempScript = new CreepScript(clone,creepTempScript);
					Creep1[i] = tempScript;
				}
				break;
			}
		}
	}

	public CreepScript GetCreep(int tier){
		switch (tier) {
		case 0:
			for (int i =0; i < Tier0Cuantity; i++){
				if (!Creep0[i].creep.activeInHierarchy)
					return Creep0[i];
			}
			break;
		case 1:
			for (int i =0; i < Tier1_1Cuantity; i++){
				if (!Creep1[i].creep.activeInHierarchy)
					return Creep1[i];
			}
			break;
		}
		return null;
	}
}
