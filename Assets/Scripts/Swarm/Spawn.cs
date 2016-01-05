using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawn : MonoBehaviour {

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

	void Start () {
		path = null;
		Invoke("Create",spawnRate);
		Invoke("CreateTier",spawnRateTier);
		pool = GameObject.Find ("Pool").GetComponent<Pool> ();
	}

	void Create(){

		/*foreach(GameObject tempGO in creepPrefabs){
			GameObject clone = Instantiate(tempGO,new Vector3(transform.position.x + Random.Range(1,5),
			                                                  transform.position.y + Random.Range(1,5),0),Quaternion.identity) as GameObject;
			clone.name = "Creep";
			clone.GetComponent<Creep>().OriginSpawn = this;
			clone.transform.parent = this.transform;
			            
		}*/
		CreepScript creep = pool.GetCreep (0);
		if (creep != null) {
			creep.creep.name = "Creep";
			creep.creep.transform.parent = this.transform;
			creep.creep.transform.position = new Vector3 (transform.position.x + Random.Range (-50, 50),
		                                             transform.position.y + Random.Range (-50, 50));
			creep.creep.SetActive (true);
			creep.creepScript.OriginSpawn = this;
		}
		Invoke("Create",spawnRate);
	}
	
	void CreateTier(){
		CreepScript creep = pool.GetCreep (tier);
		if (creep != null) {
			creep.creep.name = "Creep";

			creep.creep.transform.parent = this.transform;
			creep.creep.transform.position = new Vector3 (transform.position.x + Random.Range (-50, 50),
		                                             transform.position.y + Random.Range (-50, 50));
			creep.creep.SetActive (true);
			creep.creepScript.OriginSpawn = this;
		}
		Invoke("CreateTier",spawnRateTier);
	}


}
