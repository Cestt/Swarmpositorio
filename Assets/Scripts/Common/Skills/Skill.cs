using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill :MonoBehaviour{

	public enum typesSkill{
		Instant,
		Projectile
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


	/// <summary>
	/// Usa la habilidad
	/// </summary>
	/// <param name="owner">Owner. Unidad que usa la habilidad</param>
	public void Use(Unit owner){
		switch (typeSkill) {
		case typesSkill.Instant:
				Attack(owner);
				break;
		case typesSkill.Projectile:
				Vector3 dir = owner.target.thisTransform.position - owner.thisTransform.position;
				dir = dir.normalized;
				Projectile newProj = ((GameObject)Instantiate(projectile,owner.thisTransform.position + dir,Quaternion.identity)).GetComponent<Projectile>();
			newProj.Ini (owner,dir,enemyPenetration, range,this);
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
}
