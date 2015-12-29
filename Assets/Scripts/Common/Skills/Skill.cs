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
				newProj.Ini (owner,dir,this);
				break;
		}

	}


	public void Attack(Unit owner){
		if (owner.target != null) {
			//Primero ponemos al objetivo de la unidad a cequear el daño
			owner.target.StartCheckDamage ();
			//El daño lo metemos en cola
			ThreadManager.EnQueue (new ParseQueue (owner.target, damage, armorPen, typeDamage));
		}
	}
}
