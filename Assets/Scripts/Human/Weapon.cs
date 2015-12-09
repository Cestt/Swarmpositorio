using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float attackRate = 1; //Velocidad de ataque
	public int damage = 1; //Daño de sus ataques
	public int armorPenetration = 0; //Penetracion de armadura de sus ataques
	public int hitThrough = 1; //Numero de enemigos que puede golpear 
	public float speed = 1; //Velocidad de movimiento
	public float distance = 1; //Distancia que puede recorrer

	public GameObject prefabBullet; //Prefab bala
	Human human; //Humano al que pertenece el arma


	public void Ini(Human h){
		//Normalizamos vector direccion
		human = h;
	}

	public void Shoot(){

		Vector3 dir = transform.position - transform.parent.position;
		dir = dir.normalized;
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		//Debug.Log (Vector2.Angle (transform.position + newDir, transform.position));
		GameObject newBullet = (GameObject)Instantiate(prefabBullet,transform.position,Quaternion.Euler(0,0,angle+90));
		newBullet.SetActive (true);
		newBullet.GetComponent<Bullet> ().Ini (dir,damage,armorPenetration,hitThrough,speed,distance);
	}
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log (other.tag);
		if (other.tag == "Swarm") {
			//Debug.Log ("Añadir bicho");
			human.enemies.Add(other.gameObject);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Swarm") {
			human.enemies.Remove(other.gameObject);
		}
	}
}
