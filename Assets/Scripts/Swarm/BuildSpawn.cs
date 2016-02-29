using UnityEngine;
using System.Collections;

public class BuildSpawn : MonoBehaviour {

	//Prefab del spawn
	public GameObject prefabSpawn;
	//El propio sprite renderer para cambiarle el color
	SpriteRenderer render;

	[Tooltip("Maximo de distancia permitida entre Spawns")]
	public float maxDistSpawn;

	[Tooltip("Minimo de distancia permitida a estructuras enemigas")]
	public float minDistEnemy;

	[Tooltip("Radio de colision con obstaculos")]
	public float radiusCollision;
	//Boolean si puede construir
	private bool canBuild;

	void Awake(){
		render = GetComponent<SpriteRenderer>();
	}
		
	/// <summary>
	/// En el metodo Update se comprueba si se puede construir o no el spawn.
	/// Condiciones para poder construir:
	/// 1- No este tocando ningun obstaculo, esto se calcula usando como radio radiusCollision. Layer = Obstacles.
	///	2- No esta cerca de una estructura enemiga, se calcula usando como radio minDistEnemy. Layer = Human.
	///	3- Esta proximo a otro spawn, se calcula usando como radio maxDistSpawn. Layer = Spawn.
	///	Notas: EL objeto se mueve desde el TouchManager a la posicion del cursor.
	///	       Solo los edificios y los obstaculos tienen collider.
	/// </summary>
	void Update(){
		
		if (Physics2D.OverlapCircle(transform.position, radiusCollision, 1 << LayerMask.NameToLayer("Obstacles")) == null
			&& Physics2D.OverlapCircle(transform.position, minDistEnemy, 1 << LayerMask.NameToLayer("Human")) == null
			&& Physics2D.OverlapCircle(transform.position, maxDistSpawn, 1 << LayerMask.NameToLayer("Spawn")) != null){
			canBuild = true;
			render.color = new Color(0,1,0,0.5f);
		}else{
			canBuild = false;
			render.color = new Color(1,0,0,0.5f);
		}
	}
		
	/// <summary>
	/// Metodo que es llamado cuando se clickea y, en caso de poder, construye el nuevo Spawn
	/// </summary>
	public bool Build(){
		if (canBuild && EconomyManager.gene >= EconomyManager.newSpawnCostGene && EconomyManager.biomatter >= EconomyManager.newSpawnCostBio) {
			Instantiate (prefabSpawn, transform.position, prefabSpawn.transform.rotation);
			EconomyManager.gene -= EconomyManager.newSpawnCostGene;
			EconomyManager.biomatter -= EconomyManager.newSpawnCostBio;
		}
		else
			Debug.Log ("No se puede construir :(");
		return canBuild;
	}
}
