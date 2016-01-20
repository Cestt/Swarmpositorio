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
	//Numero de creeps de este spawn
	private int numberCreeps;
	//Texto con el numero de creeps
	private UITest textNumberCreeps;
	//Lista de los punto de ruta del criadero
	public List<WayPoint> wayPoints;
	//Dice a que punto de ruta marca el spawn de inicio
	WayPoint actualWayPoint;

	void Start () {
		//Inicializamos el path para evitar errores;
		path = null;
		//Buscamos la pool para solicitar los creeps;
		pool = GameObject.Find ("Pool").GetComponent<Pool> ();
		//Iniciamos la solicitud de creeps basicos;
		Invoke("Create",spawnRate);
		//Iniciamos la solicitud de creeps de tier;
		//Invoke("CreateTier",spawnRateTier);
		//Texto para ver el numero de creeps;
		textNumberCreeps = GameObject.Find ("CreepsText/Number").GetComponent<UITest> ();
		wayPoints = new List<WayPoint> ();
		numberCreeps = 0;
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
			textNumberCreeps.Add ();
			numberCreeps++;
			if (actualWayPoint != null)
				actualWayPoint.AddCreep ();
		}
		//Invoke("Create",spawnRate);
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
			textNumberCreeps.Add ();
			numberCreeps++;
			if (actualWayPoint != null)
				actualWayPoint.AddCreep ();
		}
		Invoke("CreateTier",spawnRateTier);
	}

	/// <summary>
	/// Es llamado cuando un creep muere
	/// </summary>
	public void CreepDead(){
		numberCreeps--;
	}

	/// <summary>
	/// Asigna el path al ultimo punto de ruta
	/// </summary>
	/// <param name="_path">Path.</param>
	public void SetPath(Vector3[] _path){
		path = _path;
		actualWayPoint = wayPoints [wayPoints.Count - 1];
	}

	/// <summary>
	/// Añade un punto de ruta al spawn
	/// </summary>
	/// <param name="wayPoint">Punto de ruta clase WayPoint</param>
	public void AddWayPoint(WayPoint wayPoint){
		wayPoint.Ini (this, numberCreeps);
		wayPoints.Add (wayPoint);
		if (wayPoints.Count > 1) {
			Debug.Log ("NUM WP: " + (wayPoints.Count - 2));
			wayPoints [wayPoints.Count - 2].NewPath (wayPoint);
		}
	}
		

	/// <summary>
	/// Elimina el primer punto de ruta.
	/// </summary>
	public void RemoveWayPoint(){
		//Destroy (wayPoints [0]);
		wayPoints.RemoveAt (0);
	}

}
