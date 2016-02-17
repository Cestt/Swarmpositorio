using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	//TouchManager para saber el estado de lo que se tenga seleccionado
	TouchManager touchManager;
	//Boton de nuevo spawn
	Button buttonNewSpawn;
	//Boton para evolucionar el Spawn por el primer Creep
	Button buttonEvolveSpawnT1; 
	//Boton para evolucionar el Spawn por el segundo Creep
	Button buttonEvolveSpawnT2; 
	//Boton para evolucionar el Creep a su tipo A
	Button buttonEvolveCreepA;  
	//Boton para evolucionar el Creep a su tipo B
	Button buttonEvolveCreepB;
	//Boton para activar la habilidad del Spawn
	Button buttonSkillSpawn;
	//Boton para añadir una piscina de biomateria
	Button buttonAddBioPool;

	Pool pool;
	// Use this for initialization
	void Awake () {
		buttonNewSpawn = transform.FindChild ("ButtonNewSpawn").GetComponent<Button> ();
		buttonEvolveSpawnT1 = transform.FindChild ("ButtonEvolveSpawnT1").GetComponent<Button> ();
		buttonEvolveSpawnT2 = transform.FindChild ("ButtonEvolveSpawnT2").GetComponent<Button> ();
		buttonEvolveCreepA = transform.FindChild ("ButtonEvolveCreepA").GetComponent<Button> ();
		buttonEvolveCreepB = transform.FindChild ("ButtonEvolveCreepB").GetComponent<Button> ();
		buttonSkillSpawn = transform.FindChild ("ButtonSkillSpawn").GetComponent<Button> ();
		buttonAddBioPool = transform.FindChild ("ButtonAddBioPool").GetComponent<Button> ();
		buttonEvolveCreepA.interactable = false;
		buttonEvolveCreepB.interactable = false;
		buttonEvolveSpawnT2.interactable = false;
		buttonSkillSpawn.interactable = false;
		touchManager = GameObject.Find ("GameManager/TouchManager").GetComponent<TouchManager> ();
		pool = GameObject.Find ("Pool").GetComponent<Pool> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Boton nuevo spawn
		if (EconomyManager.gene < EconomyManager.newSpawnCostGene || EconomyManager.biomatter < EconomyManager.newSpawnCostBio) {
			buttonNewSpawn.interactable = false;
		} else {
			buttonNewSpawn.interactable = true;
		}
		//Botones de evolucion
		if (touchManager.selected.tier == 3) {
			buttonEvolveSpawnT1.interactable = false;
			buttonEvolveSpawnT2.interactable = false;
		} else {
			buttonEvolveSpawnT1.interactable = CheckCostButtonTier(touchManager.selected.tier+1,0,-1);
			buttonEvolveSpawnT2.interactable = CheckCostButtonTier(touchManager.selected.tier+1,1,-1);
		}
		if (!touchManager.selected.skillTierActive) {
			if (touchManager.selected.tier > 0) {
				if (touchManager.selected.subType == -1) {
					buttonEvolveCreepA.interactable = CheckCostButtonTier(touchManager.selected.tier,touchManager.selected.subTier,0);
					buttonEvolveCreepB.interactable = CheckCostButtonTier(touchManager.selected.tier,touchManager.selected.subTier,1);;
					buttonSkillSpawn.interactable = false;
				} else {
					buttonEvolveCreepA.interactable = false;
					buttonEvolveCreepB.interactable = false;
					buttonSkillSpawn.interactable = true;
				}
			} else {
				buttonEvolveCreepA.interactable = false;
				buttonEvolveCreepB.interactable = false;
				buttonSkillSpawn.interactable = false;
			}
		} else {
			buttonEvolveSpawnT1.interactable = false;
			buttonEvolveCreepA.interactable = false;
			buttonEvolveCreepB.interactable = false;
			buttonSkillSpawn.interactable = false;
		}
		//Boton de piscina de biomateria
		if (touchManager.selected.numBioPools < touchManager.selected.costBioPoolGene.Length && 
			EconomyManager.gene >= touchManager.selected.costBioPoolGene[touchManager.selected.numBioPools])
			buttonAddBioPool.interactable = true;
		else
			buttonAddBioPool.interactable = false;
		
	}


	bool CheckCostButtonTier(int tier, int subTier, int subType){
		switch (tier) {
		case 1:
			//Comprobamos nulos
			if ((subType == -1 && pool.tier1Evolve[subTier] == null) ||
				(subType == 0 && pool.tier1Evolve[subTier].evolveA == null) ||
				(subType == 1 && pool.tier1Evolve[subTier].evolveB == null)) {
					return false;
			} 
			return (EconomyManager.gene >= EconomyManager.GetCreepEvolveCostGene (tier, subTier, subType) &&
					EconomyManager.biomatter >= EconomyManager.GetCreepEvolveCostBio (tier, subTier, subType));
			break;
		case 2:
			if ((subType == -1 && pool.tier2Evolve[subTier] == null) ||
				(subType == 0 && pool.tier2Evolve[subTier].evolveA == null) ||
				(subType == 1 && pool.tier2Evolve[subTier].evolveB == null)) {
				return false;
			} 
			return (EconomyManager.gene >= EconomyManager.GetCreepEvolveCostGene (tier, subTier, subType) &&
				EconomyManager.biomatter >= EconomyManager.GetCreepEvolveCostBio (tier, subTier, subType));
			break;
		case 3:
			if ((subType == -1 && pool.tier3Evolve[subTier] == null) ||
				(subType == 0 && pool.tier3Evolve[subTier].evolveA == null) ||
				(subType == 1 && pool.tier3Evolve[subTier].evolveB == null)) {
				return false;
			} 
			return (EconomyManager.gene >= EconomyManager.GetCreepEvolveCostGene (tier, subTier, subType) &&
				EconomyManager.biomatter >= EconomyManager.GetCreepEvolveCostBio (tier, subTier, subType));
			break;
		}
		return false;
	}
}
