using UnityEngine;
using System.Collections;

public class CreepSquad : Unit {

	[HideInInspector]
	public Squad squad;
	public Vector3 goTo = new Vector3(0,0,10000);
	public Vector3 startPos = new Vector3(0,0,1000);


	void Start(){
		StartCoroutine(Move());
	}

	IEnumerator Move(){
		while(true){
			if(goTo.z != 10000){
				Utils.LookAt2D(1.2f,thisTransform,thisTransform.position + goTo);
				thisTransform.position = Vector3.MoveTowards(thisTransform.position,thisTransform.position + goTo ,speed * Time.deltaTime);
			}
			yield return null; 
		}

	}
}
