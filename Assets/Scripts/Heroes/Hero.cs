using UnityEngine;
using System.Collections;

public class Hero : Unit {

	[HideInInspector]
	public Vector3[] path;//Path del heroe
	public float speedAlongPath;//Velocidad del heroe a lo largo del path.
	public float rotationSpeed = 5f;//Velocidad de rotacion de heroe.

	void Awake(){
		base.Awake();
		state = FSM.States.Idle;
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
				Utils.LookAt2D(rotationSpeed, thisTransform, currentWayPoint);
				thisTransform.position = Vector3.MoveTowards(thisTransform.position,currentWayPoint,speedAlongPath * Time.fixedDeltaTime);
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
}