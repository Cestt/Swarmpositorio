using UnityEngine;
using System.Collections;

[System.Serializable]
public class Building : Unit {

	//Radio deteccion creeps;
	public float detectionCreepsRadius;
	Creep[] nearCreeps;
	private Grid grid;

	void Awake(){
		base.Awake ();
		grid = GameObject.Find("GameManager/PathFinder").GetComponent<Grid>();
		Invoke ("CheckEnemies", Random.Range (0.05f, 0.2f));
	}

	/// <summary>
	/// Chequea si existen creeps en el area para mandarles a atacar a esta estructura
	/// </summary>
	private void CheckEnemies(){
		nearCreeps = grid.GetCreepsArea (thisTransform.position, detectionCreepsRadius);
		if (nearCreeps != null) {
			for(int i = 0;i < nearCreeps.Length - 1; i++) {
				if (nearCreeps[i] != null && nearCreeps[i].gameObject.activeInHierarchy && nearCreeps[i].state != FSM.States.Attack) {
					nearCreeps[i].EnemyDetected (this);
				}
			}
		}
		Invoke ("CheckEnemies", Random.Range (0.05f, 0.2f));
	}

	public override void Dead ()
	{
		Destroy (thisGameObject);
	}
}
