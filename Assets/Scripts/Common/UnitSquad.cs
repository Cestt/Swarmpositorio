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
	[HideInInspector]
	public Squad.squadType tipoUnidad;
	Grid grid;
	PathFinding pathfinder;
	public float detectionRadius;
	public float detectionCreepsRadius;

	void Start(){
		grid = GameObject.Find("GameManager/PathFinder").GetComponent<Grid>();
		Debug.Log ("UQ: " + tipoUnidad);
		if(tipoUnidad == Squad.squadType.Humanos){
			
			StartAgain();
		}else{
			
		}
			
	}

	IEnumerator Move(){
		while(true){
			if(tipoUnidad == Squad.squadType.Swarm){
				if(goTo.z != 10000){
					Utils.LookAt2D(1.2f,thisTransform,thisTransform.position + goTo);
					thisTransform.position = Vector3.MoveTowards(thisTransform.position,thisTransform.position + goTo ,speed * Time.deltaTime);
				}
			}else if(tipoUnidad == Squad.squadType.Humanos){
				if(goTo.z != 10000){
				Utils.LookAt2D(1.2f,thisTransform,goTo);
				thisTransform.position = goTo;
				}
			}

			yield return null; 
		}

	}


	/// <summary>
	/// Mantiene al creep buscando enemigos alrededor suya
	/// </summary>
	IEnumerator EnemyDetection(){
		bool loop = true;//Mantiene el bucle.
		while(loop){
			if (CheckEnemies ()) {
				loop = false;
				squad.StartAttack();
				yield return new WaitForSeconds (Random.Range (0.1f, 0.15f));
			}
			yield return new WaitForSeconds(Random.Range(0.1f,0.15f));
		}
	}

	/// <summary>
	/// Comprueba si esta en rango de vision de los creeps para activarlos
	/// </summary>
	IEnumerator ActiveCreeps(){
		while (true) {
			Creep[] nearCreeps = grid.GetCreepsArea (thisTransform.position, detectionCreepsRadius);
			if (nearCreeps != null) {
				foreach (Creep creep in nearCreeps) {
					if (creep != null && creep.state != FSM.States.Attack) {
						creep.EnemyDetected (this);
					}
				}
			}
			Collider[] spawns = Physics.OverlapSphere (thisTransform.position, detectionRadius, 1 << LayerMask.NameToLayer ("Creep"));
			if (spawns.Length > 0) {
				foreach(Collider col in spawns){
					UnitSquad temp = col.GetComponent<UnitSquad>();
					temp.target = this;
					temp.AttackSwarm();
				}

				return true;
			}
			yield return new WaitForSeconds (Random.Range (0.1f, 0.5f));
		}
	}
	/// <summary>
	/// Chequea si tiene enemigos en su area de vision y devuelve true en caso afirmativo
	/// </summary>
	/// <returns><c>true</c>, if enemies was checked, <c>false</c> otherwise.</returns>
	private bool CheckEnemies(){

		float points = -1;//Euristica de puntos para evaluar el mejor objetivo.
		Unit bestTarget = null;//Objetivo designado.
		Unit[] nearCreeps = grid.GetEnemiesArea (thisTransform.position, detectionRadius);
		if (nearCreeps != null) {
			foreach (Unit enemy in nearCreeps) {
				if (enemy != null) {
					if (points < 1 / (thisTransform.position - enemy.thisTransform.position).magnitude) {
						points = 1 / (thisTransform.position - enemy.thisTransform.position).magnitude;
						bestTarget = enemy;
					}
				}
			}
			target = bestTarget;
			return true;
		}
		Collider[] spawns = Physics.OverlapSphere (thisTransform.position, detectionRadius, 1 << LayerMask.NameToLayer ("Spawn"));
		if (spawns.Length > 0) {
			target = spawns [0].transform.parent.GetComponent<Spawn> ();
			return true;
		}

		return false;
	}

	public void StartAttack(){
		StopAllCoroutines();
		StartCoroutine(Attack());
		StartCoroutine(ActiveCreeps());
	}
	public void StartAgain(){
		StopAllCoroutines();
		StartCoroutine(Move());
		StartCoroutine(ActiveCreeps());
		StartCoroutine(EnemyDetection());
	}

	public void AttackSwarm(){
		StartCoroutine(Attack());
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
					thisTransform.position = Vector3.MoveTowards (thisTransform.position, target.thisTransform.position, speed * Time.deltaTime);
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
		yield return new WaitForSeconds(0f);
		StartAgain();
	}


	public override void Dead ()
	{
		Destroy (thisGameObject);
	}


}
