using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : MonoBehaviour {

	public int life = 1; //Vida del humano
	public int armor = 0; //Armadura
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
		weaponEquipped = transform.FindChild ("Weapon").GetComponent<Weapon> ();
		weaponEquipped.Ini(this);
		nextAttack = weaponEquipped.attackRate;
	}

	void Update () {

		//ATAQUE: Si tiene listo el siguiente ataque y tiene objetivos al alcance
		if (nextAttack <= 0 && enemies.Count > 0) {
			//Buscamos el enemigo mas cercano
			GameObject target = null;
			float distance = 99999999;
			for (int i=(enemies.Count-1); i >= 0; i--){
				if (enemies[i] != null){
					float newDistance = Vector3.Distance(transform.position,enemies[i].transform.position);
					if (newDistance < distance){
						target=enemies[i];
						distance = newDistance;
					}
				}else{
					enemies.RemoveAt(i);
				}
			}
			if (target != null){
				nextAttack = weaponEquipped.attackRate;
				//Atacamos al objetivo mas cercano
				Vector3 dir = target.transform.position - transform.position;
				dir = dir.normalized;
				angleTarget = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
				weaponEquipped.Shoot();
				angleTarget += 90;
				if (angleTarget < transform.eulerAngles.z){
					if (Mathf.Abs (transform.eulerAngles.z - angleTarget) < 180)
						dirRot = new Vector3(0,0,-1);
					else
						dirRot = new Vector3(0,0,1);
				}else{
					if (Mathf.Abs ( angleTarget - transform.eulerAngles.z) < 180)
						dirRot = new Vector3(0,0,1);
					else
						dirRot = new Vector3(0,0,-1);
				}
			}
		}
		if (angleTarget != transform.eulerAngles.z) {
			float distAngle = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z,angleTarget));
			if (distAngle < 2){
				transform.eulerAngles = new Vector3(0,0,angleTarget);
			}else{
				transform.eulerAngles += dirRot*rotationSpeed*Time.deltaTime;
			}
		}
		nextAttack -= Time.deltaTime;
	}


}
