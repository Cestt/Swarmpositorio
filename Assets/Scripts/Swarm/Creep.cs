
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Deriva de la clase basica Unit;
//Clase basica de los creeps;
public class Creep : Unit{

	//Spawn de origen
	[HideInInspector]
	public Spawn OriginSpawn;
	//Radio deteccion creep;
	public float detectionRadius;
	//Velocidad de movimiento
	public float speedAlongPath;
	//Camino generado por el PathFinding
	[HideInInspector]
	public Vector3[] path;



	void Start () {
		//Inicializamos al estado principal;
		state = FSM.States.Idle;
		stateChanger();
	}
	



	/// <summary>
	/// Cambia su estado propio dentro de la FSM
	/// </summary>
	void stateChanger(){

		StopCoroutine(EnemyDetection());

		if(state == FSM.States.Idle){
			StartCoroutine(EnemyDetection());
			if(path == null){
				Invoke("RequestPath",Random.Range(0f,0.5f));
			}
		}
		if(state == FSM.States.Move){
			if(path == null || path[path.Length -1] != thisTransform.position){
				StartCoroutine(EnemyDetection());
				StartCoroutine(MoveAlongPath());
			}else{
				state = FSM.States.Idle;
				stateChanger();
			}
		}

	}

	/// <summary>
	/// Solicita un path de manera recursiva
	/// </summary>
	void RequestPath(){


		if(OriginSpawn.path != null & path != OriginSpawn.path){
			CancelInvoke();
			state = FSM.States.Move;
			stateChanger();
		}else{
			Invoke("RequestPath",Random.Range(0f,0.5f));
		}
	}


	/// <summary>
	/// Mueve el creep a lo largo de path.
	/// </summary>
	IEnumerator MoveAlongPath(){
		//Punto de la ruta en la que se encuentra
		int targetIndex = 0;
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
					if(targetIndex <= path.Length -1 & path != null)
						currentWayPoint = path[targetIndex];
				}
				thisTransform.position = Vector3.MoveTowards(thisTransform.position,currentWayPoint,speedAlongPath * Time.fixedDeltaTime);
				yield return null;
			}
		}
		state = FSM.States.Idle;
		stateChanger();
	}

	IEnumerator EnemyDetection(){
		Collider2D[] colls = new Collider2D[25];
		float points = -1;
		Collider2D bestTarget = null;
		while(true){

			int collsNum =  Physics2D.OverlapCircleNonAlloc(thisTransform.position,detectionRadius,colls);
			if(collsNum > 0){
				foreach(Collider2D coll in colls){
					if(coll.tag == "Human"){
						if(points < 2/(thisTransform.position -coll.gameObject.transform.position).magnitude){
							points = 2/(thisTransform.position -coll.gameObject.transform.position).magnitude;
							bestTarget = coll;
						}
					}else if(coll.tag == "Building"){
							if(points < 1/(thisTransform.position -coll.gameObject.transform.position).magnitude){
								points = 1/(thisTransform.position -coll.gameObject.transform.position).magnitude;
								bestTarget = coll;
							}
					}
				}
				target = bestTarget.GetComponent<Unit>();
				state = FSM.States.Attack;
				stateChanger();

			}

			yield return new WaitForSeconds(Random.Range(0.2f,0.6f));
		}
	}



}

