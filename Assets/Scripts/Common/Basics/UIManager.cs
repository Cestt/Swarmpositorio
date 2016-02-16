using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	//TouchManager para saber el estado de lo que se tenga seleccionado
	TouchManager touchManager;
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
	// Use this for initialization
	void Awake () {
		buttonEvolveSpawnT1 = transform.FindChild ("ButtonEvolveSpawnT1").GetComponent<Button> ();
		buttonEvolveSpawnT2 = transform.FindChild ("ButtonEvolveSpawnT2").GetComponent<Button> ();
		buttonEvolveCreepA = transform.FindChild ("ButtonEvolveCreepA").GetComponent<Button> ();
		buttonEvolveCreepB = transform.FindChild ("ButtonEvolveCreepB").GetComponent<Button> ();
		buttonSkillSpawn = transform.FindChild ("ButtonSkillSpawn").GetComponent<Button> ();
		buttonEvolveCreepA.interactable = false;
		buttonEvolveCreepB.interactable = false;
		buttonEvolveSpawnT2.interactable = false;
		buttonSkillSpawn.interactable = false;
		touchManager = GameObject.Find ("GameManager/TouchManager").GetComponent<TouchManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Boton de evolucion
		if (touchManager.Selected.tier == 3) {
			buttonEvolveSpawnT1.interactable = false;
			buttonEvolveSpawnT2.interactable = false;
		} else {
			buttonEvolveSpawnT1.interactable = true;
			buttonEvolveSpawnT2.interactable = true;
		}
		if (!touchManager.Selected.skillTierActive) {
			if (touchManager.Selected.tier > 0) {
				if (touchManager.Selected.subType == -1) {
					buttonEvolveCreepA.interactable = true;
					buttonEvolveCreepB.interactable = true;
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
	}
}
