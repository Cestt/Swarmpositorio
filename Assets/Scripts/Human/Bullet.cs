using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private int damage = 1; //Daño de sus ataques
	private int armorPenetration = 0; //Penetracion de armadura de sus ataques
	private int hitThrough = 1; //Numero de enemigos que puede golpear 
	private float speed = 1; //Velocidad de movimiento
	private Vector3 dir; //Direccion de movimiento. Vector normalizado
	private float distance = 1; //Distancia que puede recorrer
	private float travel = 0; //Distancia recorrida

	public void Ini(Vector3 newDir,int damage,int armorPenetration,int hitThrough,float speed,float distance){
		//Normalizamos vector direccion
		dir = newDir.normalized;
		/*float angle = Mathf.Atan2 (dir.x, dir.y) * Mathf.Rad2Deg;
		transform.eulerAngles = new Vector3 (0, 0, angle);*/
		this.damage = damage;
		this.armorPenetration = armorPenetration;
		this.hitThrough = hitThrough;
		this.speed = speed;
		this.distance = distance;
		travel = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Movemos
		Vector3 distPos = dir * speed * Time.deltaTime;
		transform.position += distPos;
		travel += Vector3.Distance(transform.position,transform.position+distPos);
		if (travel >= distance) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Swarm") {
			int life = 0;
			//life = other.gameObject.GetComponent<Creep>().Damage(damage,armorPenetration);
			if (life > 0 ){
				Destroy (gameObject);
			}else{
				hitThrough--;
				if (hitThrough <= 0){
					Destroy (gameObject);
				}
			}
		}
	}
}
