using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

	/*//Gen total
	[Tooltip ("Numero de genes. Valor inicial")]
	public int gene;
	//Biomateria total
	[Tooltip ("Numero de biomateria. Valor inicial")]
	public int biomatter;*/


	//Capacidad pool tier 0
	[Tooltip ("Numero limite de tier 0")]
	public int tier0Cuantity = 5000;

	//Lista prefabs de Creeps;
	[Tooltip ("Prefabs del creep de tier 0")]
	public GameObject prefabCreepT0;
	//Lista de coste en genes de los creeps
	private int[] geneCostCreep;
	//Lista prefabs de Humanos;
	public List<GameObject> humanos;

	//Pool Creep 0;
	[HideInInspector]
	public CreepScript[] creep0;
	public CreepScript[] creepT1_1;
	public CreepScript[] creepT1_1A;
	public CreepScript[] creepT1_1B;
	public CreepScript[] creepT1_2;
	public CreepScript[] creepT1_2A;
	public CreepScript[] creepT1_2B;
	public CreepScript[] creepT2_1;
	public CreepScript[] creepT2_1A;
	public CreepScript[] creepT2_1B;
	public CreepScript[] creepT2_2;
	public CreepScript[] creepT2_2A;
	public CreepScript[] creepT2_2B;
	public CreepScript[] creepT3_1;
	public CreepScript[] creepT3_1A;
	public CreepScript[] creepT3_1B;
	public CreepScript[] creepT3_2;
	public CreepScript[] creepT3_2A;
	public CreepScript[] creepT3_2B;

	[Tooltip ("Array tamaño 2 donde van las dos configuraciones de los creep de tier 1")]
	public CreepEvolve[] tier1Evolve = new CreepEvolve[2];
	[Tooltip ("Array tamaño 2 donde van las dos configuraciones de los creep de tier 2")]
	public CreepEvolve[] tier2Evolve = new CreepEvolve[2];
	[Tooltip ("Array tamaño 2 donde van las dos configuraciones de los creep de tier 2")]
	public CreepEvolve[] tier3Evolve = new CreepEvolve[2];
	void Awake(){
		CreatePool();
	}

	/// <summary>
	/// Crea la pool de creeps y humanos
	/// </summary>
	void CreatePool(){
		//Tier 0
		creep0 = new CreepScript[tier0Cuantity];
		FillPoolCreep (prefabCreepT0, prefabCreepT0.GetComponent<Creep> (), tier0Cuantity, creep0);

		/**TIER 1**/
		//Tipo 1
		creepT1_1 = CreatePoolCreep(tier1Evolve[0]);
		if (tier1Evolve [0] != null) {
			creepT1_1A = CreatePoolCreep (tier1Evolve [0].evolveA);
			creepT1_1B = CreatePoolCreep (tier1Evolve [0].evolveB);
		}
			//Tipo 2
		creepT1_2 = CreatePoolCreep(tier1Evolve[1]);
		if (tier1Evolve [1] != null) {
			creepT1_2A = CreatePoolCreep (tier1Evolve [1].evolveA);
			creepT1_2B = CreatePoolCreep (tier1Evolve [1].evolveB);
		}
		/**TIER 2**/
		//Tipo 1
		creepT2_1 = CreatePoolCreep(tier2Evolve[0]);
		if (tier2Evolve [0] != null) {
			creepT2_1A = CreatePoolCreep (tier2Evolve [0].evolveA);
			creepT2_1B = CreatePoolCreep (tier2Evolve [0].evolveB);
		}
		//Tipo 2
		creepT2_2 = CreatePoolCreep(tier2Evolve[1]);
		if (tier2Evolve [1] != null) {
			creepT2_2A = CreatePoolCreep (tier2Evolve [1].evolveA);
			creepT2_2B = CreatePoolCreep (tier2Evolve [1].evolveB);
		}
		/**TIER 3**/
		//Tipo 1
		creepT3_1 = CreatePoolCreep(tier3Evolve[0]);
		if (tier3Evolve [0] != null) {
			creepT3_1A = CreatePoolCreep (tier3Evolve [0].evolveA);
			creepT3_1B = CreatePoolCreep (tier3Evolve [0].evolveB);
		}
		//Tipo 2
		creepT3_2 = CreatePoolCreep(tier3Evolve[1]);
		if (tier3Evolve [1] != null) {
			creepT3_2A = CreatePoolCreep (tier3Evolve [1].evolveA);
			creepT3_2B = CreatePoolCreep (tier3Evolve [1].evolveB);
		}
	}

	/// <summary>
	/// Se crea una pool con la configuracion de creep pasado por parametro
	/// </summary>
	/// <param name="creepEvolve">Configuracion del creep</param>
	private CreepScript[] CreatePoolCreep(CreepEvolve creepEvolve){
		if (creepEvolve == null)
			return null;
		CreepScript[] creepPool = new CreepScript[creepEvolve.numPool];
		FillPoolCreep (creepEvolve.creep.gameObject, creepEvolve.creep,creepEvolve.numPool, creepPool);
		return creepPool;
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
		}
	}


	/// <summary>
	/// Devuelve un creep que este disponible en la pool
	/// </summary>
	/// <returns>El creep disponible</returns>
	/// <param name="tier">Tier del que se quiere obtener un creep</param>
	public CreepScript GetCreep(int tier, int subTier = 0, int subType = -1){
		switch (tier) {
		case 0:
			//if (gene >= geneCostCreep [0]) {
				for (int i = 0; i < tier0Cuantity; i++) {
					if (!creep0 [i].creep.activeInHierarchy) {
						//gene -= geneCostCreep [0];
						return creep0 [i];
					}
				}
			//}
			break;
			/*Para los tier se sigue la siguiente regla
		SubTier tipo creep:
			0 - Tipo 1
			1 - Tipo 2
		SubType evolucion creep:
		   -1 - Ninguna Evolucion
		    0 - Evolucion A
		    1 - Evolucion B
		*/
		case 1:
			if (subType == -1) {
				if (subTier == 0)
					return GetFreeCreep(tier1Evolve [subTier],creepT1_1);
				else
					return GetFreeCreep(tier1Evolve [subTier],creepT1_2);
			} else if (subType == 0) {
				if (subTier == 0)
					return GetFreeCreep(tier1Evolve [subTier].evolveA,creepT1_1A); 
				else
					return GetFreeCreep(tier1Evolve [subTier].evolveA,creepT1_2A); 
			} else if (subType == 1) {
				if (subTier == 0)
					return GetFreeCreep(tier1Evolve [subTier].evolveB,creepT1_1B); 
				else
					return GetFreeCreep(tier1Evolve [subTier].evolveB,creepT1_2B); 
			}
			break;
		case 2:
			if (subType == -1) {
				if (subTier == 0)
					return GetFreeCreep(tier1Evolve [subTier],creepT2_1);
				else
					return GetFreeCreep(tier1Evolve [subTier],creepT2_2);
			} else if (subType == 0) {
				if (subTier == 0)
					return GetFreeCreep(tier1Evolve [subTier].evolveA,creepT2_1A); 
				else
					return GetFreeCreep(tier1Evolve [subTier].evolveA,creepT2_2A); 
			} else if (subType == 1) {
				if (subTier == 0)
					return GetFreeCreep(tier1Evolve [subTier].evolveB,creepT2_1B); 
				else
					return GetFreeCreep(tier1Evolve [subTier].evolveB,creepT2_2B); 
			}
			break;
		case 3:
			if (subType == -1) {
				if (subTier == 0)
					return GetFreeCreep(tier1Evolve [subTier],creepT3_1);
				else
					return GetFreeCreep(tier1Evolve [subTier],creepT3_2);
			} else if (subType == 0) {
				if (subTier == 0)
					return GetFreeCreep(tier1Evolve [subTier].evolveA,creepT3_1A); 
				else
					return GetFreeCreep(tier1Evolve [subTier].evolveA,creepT3_2A); 
			} else if (subType == 1) {
				if (subTier == 0)
					return GetFreeCreep(tier1Evolve [subTier].evolveB,creepT3_1B); 
				else
					return GetFreeCreep(tier1Evolve [subTier].evolveB,creepT3_2B); 
			}
			break;
		}
		return null;
	}

	/// <summary>
	/// Busca un creep libre en el array pasado como parametro
	/// </summary>
	/// <returns>El creep disponible</returns>
	/// <param name="evolve">Configuracion del creep</param>
	/// <param name="creepArray">Array donde busca creep</param>
	private CreepScript GetFreeCreep(CreepEvolve evolve, CreepScript[] creepArray){
		if (EconomyManager.gene < evolve.creep.costGene)
			return null;
		for (int i = 0; i < evolve.numPool; i++) {
			if (!creepArray [i].creep.activeInHierarchy) {
				EconomyManager.gene -= evolve.creep.costGene;
				return creepArray [i];
			}
		}
		return null;
	}
}
