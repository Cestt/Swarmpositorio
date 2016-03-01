using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

	//Valor estatico del propio gestor de habilidades
	public static SkillManager skillManager;
	//Valor estatico del gestor de boosts
	public static BoostManager boostManager;
	// Lista de habilidades activas
	List<SkillItem> skillsEnabled;

	void Awake () {
		skillManager = this;
		boostManager = transform.FindChild ("BoostManager").GetComponent<BoostManager> ();
		skillsEnabled = new List<SkillItem> ();
	}

	/// <summary>
	/// Añade una habilidad de Charge a la lista de habilidades activas
	/// </summary>
	/// <param name="unit">Unidad que lanza la habilidad</param>
	/// <param name="targetPos">Posicion de destino</param>
	/// <param name="attack">Ataque producido por la carga</param>
	public void AddCharge(Unit unit, Vector3 targetPos, Skill attack){
		skillsEnabled.Add (new Charge (unit, targetPos,attack));
	}
	// Update is called once per frame
	void Update () {
		for (int i = skillsEnabled.Count-1; i >= 0; i--) {
			if (skillsEnabled [i].end) {
				skillsEnabled [i].Remove ();
				skillsEnabled.RemoveAt (i);
			}
			else {
				skillsEnabled [i].Update (Time.deltaTime);
			}
		}
	}
}
