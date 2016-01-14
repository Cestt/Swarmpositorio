using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : Unit {

	//Radio deteccion del humano;
	public float detectionRadius;
	//Velocidad de movimiento
	public float speedAlongPath = 50;
	//Camino generado por el PathFinding
	[HideInInspector]
	public Vector3[] path;
	//Punto de la ruta en la que se encuentra
	int targetIndex = 0;

	void Start(){
		path = null;

	}
	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable () {
		//Iniciar separacion de los creeps(Comentado hasta revision y optimizacion).
		//StartCoroutine(CheckSeparation());
		//Inicializamos el path.
		path = null;
		//Inicializamos al estado principal;
		state = FSM.States.Idle;
		life = lifeIni;
		stateChanger();

	}

	void OnDisable() {	
		//Re inicializamos el path;
		path = null;
		//Paramos la IA;
		state = FSM.States.Idle;
		StopAllCoroutines();
	}




	/// <summary>
	/// Cambia su estado propio dentro de la FSM
	/// </summary>
	void stateChanger(){

		StopAllCoroutines ();

		if(state == FSM.States.Idle){
			StartCoroutine(EnemyDetection());
		}
		if(state == FSM.States.Move){
			if(path != null ){
				if(path[path.Length - 1] != thisTransform.position){
					StartCoroutine(EnemyDetection());
					StartCoroutine(MoveAlongPath());
				}
			}else{
				state = FSM.States.Idle;
				stateChanger();
			}
		}
		if(state == FSM.States.Attack){
			StopAllCoroutines ();
			StartCoroutine(Attack());
		}

	}


	/// <summary>
	/// Mueve el creep a lo largo de path.
	/// </summary>
	IEnumerator MoveAlongPath(){
		Debug.Log("Moving Along path");
		if(path != null){
			Vector3 currentWayPoint = path[0];
			//Mantiene el bucle de movimiento.
			bool loop =  true;
			while(loop){

				if(thisTransform.position == currentWayPoint){

					targetIndex++;
					if(targetIndex >= path.Length){
						loop = false;
						path = null;

					}

					currentWayPoint = path[targetIndex];
				}
				thisTransform.position = Vector3.MoveTowards(thisTransform.position,currentWayPoint,speedAlongPath * Time.fixedDeltaTime);
				yield return null;
			}
		}
		targetIndex = 0;
		state = FSM.States.Idle;
		stateChanger();
	}

	/// <summary>
	/// Mantiene al creep buscando enemigos alrededor suya
	/// </summary>
	IEnumerator EnemyDetection(){
		bool loop = true;//Mantiene el bucle.
		while(loop){
			if (CheckEnemies ()) {
				loop = false;
				state = FSM.States.Attack;
				yield return new WaitForSeconds (Random.Range (0.1f, 0.15f));
				stateChanger ();
			}
			yield return new WaitForSeconds(Random.Range(0.1f,0.15f));
		}
	}

	/// <summary>
	/// Chequea si tiene enemigos en su area de vision y devuelve true en caso afirmativo
	/// </summary>
	/// <returns><c>true</c>, if enemies was checked, <c>false</c> otherwise.</returns>
	private bool CheckEnemies(){
		Collider2D[] colls = new Collider2D[25];//Maximo de colliders que detectara alrededor suya.
		float points = -1;//Euristica de puntos para evaluar el mejor objetivo.
		Collider2D bestTarget = null;//Objetivo designado.
		int collsNum =  Physics2D.OverlapCircleNonAlloc(thisTransform.position,detectionRadius,colls,1 << LayerMask.NameToLayer("Creep"));
			if(collsNum > 0){
				foreach(Collider2D coll in colls){
					if(coll != null){
						if(points < 1/(thisTransform.position -coll.gameObject.transform.position).magnitude){
							points = 1/(thisTransform.position -coll.gameObject.transform.position).magnitude;
							bestTarget = coll;
						}
					}
				}
			target = bestTarget.GetComponent<Unit>();
			return true;
		}
		return false;
	}
	/// <summary>
	/// Ataca al target con la habilidad designada.
	/// </summary>
	IEnumerator Attack(){
		bool loop = true;//Mantiene el bucle.
		//Debug.Log("Attack");
		while(loop){
			if(target != null && target.thisGameObject.activeInHierarchy){
				if(Vector3.Distance(thisTransform.position,target.thisTransform.position) > skills[0].range - 0.5f){//Mantiene la distancia de ataque
					Utils.LookAt2D(thisTransform,target.thisTransform);
					thisTransform.position = Vector3.MoveTowards (thisTransform.position, target.thisTransform.position, speedAlongPath * Time.deltaTime);
					yield return null;
				}else{
					Utils.LookAt2D(thisTransform,target.thisTransform);
					skills[0].Use(this);
					yield return new WaitForSeconds(skills[0].coolDown);
				}
			}else{
				target = null;
				loop = false;
				yield return new WaitForSeconds (0f);
			}
		}
		state = FSM.States.Idle;
		yield return new WaitForSeconds(0f);
		stateChanger();
	}
		

	public override void Dead ()
	{
		Destroy (thisGameObject);
	}

}
