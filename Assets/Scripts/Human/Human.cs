using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : Unit {

	private float targetDistanceIni;

	void Awake () {
		base.Awake ();
		if (canAttack)
			StartCoroutine (EnemyDetection ());
	}


	/// <summary>
	/// Mantiene al humano buscando enemigos alrededor suya
	/// </summary>
	IEnumerator EnemyDetection(){
		StopCoroutine (Attack ());
		Collider2D[] colls = new Collider2D[25];//Maximo de colliders que detectara alrededor suya.
		float points = -1;//Euristica de puntos para evaluar el mejor objetivo.
		Collider2D bestTarget = null;//Objetivo designado.
		bool loop = true;//Mantiene el bucle.
		Debug.Log("Checking for enemies");
		while(loop){
			int collsNum =  Physics2D.OverlapCircleNonAlloc(thisTransform.position,skills[0].range,colls, 1 << LayerMask.NameToLayer("Creep"));
			if(collsNum > 0){
				points = (thisTransform.position - colls[0].transform.position).magnitude;
				bestTarget = colls[0];
				for (int i = 1; i < colls.Length && colls[i] != null; i++){
					if(points < (thisTransform.position -colls[i].transform.position).magnitude){
						points = (thisTransform.position -colls[i].transform.position).magnitude;
						bestTarget = colls[i];
					}
				}
				loop = false;
				targetDistanceIni = points;

				target = bestTarget.GetComponent<Unit>();
				StartCoroutine(Attack());
				state = FSM.States.Attack;
			}else{
				yield return new WaitForSeconds(Random.Range(0.1f,0.2f));
			}
		}
	}

	/// <summary>
	/// Ataca al target con la habilidad designada.
	/// </summary>
	IEnumerator Attack(){
		StopCoroutine (EnemyDetection ());
		bool loop = true;//Mantiene el bucle.
		Debug.Log("Attacking");
		while(loop){
			if(target != null && Vector2.Distance(thisTransform.position, target.thisTransform.position) <= targetDistanceIni){
				Vector3 dir = target.thisTransform.position - thisTransform.position;
				float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				angle += 90;
				thisTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				skills[0].Use(this);
					yield return new WaitForSeconds(skills[0].coolDown);
			}else{
				loop = false;
				yield return new WaitForSeconds(0.2f);
				StartCoroutine(EnemyDetection());
			}
		}
		state = FSM.States.Idle;
	}

	public override void Dead ()
	{
		Destroy (thisGameObject);
	}
}
