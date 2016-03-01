using UnityEngine;
using System.Collections;

public class Hero : Unit {

	[HideInInspector]
	public Vector3[] path;//Path del heroe
	public float speedAlongPath;//Velocidad del heroe a lo largo del path.
	public float rotationSpeed = 5f;//Velocidad de rotacion de heroe.
	Node tempNode;
	Grid grid;
	Node actualNode;
	bool conquered;

	void Awake(){
		base.Awake();
		grid = GameObject.Find("GameManager/PathFinder").GetComponent<Grid>();
		CheckGridPosition();
		state = FSM.States.Idle;
		conquered = false;
	}


	/// <summary>
	/// Callback que recibe el path y lo inicia.
	/// </summary>
	/// <param name="callBackData">Path del heroe.</param>
	public void SetPath(Vector3[] callBackData){
		StopAllCoroutines();
		path = callBackData;
		state = FSM.States.Move;
		stateChanger();

	}

	public void SetPathAttack(Vector3[] callBackData){
		StopAllCoroutines();
		path = callBackData;
		//target = _target;
		state = FSM.States.Attack;
		stateChanger();


	}


	void stateChanger(){
		StopAllCoroutines();
		if (conquered) {
			powerPoint.RemoveUnit ();
			DeletePowerPoint ();
			conquered = false;
		}
		if(state == FSM.States.Idle){
			
		}else
		if(state == FSM.States.Move){
			StartCoroutine(MoveAlongPath());
		}else
		if(state == FSM.States.Attack){
			StartCoroutine(MoveAlongPathAttack());
		}

	}


	/// <summary>
	/// Mueve el heroe a lo largo de path.
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

					}
					//Comprueba si sigue en el path.
					if(targetIndex < path.Length)
						currentWayPoint = path[targetIndex];
				}
				Utils.LookAt2D(rotationSpeed, thisTransform, currentWayPoint);
				thisTransform.position = Vector3.MoveTowards(thisTransform.position,currentWayPoint,speedAlongPath * Time.fixedDeltaTime);
				CheckGridPosition();
				if (powerPoint != null && Vector3.Distance (powerPoint.gameObject.transform.position, thisTransform.position) < 1) {
					powerPoint.SetUnit (this, 1);
					loop = false;
					conquered = true;
				}
				yield return new WaitForEndOfFrame();
			}
			path = null;

		}

	}

	IEnumerator MoveAlongPathAttack(){
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
						StartCoroutine(Attack());
					}
					//Comprueba si sigue en el path.
					if(targetIndex < path.Length)
						currentWayPoint = path[targetIndex];
				}
				if(Vector3.Distance(thisTransform.position,target.thisTransform.position) < skills[0].range - 0.5f){
					StopAllCoroutines();
					StartCoroutine(Attack());
				}
				Utils.LookAt2D(rotationSpeed, thisTransform, currentWayPoint);
				thisTransform.position = Vector3.MoveTowards(thisTransform.position,currentWayPoint,speedAlongPath * Time.fixedDeltaTime);
				CheckGridPosition();

					
				yield return new WaitForEndOfFrame();
			}
			path = null;

		}

	}

	IEnumerator Attack(){
		bool loop = true;//Mantiene el bucle.
		Debug.Log("Attacking");
		while(loop){
			if(target != null){
				if(Vector3.Distance(thisTransform.position,target.thisTransform.position) > skills[0].range - 0.5f){//Mantiene la distancia de ataque.
					thisTransform.position = Vector3.MoveTowards(thisTransform.position,target.thisTransform.position,speedAlongPath * Time.deltaTime);
					Utils.LookAt2D(rotationSpeed, thisTransform, target.thisTransform.position);
					CheckGridPosition();
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

	void CheckGridPosition(){
		tempNode =  grid.NodeFromWorldPosition (thisTransform.position);
		if(actualNode != null){
			if(actualNode != tempNode){
				actualNode.hero = null;
				actualNode = tempNode;
				actualNode.hero = this;
			}

		}else{
			actualNode = tempNode;
			actualNode.hero = this;
		}

	}

	public override void Dead ()
	{
		state = FSM.States.Idle;
		actualNode.hero = null;
		actualNode = null;
		StopAllCoroutines();
		gameObject.SetActive(false);
	}

	public void UseSkill(int numSkill, Vector3 targetPos){
		skills [0].Use (this, targetPos);
	}

	public override void MoveToPosition (Vector3 targetPos, float deltaTime)
	{
		Utils.LookAt2D(rotationSpeed, thisTransform, targetPos);
		thisTransform.position = Vector3.MoveTowards(thisTransform.position,targetPos,speedAlongPath * deltaTime * 2);
		CheckGridPosition();
	}
}