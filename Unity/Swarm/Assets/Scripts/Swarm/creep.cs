using UnityEngine;
using System.Collections;

public class Creep : MonoBehaviour{

	public int life;
	public int armor;
	public int damage;
	public int armorPenetration;
	public float attackRate;
	public float speed;
	[HideInInspector]
	public Vector3[] path;

	void Start () {
		InvokeRepeating("RequestPath",0,1f);
	}
	

	void Update () {
	
	}

<<<<<<< HEAD
	void RequestPath(){

		if(path[0] != null){
			CancelInvoke();
			StartCoroutine(MoveAlongPath());
		}
	}

	IEnumerator MoveAlongPath(){
		int targetIndex = 0;
		if(path[0] != null){
			Vector3 currentWayPoint = path[0];

			while(true){

				if(transform.position == currentWayPoint){

					targetIndex++;
					if(targetIndex >= path.Length){


						yield break;
					}
					currentWayPoint = path[targetIndex];
				}
				transform.position = Vector3.MoveTowards(transform.position,currentWayPoint,speed * Time.fixedDeltaTime);
				yield return null;
			}
		}
=======
	public int Damage(int damage, int armorPen){
		//Debug.Log ("golpeado");
		int defense = Mathf.Max (0, armor - armorPen);
		int damageReal = Mathf.Max (0, damage - defense);
		life -= damageReal;
		if (life <= 0)
			Destroy (gameObject);
		return life;
>>>>>>> origin/master
	}
}
