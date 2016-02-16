using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Skill :MonoBehaviour{

	/* Tipos de habilidad
	* Instant - Daño instantaneo. Ejemplo ataque de un creep.
	* Projectile - Dispara el proyectil que se tenga en el prefab
	* Boost - Cambia uno o varios stats de la unidad
	*
	*/
	public enum typesSkill{
		Instant,
		Projectile,
		Boost
	}
	public float coolDown = 1; //Tiempo que tarda en recargarse la habilidad
	public int damage = 1; //Daño que hace la habilidad
	public int armorPen = 0; //Penetracion de armadura
	public float range = 0.5f; //Distancia de alcance
	public typesSkill typeSkill; //Tipo de ataque
	[HideInInspector]
	public TypesAttacks typesAttacks; //Clase donde se tienen todos los tipos de ataque
	[HideInInspector]
	public int typeDamage; //Tipo al que es debil la unidad
	[HideInInspector]
	public GameObject projectile; //Proyectil generado si el ataque es de tipo Projectile
	[HideInInspector]
	public int enemyPenetration; //Numero de enemigos a los que puede dañar y penetrar la habilidad
	[HideInInspector]
	//[Tooltip("Tiempo que dura el boost")]
	public int timeBoost; 
	public List<Boost> boosts = new List<Boost>(); //Lista de los boosts que activan

	/// <summary>
	/// Usa la habilidad
	/// </summary>
	/// <param name="unit">unit. Unidad objetivo de la habilidad o unidad propietaria. Depende del tipo.</param>
	public void Use(Unit unit){
		switch (typeSkill) {
		case typesSkill.Instant:
			Attack(unit);
			break;
		case typesSkill.Projectile:
			Vector3 dir = unit.target.thisTransform.position - unit.thisTransform.position;
			dir = dir.normalized;
			Projectile newProj = ((GameObject)Instantiate(projectile,unit.thisTransform.position + dir,Quaternion.identity)).GetComponent<Projectile>();
			newProj.Ini (unit,dir,enemyPenetration, range,this);
			break;
		case typesSkill.Boost:
			foreach (Boost boost in boosts)
				boost.Apply (unit);
			break;
		}


	}



	/// <summary>
	/// Ataca al objetivo del propietario.
	/// </summary>
	/// <param name="owner">Owner. Unidad que usa la habilidad</param>
	public void Attack(Unit owner){
		if (owner.target != null) {
			/***********Uso de hilos***************
			//Primero ponemos al objetivo de la unidad a cequear el daño
			owner.target.StartCheckDamage (owner);
			*********************************/
			//El daño lo metemos en cola
			//ThreadManager.EnQueue (new ParseQueue (owner.target, damage, armorPen, typeDamage, owner));
			//Daño directo en hilo secundario
			owner.target.LaunchDamage(damage,armorPen,typeDamage, owner);
		}
	}

	/// <summary>
	/// Ataca a un objetivo que no tiene por que ser al que apunta la unidad
	/// </summary>
	/// <param name="target">Target. Objetivo</param>
	/// <param name="owner">Owner. Unidad que ataca</param>
	public void Attack(Unit target, Unit owner){
		if (target != null) {
			/***********Uso de hilos***************
			//Primero ponemos al objetivo de la unidad a cequear el daño
			owner.target.StartCheckDamage (owner);
			*********************************/
			//El daño lo metemos en cola
			//ThreadManager.EnQueue (new ParseQueue (target, damage, armorPen, typeDamage, owner));
			//Daño en hilo secundario
			target.LaunchDamage(damage,armorPen,typeDamage, owner);
		}
	}

	/// <summary>
	/// Remueve el efecto de la habilidad sobre la unidad
	/// </summary>
	/// <param name="unit">unit. Unidad afectada</param>
	public void Remove(Unit unit){
		switch (typeSkill) {
		case typesSkill.Boost:
			foreach (Boost boost in boosts)
				boost.Remove (unit);
			break;
		}
	}
}