using UnityEngine;
using System.Collections;

public class UnitSquad : Unit {

	[HideInInspector]
	public Squad squad;
	[HideInInspector]
	public bool leader;
	[HideInInspector]
	public Vector3 goTo = new Vector3(0,0,10000);
	[HideInInspector]
	public Vector3 startPos = new Vector3(0,0,1000);
	public Squad.squadType tipoUnidad;


	void Start(){
		StartCoroutine(Move());
	}

	IEnumerator Move(){
		while(true){
			if(tipoUnidad == Squad.squadType.Swarm){
				if(goTo.z != 10000){
					Utils.LookAt2D(1.2f,thisTransform,thisTransform.position + goTo);
					thisTransform.position = Vector3.MoveTowards(thisTransform.position,thisTransform.position + goTo ,speed * Time.deltaTime);
				}
			}else if(tipoUnidad == Squad.squadType.Swarm){
				Utils.LookAt2D(1.2f,thisTransform,goTo);
				thisTransform.position = goTo;
			}

			yield return null; 
		}

	}

}
