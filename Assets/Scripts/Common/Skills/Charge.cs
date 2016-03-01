using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Charge :SkillItem {

	// Unidad que lanza la habilidad
	Unit unit;
	//Ataque que produce sobre las unidades que choca
	public Skill attack;
	//Poisicion en la que finaliza la carga
	[HideInInspector]
	public Vector3 targetPos;
	//Rango de choque con otras unidades
	public float rangeCharge;
	//Anterior posicion
	Vector3 prevPos;
	//Lista de unidades con las que choca
	List<Unit> unitsCharged;

	/// <summary>
	/// Constructor de la habilidad Charge<see cref="Charge"/> class.
	/// </summary>
	/// <param name="_unit">Unidad que ejecuta la habilidad</param>
	/// <param name="_targetPos">Posicion en la que finaliza la carga</param>
	/// <param name="_attack">Ataque que produce sobre las unidades con las que choca</param>
	public Charge(Unit _unit, Vector3 _targetPos, Skill _attack){
		unit = _unit;
		targetPos = _targetPos;
		attack = _attack;
		end = false;
		unit.state = FSM.States.Charge;
		unitsCharged = new List<Unit> ();
	}
	// Update is called once per frame
	public override void Update (float deltaTime) {
		unit.MoveToPosition (targetPos, deltaTime);
		Collider[] colls = Physics.OverlapSphere (unit.thisTransform.position, rangeCharge);
		for (int i = 0; colls != null && i < colls.Length; i++) {
			Unit newUnit = colls [i].gameObject.GetComponent<Unit> ();
			if (!unitsCharged.Contains (newUnit)) {
				attack.Attack (newUnit, unit);
				unitsCharged.Add (newUnit);
			}
		}
		if (prevPos == unit.thisTransform.position) {
			end = true;
		}else
			prevPos = unit.thisTransform.position;
	}

	/// <summary>
	/// Para el efecto de la habilidad
	/// </summary>
	public override void Remove(){
		unit.state = FSM.States.Idle;
	}
}
