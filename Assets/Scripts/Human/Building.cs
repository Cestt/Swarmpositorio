using UnityEngine;
using System.Collections;

[System.Serializable]
public class Building : Unit {

	//Radio deteccion creeps;
	public float detectionCreepsRadius;

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
		Creep[] nearCreeps = grid.GetCreepsArea (thisTransform.position, detectionCreepsRadius);
		if (nearCreeps != null) {
			foreach (Creep creep in nearCreeps) {
				if (creep != null && creep.state != FSM.States.Attack) {
					creep.EnemyDetected (this);
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
