using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Battlehub.Dispatcher;



public class Unit : MonoBehaviour {

	public int life = 1; //Vida de la unidad
	public int armor; //Armadura de la unidad
	[HideInInspector]
	public int lifeIni;

	public bool canAttack = true; //Si puede atacar la unidad
	[HideInInspector]
	public FSM.States state;


	public int weaknessType; //Tipo al que es debil la unidad
	[HideInInspector]
	public TypesAttacks typesAttacks; //Clase donde se tienen todos los tipos de ataque
	[HideInInspector]
	public Transform thisTransform; //Transform propio.
	[HideInInspector]
	public GameObject thisGameObject; //Transform propio.

	[HideInInspector]
	public Unit target; //Unidad objetivo al que apunta
	//[HideInInspector]
	public List<Skill> skills = new List<Skill>(); //Lista de habilidades. La 0 es la habilidad basica

	private bool endDamage;

	public void Awake(){
		typesAttacks = GameObject.Find ("GameManager/TypesAttacks").GetComponent<TypesAttacks> ();
		thisTransform = transform;
		thisGameObject = gameObject;
		lifeIni = life;
	}




	#region Daño Hilos
	/// <summary>
	/// Damage. Gestiona el daño recibido por un ataque. CON HILOS
	/// </summary>
	/// <param name="damage">Damage. Daño del ataque</param>
	/// <param name="armorPen">Armor pen. Penetracion de armadura</param>
	/// <param name="typeAttack">TypeAttack. Tipo del ataque</param> 
	public void Damage(int damage, int armorPen, int typeAttack, Unit enemy){
		int damageWeak = damage;
		if (weaknessType == typeAttack) {
			damageWeak = (int)(damage * typesAttacks.types[typeAttack].value);
		}
		int damageReal = Mathf.Max (0, damageWeak - Mathf.Max (0, armor - armorPen));
		life -= damageReal;
		Dispatcher.Current.BeginInvoke(() =>{
			if (life <= 0) {
				enemy.target = null;
				Dead();
			}
		});

	}
	/// <summary>
	/// Metodo que llama a la corrutina que comprueba si ha finalizado el ataque
	/// </summary>
	public void StartCheckDamage(Unit enemy){
		//La condicion que dice si ha terminado el ataque la ponemos a falso
		endDamage = false;
		if(gameObject.activeInHierarchy)
			StartCoroutine (CheckDamage (enemy));
	}

	/// <summary>
	/// Corrutina que espera a que finalice el ataque y comprueba si la unidad resulta muerta tras este
	/// </summary>
	IEnumerator CheckDamage(Unit enemy){

		while (!endDamage) {
			yield return null;
		}
		if (life <= 0) {
			enemy.target = null;
			Dead();
		}
	}

	#endregion

	/// <summary>
	/// Metodo que se llama cuando la unidad muere.
	/// </summary>
	public virtual void Dead (){}
}
