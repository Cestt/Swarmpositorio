
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
	[HideInInspector]
	public float speedAlongPath = 1;
	//Camino generado por el PathFinding
	[HideInInspector]
	public Vector3[] path;
	//Punto de la ruta en la que se encuentra
	int targetIndex = 0;
	//Tier
	public int tier = 0;
	//Sub tier
	public int subTier = 0;


	void OnEnable () {
		//Inicializamos el path.
		path = null;
		//Inicializamos al estado principal;
		state = FSM.States.Idle;
		stateChanger();
	}
	void OnDisable() {
		//Re inicializamos el path;
		path = null;
		//Paramos la IA;
		StopAllCoroutines();
		state = FSM.States.Idle;
		OriginSpawn = null;	
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
			}else{
				state = FSM.States.Move;
				stateChanger();
			}
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
			skills[0].Use(this);
		}

	}

	/// <summary>
	/// Solicita un path de manera recursiva
	/// </summary>
	void RequestPath(){

		Debug.Log("Requesting Path");
		if(path != null){
			CancelInvoke();//Para de pedir un path.
			state = FSM.States.Move;
			stateChanger();
		}else{
			Invoke("RequestPath",Random.Range(0.2f,0.7f));
			path = new Vector3[]{new Vector3(5,5,5)};
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
					if(targetIndex >= path.Length -1){
						loop = false;
						path = null;
						targetIndex = 0;
					}
					if(path != null)
						currentWayPoint = path[targetIndex];
				}
				thisTransform.position = Vector3.MoveTowards(thisTransform.position,currentWayPoint,speedAlongPath * Time.fixedDeltaTime);
				Debug.Log("Moving");
				yield return null;
			}
		}
		state = FSM.States.Idle;
		stateChanger();
	}

	/// <summary>
	/// Mantiene al creep buscando enemigos alrededor suya
	/// </summary>
	IEnumerator EnemyDetection(){
		Collider2D[] colls = new Collider2D[25];//Maximo de colliders que detectara alrededor suya.
		float points = -1;//Euristica de puntos para evaluar el mejor objetivo.
		Collider2D bestTarget = null;//Objetivo designado.
		bool loop = true;//Mantiene el bucle.
		while(loop){
			Debug.Log("Checking for enemies");
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
				loop = false;
				target = bestTarget.GetComponent<Unit>();
				state = FSM.States.Attack;
				stateChanger();
			}

			yield return new WaitForSeconds(Random.Range(0.2f,0.6f));
		}

	}

	
	/// <summary>
	/// Ataca al target con la habilidad designada.
	/// </summary>
	IEnumerator Attack(){

	 bool loop = true;//Mantiene el bucle.

		Debug.Log("Attacking");
		while(loop){
			if(target != null){
				if(Vector3.Distance(thisTransform.position,target.thisTransform.position) > skills[0].range - 0.5f){//Mantiene la distancia de ataque.
					thisTransform.position = Vector3.MoveTowards(thisTransform.position,target.thisTransform.position,speedAlongPath * Time.deltaTime);
					yield return null;
				}else{
					skills[0].Use(this);
					yield return new WaitForSeconds(skills[0].coolDown);
				}

			}else{
				loop = false;
			}
		}
		state = FSM.States.Idle;
		stateChanger();
	}





}

