
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Battlehub.Dispatcher;
using CielaSpike;

//Deriva de la clase basica Unit;
//Clase basica de los creeps;
public class Creep : Unit{
	Grid grid;
	//Spawn de origen
	[HideInInspector]
	public Spawn OriginSpawn;
	//Radio deteccion creep;
	public float detectionRadius;
	//Velocidad de movimiento
	public float speedAlongPath = 50;
	//Camino generado por el PathFinding
	[HideInInspector]
	public Vector3 initPos;
	//Punto de la ruta en la que se encuentra
	int targetIndex = 0;
	//Tier
	public int tier = 0;
	//Lista de Creeps cercanos
	public List<EVector2> NearbyAllies = new List<EVector2>();
	//Task para manejar las coRutinas en los hilos.
	Task task;
	//Coste de genes del creep
	public int costGene;
	//Dice si ha llegado al destino
	bool arrive = false;
	//Punto de ruta al que se dirije
	public WayPoint wayPoint;

	public int position;

	public int index;
	bool loop =  true;
	static Vector3 lame = new Vector3(0,0,0);
	Node tempNode;

	Node node = null;

	void Awake(){
		base.Awake ();
		grid = GameObject.Find("GameManager/PathFinder").GetComponent<Grid>();
		initPos = new Vector3(0,0,100000);
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable () {
		//Iniciar separacion de los creeps;
		//this.StartCoroutineAsync(CheckSeparation());
		//Inicializamos el path.
		initPos = new Vector3(0,0,100000);;
		//Inicializamos al estado principal;
		state = FSM.States.Idle;
		CheckGridPosition();
		life = lifeIni;
		stateChanger();
		
	}

	void OnDisable() {	
		//Re inicializamos el path;
		initPos = new Vector3(0,0,100000);
		int numCreep;
		//Paramos la IA;
		state = FSM.States.Idle;
		StopAllCoroutines();
	}

	


	/// <summary>
	/// Cambia su estado propio dentro de la FSM
	/// </summary>
	void stateChanger(){

		//StopCoroutine(EnemyDetection());

		//Debug.Log (name + "" + arrive);
		if(state == FSM.States.Idle){
			//StartCoroutine(EnemyDetection());
			if(initPos.z == 100000){
				initPos = new Vector3(0,0,100000);
				InvokeRepeating("RequestPath",0,Random.Range(0.1f,0.4f));
				if (!arrive) {
					
				} /*else {
					initPos = new Vector3(0,0,100000);
					InvokeRepeating("RequestPathWayPoint",0,Random.Range(0.1f,2f));
				}*/
			}else{
				state = FSM.States.Move;
				index = grid.index;
				stateChanger();
			}
		}else
		if(state == FSM.States.Move){
			if(initPos.z != 100000){
					if (initPos  != thisTransform.position) {
						//StartCoroutine (EnemyDetection ());
						StartCoroutine (MoveAlongPath ());
					}
			}else{
				state = FSM.States.Idle;
				stateChanger();
			}
		}else
		if(state == FSM.States.Attack){
			//StopCoroutine(EnemyDetection());
			StartCoroutine(Attack());
		}

	}

	/// <summary>
	/// Solicita un path de manera recursiva
	/// </summary>
	void RequestPath(){
		
		if(OriginSpawn != null){
			if(initPos.z != 100000){
				//wayPoint = OriginSpawn.actualWayPoint;
				//wayPoint.AddCreep ();	
				state = FSM.States.Move;
				CancelInvoke("RequestPath");
				stateChanger();
			}
		} 

	}

	/// <summary>
	/// Solicita el path al punto de ruta en el que esta
	/// </summary>
	/// 
	void RequestPathWayPoint(){
		if (wayPoint != null) {
			if (wayPoint.path != null) {
				//initPos = wayPoint.path;
				WayPoint nextWP = wayPoint.nextWayPoint;
				wayPoint.RemoveCreep ();
				wayPoint = nextWP;
				nextWP.AddCreep ();
				state = FSM.States.Move;
				CancelInvoke("RequestPathWayPoint");
				stateChanger();
			}
		}
	}
	/*IEnumerator RequestPathWayPoint(){
		if(path != null){
			yield return new WaitForSeconds(Random.Range(0.2f,0.6f));
			state = FSM.States.Move;
			stateChanger();

		}else{
			if(wayPoint != null){
				if(wayPoint.path != null){
					path = wayPoint.path;
					WayPoint nextWP = wayPoint.nextWayPoint;
					wayPoint.RemoveCreep();
					wayPoint = nextWP;
					nextWP.AddCreep ();
					yield return new WaitForSeconds(Random.Range(0.2f,0.6f));
					StartCoroutine(RequestPathWayPoint());

				}else{
					yield return new WaitForSeconds(Random.Range(0.4f,0.8f));
					StartCoroutine(RequestPathWayPoint());
				}
			} 
		}
	}*/
	/// <summary>
	/// Mueve el creep a lo largo de path.
	/// </summary>
	IEnumerator MoveAlongPath(){
		if(initPos != null){
			Vector3 currentWayPoint = initPos;
			Vector3 dir;
			//Mantiene el bucle de movimiento.

			while(loop){
				tempNode = grid.NodeFromWorldPosition (thisTransform.position);

				if(tempNode.dir != lame){
					dir = tempNode.dir;
					Utils.LookAt2D (10f, thisTransform, thisTransform.position + dir);
					thisTransform.position = Vector3.MoveTowards (thisTransform.position,thisTransform.position + dir, speedAlongPath * Time.deltaTime / 10);
					if(tempNode.heatCost[index] + tempNode.creeps.Count < 2){
						loop = false;
					}

				}else{
					Utils.LookAt2D (10f, thisTransform, initPos);
					thisTransform.position = Vector3.MoveTowards (thisTransform.position, currentWayPoint, speedAlongPath * Time.deltaTime / 10);

				}
				CheckGridPosition ();
				yield return new WaitForSeconds(Random.Range(0.1f,0.25f));			}
		}
		state = FSM.States.Idle;
		yield return new WaitForSeconds(Random.Range(0.2f,2.0f));
		//stateChanger();
	}



	void CheckGridPosition(){
		tempNode =  grid.NodeFromWorldPosition (thisTransform.position);
		if(node != null){
			if(node != tempNode){
				node.creeps.Remove(this);
				node = tempNode;
				node.creeps.Add(this);
			}

		}else{
			node = tempNode;
			node.creeps.Add(this);
		}

	}

	/// <summary>
	/// Mantiene al creep buscando enemigos alrededor suya
	/// </summary>
	IEnumerator EnemyDetection(){
		Collider2D bestTarget = null;//Objetivo designado.
		Collider2D[] colls = new Collider2D[25];//Maximo de colliders que detectara alrededor suya.
		float points = -1;//Euristica de puntos para evaluar el mejor objetivo.
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
	/// Activa el creep y lo asigna a atacar a la unidad
	/// </summary>
	public void EnemyDetected(Unit enemy){
		//Debug.Log ("NUEVO ENEMIGO");
		CancelInvoke();
		if (state == FSM.States.Move)
			StopAllCoroutines ();
		target = enemy;
		state = FSM.States.Attack;
		stateChanger();
	}
	
	/// <summary>
	/// Ataca al target con la habilidad designada.
	/// </summary>
	IEnumerator Attack(){
	 bool loop = true;//Mantiene el bucle.
		
		while(loop){
			if(target != null){
				if(Vector3.Distance(thisTransform.position,target.thisTransform.position) > skills[0].range - 0.5f){//Mantiene la distancia de ataque.
					thisTransform.position = Vector3.MoveTowards(thisTransform.position,target.thisTransform.position,speedAlongPath * Time.deltaTime / 10);
					Utils.LookAt2D (thisTransform, target.thisTransform);
					CheckGridPosition ();
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
			yield return Ninja.JumpToUnity;
			Collider2D[] colls = new Collider2D[11];
			int numColls = Physics2D.OverlapCircleNonAlloc(thisTransform.position,detectionRadius,colls);
			if(colls.Length > 10){
				NearbyAllies.Clear();
				for(int i = 0; i < colls.Length;i++){
					if(colls[i] != null)
						NearbyAllies.Add(new EVector2(colls[i].transform.position.x,colls[i].transform.position.y));
				}

					EVector2 tempEvector2 = new EVector2(thisTransform.position.x,thisTransform.position.y);
					this.StartCoroutineAsync(SeparationCalc(NearbyAllies,SeparationResult,tempEvector2));
					yield return Ninja.JumpBack;
			}
				System.Random rnd = new System.Random();
				int temp = rnd.Next(30,60);
				yield return new WaitForSeconds(temp/100f);
			}
		}




	IEnumerator SeparationCalc(List<EVector2> allCreeps, System.Action <EVector2> SeparationCallback,EVector2 position)
	{
		int j = 0;
		EVector2 separationForce = new EVector2(0,0);
		EVector2 averageDirection = new EVector2(0,0);
		EVector2 distance = new EVector2(0,0);
		if(allCreeps != null){
			for (int i = 0; i < allCreeps.Count - 1; i++)
			{
				if(allCreeps[i] != null){
					distance = position - allCreeps[i];
					if (Mathf.Sqrt((distance.x * distance.x)+(distance.y * distance.y))  < 0.5f && allCreeps[i] != position)
					{
						j++;
						separationForce += position - allCreeps[i];
						separationForce = EVector2.Normalized(separationForce);
						separationForce = separationForce * (4f);
						averageDirection = averageDirection + separationForce;
					}
				}

			}
		}

		if (j == 0)
		{
			yield return null;
		}
		else
		{
		  	averageDirection = averageDirection / j;
			yield return Ninja.JumpToUnity;
			SeparationCallback (averageDirection);
		
			
			yield return null;
		}
	}

	void SeparationResult(EVector2 result){
		Vector3 final = new Vector3(result.x,result.y,0);
		thisTransform.position += final * 1f * Time.deltaTime;
	}

	public override void Dead ()
	{
		state = FSM.States.Idle;
		node.creeps.Remove(this);
		node = null;
		StopAllCoroutines();
		OriginSpawn.CreepDead (this);
		OriginSpawn = null;	
		GameObject.Find ("CreepsText/Number").GetComponent<UITest> ().Remove();
		gameObject.SetActive(false);
	}

}

