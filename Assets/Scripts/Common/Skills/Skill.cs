using UnityEngine;
using System.Collections;

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

	private Unit owner; // Unidad que tiene la habilidad

	void Awake(){
		owner = gameObject.GetComponent<Unit> ();
	}

	public void Use(){
		ThreadManager.EnQueue (new ParseQueue (owner.target, damage, armorPen, typeDamage));
	}
}
