using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoostManager : MonoBehaviour {

	//Lista de boost activos
	List<BoostItem> boostsEnabled = new List<BoostItem>();

	/// <summary>
	/// Añade un boost nuevo que se debe gestionar
	/// </summary>
	/// <returns>Devuelve el boost generado.</returns>
	/// <param name="boosts">Lista de mejoras que va a aplicar el boost</param>
	/// <param name="timeBoost">Duracion del boost.</param>
	/// <param name="units">Unidades afectadas por el boost</param>
	/// <param name="type">Tipo de boost que se va a generar</param>
	public BoostItem AddBoost(Boost[] boosts,float timeBoost ,List<Unit> units, Skill.typesSkill type){
		if (type == Skill.typesSkill.Boost || type == Skill.typesSkill.BoostSpawn){
			//Debug.Log ("Nuevo boost: tiempo: " + timeBoost);
			BoostItem boostItem =  new BoostItem(boosts,timeBoost, units, this);
			boostsEnabled.Add(boostItem);
			return boostItem;
		}
		return null;
		/*En caso de que sea de tipo Area Estatica
         boostsEnabled.Add(new BoostAreaDynamic(boosts, this, pos <---- comoo sacamos la posiciiiiiiooon ));
         */
	}

	void Update(){
		for (int i=boostsEnabled.Count-1; i >= 0; i--)
			boostsEnabled[i].CheckBoost (Time.deltaTime);
	}

	/// <summary>
	/// Elimina un boost de la lista de los activos
	/// </summary>
	/// <param name="boost">Boost.</param>
	public void RemoveBoost(BoostItem boost){
		boostsEnabled.Remove(boost);
	}

	/// <summary>
	/// Elimina una unidad de todos los boost activos. Esto ocurre cuando la unidad muere o pierde sus mejoras.
	/// </summary>
	/// <param name="unit">Unit.</param>
	public void RemoveUnit(Unit unit){
		foreach(BoostItem boostItem in boostsEnabled){
			boostItem.RemoveUnit(unit);
		}
	}
}
