using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawn : MonoBehaviour {

	//Rate de genes/segundo
	public int geneConsumption;
	//Rate de spawn;
	public float spawnRate;
	//Path que obtendran los creeps;
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


	void Awake(){
		Pool po;

	}

	void Start () {
	
	}
	



}
