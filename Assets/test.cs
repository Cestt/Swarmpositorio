using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	public Vector3 [] path;
	// Use this for initialization
	void Start () {
	
	}
	public void Move(){
		StartCoroutine(MoveAlongPath());
	}
	
	IEnumerator MoveAlongPath(){
		
		int targetIndex = 0;
		Debug.Log("Moving Along path");
		if(path != null & path.Length > 0){

			Vector3 currentWayPoint = path[0];
			//Mantiene el bucle de movimiento.
			bool loop =  true;
			while(loop){
				
				if(transform.position == currentWayPoint){
					
					targetIndex++;
					if(targetIndex >= path.Length){
						loop = false;
						path = null;
						targetIndex = 0;
					}
					if(path != null)
						currentWayPoint = path[targetIndex];
				}
				transform.position = Vector3.MoveTowards(transform.position,currentWayPoint,10 * Time.fixedDeltaTime);
				yield return null;
			}
		}

	}

}
