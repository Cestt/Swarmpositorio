using UnityEngine;
using System.Collections;

public class Creep : MonoBehaviour{

	public int life;
	public int armor;
	public int damage;
	public int armorPenetration;
	public float attackRate;
	public float speed;

	void Start () {
	
	}
	

	void Update () {
	
	}

	public int Damage(int damage, int armorPen){
		//Debug.Log ("golpeado");
		int defense = Mathf.Max (0, armor - armorPen);
		int damageReal = Mathf.Max (0, damage - defense);
		life -= damageReal;
		if (life <= 0)
			Destroy (gameObject);
		return life;
	}
}
