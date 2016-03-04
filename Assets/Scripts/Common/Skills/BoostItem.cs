using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoostItem {

	//Lista de unidades afectadas
	List<Unit> units;
	//Lista de las mejoras que se aplican sobre las unidades
	Boost[] boosts;
	//Duracion del efecto
	float timeBoost;
	//Gestor de los boost activos
	BoostManager manager;
	//Marca si esta activo o se va a eliminar
	public bool active;

	/// <summary>
	/// Constructor basico sin lista de unidades a las que afecta <see cref="BoostItem"/> class.
	/// </summary>
	/// <param name="listOfBoosts">Lista de mejoras a las que llama.</param>
	/// <param name="_timeBoost">Tiempo de duracion de las mejoras.</param>
	/// <param name="_manager">Gestor de las mejoras</param>
	public BoostItem(Boost[] listOfBoosts,float _timeBoost, BoostManager _manager){
		units = null;
		boosts = listOfBoosts;
		timeBoost = _timeBoost;
		manager = _manager;
		active = true;
	//	Invoke("RemoveBoost",timeBoost);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BoostItem"/> class.
	/// </summary>
	/// <param name="listOfBoosts">Lista de mejoras a las que llama.</param>
	/// <param name="_timeBoost">Tiempo de duracion de las mejoras.</param>
	/// <param name="_units">Unidades a las que afecta</param>
	/// <param name="_manager">Gestor de las mejoras.</param>
	public BoostItem(Boost[] listOfBoosts,float _timeBoost, List<Unit> _units, BoostManager _manager){
		boosts = listOfBoosts;
		timeBoost = _timeBoost;
		units = _units;
		manager = _manager;
		active = true;
		for (int i=0; units != null && i < units.Count; i++){
			foreach(Boost boost in boosts)
				boost.Apply(units[i]);
		}
//		Invoke("RemoveBoost",timeBoost);
	}

	/// <summary>
	/// Chequea si ya debería finalizar el efecto
	/// </summary>
	/// <param name="deltaTime">Delta time.</param>
	public void CheckBoost(float deltaTime){
		timeBoost -= deltaTime;
		if (timeBoost <= 0)
			RemoveBoost ();
	}

	/// <summary>
	/// Añade una unidad afectada por las mejoras
	/// </summary>
	/// <param name="unit">Unit.</param>
	public void AddUnit(Unit unit){
		foreach(Boost boost in boosts)
			boost.Apply(unit);
	}

	/// <summary>
	/// Elimina una unidad y ya no es afectada por las mejoras
	/// </summary>
	/// <param name="unit">Unit.</param>
	public void RemoveUnit(Unit unit){
		if (units.Contains(unit)){
			foreach(Boost boost in boosts)
				boost.Remove(unit);
			units.Remove(unit);
		}
	}

	/// <summary>
	/// Elimina los efectos aplicados sobre las unidades de todas las mejoras
	/// </summary>
	public void RemoveBoost(){
//		Debug.Log (units.Count);
		for (int i=0; i < units.Count; i++){
			foreach(Boost boost in boosts)
				boost.Remove(units[i]);
		}
		active = false;
		manager.RemoveBoost(this);
	}
}
