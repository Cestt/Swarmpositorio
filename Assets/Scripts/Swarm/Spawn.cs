using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawn : Unit {

	//Rate de genes/segundo
	public int geneConsumption;
	//Rate de spawn;
	public float spawnRate;
	public float spawnRateTier;
	//Path que obtendran los creeps;
	//[HideInInspector]
	public Vector3[] path;
	//Lista de los prefabs de los creeps;
	public List<GameObject> creepPrefabs = new List<GameObject>();
	//Tier Actual a instanciar;
	[HideInInspector]
	public int tier = 1;
	//Numero de creep en el tier actual a instanciar;
	[HideInInspector]
	public int Subtier = 0;
	//Coste genes creep actual;
	[HideInInspector]
	public int coste = 0;
	//Pool de creeps
	private Pool pool;
	//Texto con el numero de creeps
	private NumberCreeps numberCreeps;

	void Start () {
		//Inicializamos el path para evitar errores;
		path = null;
		//Buscamos la pool para solicitar los creeps;
		pool = GameObject.Find ("Pool").GetComponent<Pool> ();
		//Iniciamos la solicitud de creeps basicos;
		Invoke("Create",spawnRate);
		//Iniciamos la solicitud de creeps de tier;
		Invoke("CreateTier",spawnRateTier);
		//Texto para ver el numero de creeps;
		numberCreeps = GameObject.Find ("CreepsText/Number").GetComponent<NumberCreeps> ();
	}

	/// <summary>
	/// Solicita creeps basicos a la pool.
	/// </summary>
	void Create(){

		CreepScript creep = pool.GetCreep (0);
		if (creep != null) {
			creep.creep.transform.position = new Vector3 (transform.position.x + Random.Range (-50, 50),
		                                             transform.position.y + Random.Range (-50, 50));
			creep.creep.SetActive (true);
			creep.creepScript.OriginSpawn = this;
			numberCreeps.Add ();
		}
		Invoke("Create",spawnRate);
	}

	/// <summary>
	/// Solicita creeps del tier actual a la pool;
	/// </summary>
	void CreateTier(){
		CreepScript creep = pool.GetCreep (tier);
		if (creep != null) {
			creep.creep.transform.position = new Vector3 (transform.position.x + Random.Range (-50, 50),
		                                             transform.position.y + Random.Range (-50, 50));
			creep.creep.SetActive (true);
			creep.creepScript.OriginSpawn = this;
			numberCreeps.Add ();
		}
		Invoke("CreateTier",spawnRateTier);
	}

	public void SetPath(Vector3[] _path){
		path = _path;
	}


}
