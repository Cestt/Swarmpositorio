using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public int life = 1; //Vida de la unidad
	public int armor; //Armadura de la unidad
	//public FSM.states state;

	[HideInInspector]
	public int weaknessType; //Tipo al que es debil la unidad
	[HideInInspector]
	public TypesAttacks typesAttacks; //Clase donde se tienen todos los tipos de ataque
	[HideInInspector]
	public Transform thisTransform;

	void Awake(){
		typesAttacks = GameObject.Find ("GameManager/TypesAttacks").GetComponent<TypesAttacks> ();
		thisTransform = transform;
	}
	/// <summary>
	/// Damage. Gestiona el daño recibido por un ataque
	/// </summary>
	/// <param name="damage">Damage. Daño del ataque</param>
	/// <param name="armorPen">Armor pen. Penetracion de armadura</param>
	/// <param name="typeAttack">TypeAttack. Tipo del ataque</param> 
	public void Damage(int damage, int armorPen, int typeAttack){
		int damageWeak = damage;
		if (weaknessType == typeAttack) {
			damageWeak = (int)(damage * typesAttacks.types[typeAttack].value);
		}
		int damageReal = Mathf.Max (0, damageWeak - Mathf.Max (0, armor - armorPen));
		life -= damageReal;
	}
}
