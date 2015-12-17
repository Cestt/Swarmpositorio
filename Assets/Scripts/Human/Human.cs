using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : Unit {

	//public int life = 1; //Vida del humano
	//public int armor = 0; //Armadura
	public int damage = 1;
	private float nextAttack = 1; //Tiempo para el siguiente ataque
	public float speed = 1; //Velocidad de movimiento
	public float rotationSpeed = 1; //Velocidad de rotacion
	private Vector3 dir; //Direccion de movimiento. Vector normalizado.

	private float angleTarget; //Angulo objetivo
	private Vector3 dirRot; //Direccion de rotacion
	[HideInInspector]
	public List<GameObject> enemies = new List<GameObject>(); //Lista de enemigos en rango para ataque
	private Weapon weaponEquipped;

	void Awake () {

	}

	void Update () {

	}


}
