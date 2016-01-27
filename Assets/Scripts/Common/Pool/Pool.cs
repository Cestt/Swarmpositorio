using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

	//Gen total
	public int gene;
	//Biomateria total
	public int biomatter;

	//Capacidad pool tier 0
	public int tier0Cuantity = 5000;
	//Capacidad pool tier 1
	public int tier1_1Cuantity = 500;

	//Lista prefabs de Creeps;
	public List<GameObject> creeps;
	//Lista de coste en genes de los creeps
	private int[] geneCostCreep;
	//Lista prefabs de Humanos;
	public List<GameObject> humanos;

	//Pool Creep 0;
	[HideInInspector]
	public CreepScript[] creep0;
	public CreepScript[] creep1;

	void Awake(){
		CreatePool();
	}

	/// <summary>
	/// Crea la pool de creeps y humanos
	/// </summary>
	void CreatePool(){
		creep0 = new CreepScript[tier0Cuantity];
		creep1 = new CreepScript[tier1_1Cuantity];
		geneCostCreep = new int[creeps.Count];
		//Recorremos la lista de creeps y vamos creando la pool
		foreach(GameObject tempCreep in creeps){
			Creep creepTempScript = tempCreep.GetComponent<Creep>();

			//En funcion del tier del creep rellenamos su correpsondiente pool
			if (creepTempScript.tier == 0) {
				FillPoolCreep (tempCreep, creepTempScript, tier0Cuantity, creep0);
			} else if (creepTempScript.tier == 1) {
				FillPoolCreep (tempCreep, creepTempScript, tier1_1Cuantity, creep1);
			}

		}
	}

	/// <summary>
	/// Rellenamos la pool del creep
	/// </summary>
	/// <param name="tempCreep">Prefab del creep</param>
	/// <param name="creepTempScript">Script perteneciente al prefab</param>
	/// <param name="cuantity">Cantidad de creeps a meter en la pool</param>
	/// <param name="creepArray">Pool donde se van a meter los creeps</param>
	void FillPoolCreep(GameObject tempCreep,Creep creepTempScript, int cuantity, CreepScript[] creepPool){
		GameObject clone;
		for(int i = 0;i < cuantity;i++){
			clone = Instantiate(tempCreep,Vector3.zero,Quaternion.identity) as GameObject;
			clone.transform.parent = transform;
			clone.name = "Creep"+creepTempScript.tier+"_"+i;
			CreepScript tempScript = new CreepScript(clone,clone.GetComponent<Creep>());
			creepPool[i] = tempScript;
			geneCostCreep [creepTempScript.tier] = creepTempScript.costGene;
		}
	}


	/// <summary>
	/// Devuelve un creep que este disponible en la pool
	/// </summary>
	/// <returns>El creep disponible</returns>
	/// <param name="tier">Tier del que se quiere obtener un creep</param>
	public CreepScript GetCreep(int tier){
		switch (tier) {
		case 0:
			if (gene > geneCostCreep [0]) {
				for (int i = 0; i < tier0Cuantity; i++) {
					if (!creep0 [i].creep.activeInHierarchy) {
						//gene -= geneCostCreep [0];
						return creep0 [i];
					}
				}
			}
			break;
		case 1:
			if (gene > geneCostCreep [1]) {
				for (int i = 0; i < tier1_1Cuantity; i++) {
					if (!creep1 [i].creep.activeInHierarchy) {
						//gene -= geneCostCreep [1];
						return creep1 [i];
					}
				}
			}
			break;
		}
		return null;
	}
}
