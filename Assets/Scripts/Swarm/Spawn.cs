using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawn : MonoBehaviour {

	//Rate de genes/segundo
	public int geneConsumption;
	//Rate de spawn;
	public float spawnRate;
	//Path que obtendran los creeps;
	//[HideInInspector]
	public Vector3[] path;
	//Lista de los prefabs de los creeps;
	public List<GameObject> creepPrefabs = new List<GameObject>();
	//Tier Actual a instanciar;
	[HideInInspector]
	public int tier = 0;
	//Numero de creep en el tier actual a instanciar;
	[HideInInspector]
	public int Subtier = 0;
	//Coste genes creep actual;
	[HideInInspector]
	public int coste = 0;



	void Start () {
		path = null;
		Invoke("Create",0.2f);

	}

	void Create(){

		foreach(GameObject tempGO in creepPrefabs){
			GameObject clone = Instantiate(tempGO,new Vector3(transform.position.x + Random.Range(1,5),
			                                                  transform.position.y + Random.Range(1,5),0),Quaternion.identity) as GameObject;
			clone.name = "Creep";
			clone.GetComponent<Creep>().OriginSpawn = this;
			clone.transform.parent = this.transform;
			            
		}


		Invoke("Create",Random.Range(0.2f,0.5f));
	}
	



}
