using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawn : Unit {

	//Rate de genes/segundo
	public int geneConsumption;
	//Rate de spawn; Creeps/segundo
	public float spawnRate;
	public float spawnRateTier;
	//Path que obtendran los creeps;
	//[HideInInspector]
	public Vector3[] path;
	public Vector3[][] pathSpawnPoints;
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
	public WayPoint actualWayPoint;
	//Auxiliar para guardar el punto de ruta ultimo
	private WayPoint lastWayPoint;
	//Lista de puntos de spawn
	public Vector3[] spawnPoints;
	//Indice del proximo spawn points
	private int nextSP;

	public int loops = 0;
	private PathFinding pathfinder;

	//Gen generado por cada creep de tier 0
	public int geneGain = 100;
	//Porcentaje (de 0 a 1) de creeps asignado a la generacion
	private float geneSpeed;
	//Boolean para saber si se esta ejecutando los invoke de crear tier 0 y generacion
	private bool[] invokeGene = new bool[2];

	/***************************
	 * SOLO PARA LAS PRUEBAS DE LA BARRA DE GENERACION*/
	private SpriteRenderer[] spritesGene = new SpriteRenderer[5];
	/**************************/


	void Start () {
		//Inicializamos el path para evitar errores;
		path = null;
		//Buscamos la pool para solicitar los creeps;
		pool = GameObject.Find ("Pool").GetComponent<Pool> ();
		//Iniciamos la solicitud de creeps basicos;
		invokeGene [0] = true;
		invokeGene [1] = false;
		Invoke("Create",1f/spawnRate);
		//Iniciamos la solicitud de creeps de tier;
		//Invoke("CreateTier",spawnRateTier);
		//Texto para ver el numero de creeps;
		textNumberCreeps = GameObject.Find ("CreepsText/Number").GetComponent<UITest> ();
		wayPoints = new List<WayPoint> ();
		numberCreeps = 0;
		pathfinder = GameObject.Find("GameManager/PathFinder").GetComponent<PathFinding>();
		geneSpeed = 0;
		/***************************
	 	* SOLO PARA LAS PRUEBAS DE LA BARRA DE GENERACION*/
		spritesGene [0] = transform.FindChild ("ProductionBar/Prod_0").GetComponent<SpriteRenderer> ();
		spritesGene [1] = transform.FindChild ("ProductionBar/Prod_1").GetComponent<SpriteRenderer> ();
		spritesGene [2] = transform.FindChild ("ProductionBar/Prod_2").GetComponent<SpriteRenderer> ();
		spritesGene [3] = transform.FindChild ("ProductionBar/Prod_3").GetComponent<SpriteRenderer> ();
		spritesGene [4] = transform.FindChild ("ProductionBar/Prod_4").GetComponent<SpriteRenderer> ();
		/*****************************/
		spawnPoints = new Vector3[8];
		pathSpawnPoints = new Vector3[8][];
		spawnPoints [0] = transform.FindChild ("SpawnPoints/SP01").position;
		spawnPoints [1] = transform.FindChild ("SpawnPoints/SP02").position;
		spawnPoints [2] = transform.FindChild ("SpawnPoints/SP03").position;
		spawnPoints [3] = transform.FindChild ("SpawnPoints/SP04").position;
		spawnPoints [4] = transform.FindChild ("SpawnPoints/SP05").position;
		spawnPoints [5] = transform.FindChild ("SpawnPoints/SP06").position;
		spawnPoints [6] = transform.FindChild ("SpawnPoints/SP07").position;
		spawnPoints [7] = transform.FindChild ("SpawnPoints/SP08").position;
		nextSP = 0;
	}
	void Update(){
		if(loops >= 5){
			Debug.LogError("Loops "+loops);
		}
	}
	/// <summary>
	/// Solicita creeps basicos a la pool.
	/// </summary>
	void Create(){
		
		CreepScript creep = pool.GetCreep (0);
		if (creep != null) {
			//creep.creep.transform.position = spawnPoints[nextSP];
			creep.creep.transform.position = thisTransform.position;

			creep.creep.SetActive (true);

			creep.creepScript.OriginSpawn = this;
			creep.creepScript.spawnPoint = nextSP;
			textNumberCreeps.Add ();
			numberCreeps++;
			nextSP++;
			if (nextSP == 8)
				nextSP = 0;
			if (actualWayPoint != null)
				actualWayPoint.AddCreep ();
		}
		if (geneSpeed < 1)
			Invoke ("Create", 1f / (spawnRate * (1f - geneSpeed)));
		else
			invokeGene [0] = false;
	}

	/// <summary>
	/// Solicita creeps del tier actual a la pool;
	/// </summary>
	void CreateTier(){
		CreepScript creep = pool.GetCreep (tier);
		if (creep != null) {
			creep.creep.transform.position = spawnPoints[nextSP];
			creep.creep.SetActive (true);
			creep.creepScript.OriginSpawn = this;
			creep.creepScript.spawnPoint = nextSP;
			textNumberCreeps.Add ();
			numberCreeps++;
			nextSP++;
			if (nextSP == 8)
				nextSP = 0;
			if (actualWayPoint != null)
				actualWayPoint.AddCreep ();
		}
		Invoke("CreateTier",1f / spawnRateTier);
	}

	/// <summary>
	/// Genera genes en funcion del numero asignado de creeps
	/// </summary>
	void GenerateGen(){
		pool.gene += geneGain;
		if (geneSpeed > 0)
			Invoke ("GenerateGen", 1f / (spawnRate * geneSpeed));
		else
			invokeGene [1] = false;
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
		//Debug.Log ("New Path");
		path = _path;
		actualWayPoint = lastWayPoint;
	}

	/// <summary>
	/// Asigna el path al ultimo punto de ruta
	/// </summary>
	/// <param name="_path">Path.</param>
	public void SetPathSP01(Vector3[] _path){
		//Debug.Log ("New Path SP01");
		pathSpawnPoints[0] = _path;
		actualWayPoint = lastWayPoint;
	}

	public void SetPathSP02(Vector3[] _path){
		//Debug.Log ("New Path SP02");
		pathSpawnPoints[1] = _path;
	}

	public void SetPathSP03(Vector3[] _path){
		//Debug.Log ("New Path SP03");
		pathSpawnPoints[2] = _path;
	}

	public void SetPathSP04(Vector3[] _path){
		//Debug.Log ("New Path SP04");
		pathSpawnPoints[3] = _path;
	}

	public void SetPathSP05(Vector3[] _path){
		//Debug.Log ("New Path SP5");
		pathSpawnPoints[4] = _path;
	}

	public void SetPathSP06(Vector3[] _path){
		//Debug.Log ("New Path SP6");
		pathSpawnPoints[5] = _path;
	}

	public void SetPathSP07(Vector3[] _path){
		//Debug.Log ("New Path SP7");
		pathSpawnPoints[6] = _path;
	}

	public void SetPathSP08(Vector3[] _path){
		//Debug.Log ("New Path SP8");
		pathSpawnPoints[7] = _path;
	}
	/// <summary>
	/// Añade un punto de ruta al spawn
	/// </summary>
	/// <param name="wayPoint">Punto de ruta clase WayPoint</param>
	public void AddWayPoint(WayPoint wayPoint, bool shiftPressed){
		wayPoint.Ini(this);

		//Debug.Log("Mouse Up");
		wayPoints.Add(wayPoint);
		if (!shiftPressed || actualWayPoint == null) {
			lastWayPoint = wayPoint;
			//Debug.Log ("NOSHIFT");
		}
		if (wayPoints.Count > 1) {
			wayPoints[wayPoints.Count - 2].nextWayPoint = wayPoint;
			//Debug.Log("Pos "+ wayPoints[wayPoints.Count - 2].position +" " + wayPoint.position);
			pathfinder.StartFindPath(wayPoints[wayPoints.Count - 2].position,wayPoint.position,wayPoints[wayPoints.Count - 2].SetPath);
		}

	}
		

	/// <summary>
	/// Elimina el primer punto de ruta.
	/// </summary>
	public void RemoveWayPoint(){
		//Destroy (wayPoints [0]);
		wayPoints.RemoveAt (0);
		Debug.Log("RemoveWayPoint Total:" + wayPoints.Count);
	}

	/// <summary>
	/// Cambia la velocidad de generacion de genes
	/// </summary>
	/// <param name="percent">Nuevo porcentaje de generacion</param>
	public void ChangeGeneration(float percent){
		//Debug.Log ("Cambio: " + percent);
		geneSpeed = percent;
		if (!invokeGene[1] && geneSpeed>0) {
			invokeGene [1] = true;
			Invoke ("GenerateGen", 1f/  (spawnRate * geneSpeed));
		} else if (!invokeGene[0] && geneSpeed <1 ) {
			invokeGene [0] = true;
			Invoke("Create",1f/ (spawnRate * (1f-geneSpeed)));
		}
		/************************
		 * SOLO PARA LAS PRUEBAS DE LA BARRA DE GENERACION*/
		float actual = 0.25f;
		for (int i = 1; i < 5; i++) {
			if (actual <= percent) {
				spritesGene [i].color = new Color (0, 1, 0);
			}else
				spritesGene [i].color = new Color (1, 1, 1);
			actual += 0.25f;
		}
	}

}
