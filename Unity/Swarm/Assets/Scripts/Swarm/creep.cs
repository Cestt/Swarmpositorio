using UnityEngine;
using System.Collections;

public class creep : MonoBehaviour{

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
	}
}
