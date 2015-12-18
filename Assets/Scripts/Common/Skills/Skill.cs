using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill :MonoBehaviour{

	public enum typesSkill{
		Meele
		//,Distance
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

	public void Use(Unit owner){
		owner.target.StartCheckDamage ();
		ThreadManager.EnQueue (new ParseQueue (owner.target, damage, armorPen, typeDamage));
	}

}
