
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Battlehub.Dispatcher;

//Deriva de la clase basica Unit;
//Clase basica de los creeps;
public class Creep : Unit{

	//Spawn de origen
	[HideInInspector]
	public Spawn OriginSpawn;
	//Radio deteccion creep;
	public float detectionRadius;
	//Velocidad de movimiento
	public float speedAlongPath = 50;
	//Camino generado por el PathFinding
	[HideInInspector]
	public Vector3[] path;
	//Punto de la ruta en la que se encuentra
	int targetIndex = 0;
	//Tier
	public int tier = 0;
	//Sub tier
	public int subTier = 0;
	//Lista de Creeps cercanos
	public List<EVector2> NearbyAllies = new List<EVector2>();

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
		int numCreep;
		//Paramos la IA;
		state = FSM.States.Idle;
		StopAllCoroutines();
	}
	
	
	
	
	/// <summary>
	/// Cambia su estado propio dentro de la FSM
	/// </summary>
	void stateChanger(){

		StopCoroutine(EnemyDetection());

		if(state == FSM.States.Idle){
			StartCoroutine(EnemyDetection());
			if(path == null){
				StartCoroutine(RequestPath());
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
			StopCoroutine(EnemyDetection());
			StartCoroutine(Attack());
		}

	}

	/// <summary>
	/// Solicita un path de manera recursiva
	/// </summary>
	IEnumerator RequestPath(){
		
		if(path != null){
			CancelInvoke();//Para de pedir un path.
			yield return new WaitForSeconds(Random.Range(0.2f,0.6f));
			state = FSM.States.Move;
			stateChanger();
		}else{
			if(OriginSpawn != null && OriginSpawn.path != null){
				if(path != OriginSpawn.path){
					path = OriginSpawn.path;
					yield return new WaitForSeconds(Random.Range(0.2f,0.6f));
				}
					
			} 
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
		Collider2D[] colls = new Collider2D[25];//Maximo de colliders que detectara alrededor suya.
		float points = -1;//Euristica de puntos para evaluar el mejor objetivo.
		Collider2D bestTarget = null;//Objetivo designado.
		bool loop = true;//Mantiene el bucle.
		while(loop){
			int collsNum =  Physics2D.OverlapCircleNonAlloc(thisTransform.position,detectionRadius,colls,1 << LayerMask.NameToLayer("Obstacles"));
			if(collsNum > 0){
				foreach(Collider2D coll in colls){
					if(coll != null){
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

				}
				loop = false;
				target = bestTarget.GetComponent<Unit>();
				state = FSM.States.Attack;
				yield return new WaitForSeconds(Random.Range(0.6f,0.8f));
				stateChanger();
			}

			yield return new WaitForSeconds(Random.Range(0.6f,0.8f));
		}

	}

	
	/// <summary>
	/// Ataca al target con la habilidad designada.
	/// </summary>
	IEnumerator Attack(){

	 bool loop = true;//Mantiene el bucle.
		
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
				yield return new WaitForSeconds(0);
			}
		}
		state = FSM.States.Idle;
		yield return new WaitForSeconds(0);
		stateChanger();
	}



	IEnumerator CheckSeparation(){
		while(true){
			NearbyAllies.Clear();
			Collider2D[] colls = Physics2D.OverlapCircleAll((Vector2)transform.position,7.5f);
			for(int i = 0; i < colls.Length;i++){
				NearbyAllies.Add(new EVector2(colls[i].transform.position.x,colls[i].transform.position.y));
			}
			if(NearbyAllies.Count >0)
				StartCoroutine(SeparationCalc(NearbyAllies,SeparationResult,new EVector2(thisTransform.position.x,thisTransform.position.y)));
			yield return new WaitForSeconds(0.5f);
		}

		

	}


	IEnumerator SeparationCalc(List<EVector2> allCars, System.Action <EVector2> SeparationCallback,EVector2 position)
	{
		Debug.Log("Separation calc");
		int j = 0;
		EVector2 separationForce = new EVector2(0,0);
		EVector2 averageDirection = new EVector2(0,0);
		EVector2 distance = new EVector2(0,0);
		for (int i = 0; i < allCars.Count - 1; i++)
		{
			distance = position - allCars[i];
			if (Mathf.Sqrt((distance.x * distance.x)+(distance.y * distance.y))  < 5f && allCars[i] != position)
			{
				j++;
				separationForce += position - allCars[i];
				separationForce = EVector2.Normalized(separationForce);
				separationForce = separationForce * (50f);
				averageDirection = averageDirection + separationForce;
			}
		}
		if (j == 0)
		{
			Dispatcher.Current.BeginInvoke(() =>{
				SeparationCallback (new EVector2(0,0));
			});

			yield return null;
		}
		else
		{
				Dispatcher.Current.BeginInvoke(() =>{
					//averageDirection = averageDirection / j;
					SeparationCallback (averageDirection);
			});
			
			yield return null;
		}
	}

	void SeparationResult(EVector2 result){
		Vector3 final = new Vector3(result.x,result.y,0);
		thisTransform.position += final * 10f * Time.deltaTime;
	}

	public override void Dead ()
	{
		state = FSM.States.Idle;
		StopAllCoroutines();
		OriginSpawn = null;	
		GameObject.Find ("CreepsText/Number").GetComponent<NumberCreeps> ().Remove();
		gameObject.SetActive(false);
	}

}

