using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Spawn : Unit, IPointerClickHandler {

	//Rate de genes/segundo
	public int geneConsumption;
	//Rate de spawn; Creeps/segundo
	[Tooltip ("Rate Creeps/segundo.")]
	public float spawnRate;
	[Tooltip ("Rate Creeps/segundo.")]
	public float spawnRateTier;
	//Path que obtendran los creeps;
	//[HideInInspector]
	[HideInInspector]
	public Vector3 initPos;
	public Vector3[][] pathSpawnPoints;

	public float sendRate = 1;
	public int sendCuantity = 10;

	//Tier;
	[HideInInspector]
	public int tier = 0;
	//Subtier en el tier actual a instanciar
	[HideInInspector]
	public int subTier = 0;
	//Subtipo, solo vale si el creep esta evolucionado
	[HideInInspector]
	public int subType = -1;
	//Creep de tier actual que se saca 

	[HideInInspector]
	public CreepEvolve actualEvolveCreep;

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

	private PathFinding pathfinder;

	[Tooltip ("Gen que genera un creep de tier 0")]
	public int geneGain = 100;
	//Porcentaje (de 0 a 1) de creeps asignado a la generacion
	private float geneSpeed;
	//Boolean para saber si se esta ejecutando los invoke de crear tier 0 y generacion
	private bool[] invokeGene = new bool[2];

	//Lista de los creeps de tier
	private List<Creep> creepsTier = new List<Creep>();

	private List<Creep> spawnedCreeps = new List<Creep>();

	//Ve si esta con la habilidad del tier activada
	public bool skillTierActive = false;
	/***************************
	 * SOLO PARA LAS PRUEBAS DE LA BARRA DE GENERACION*/
	private SpriteRenderer[] spritesGene = new SpriteRenderer[5];
	/**************************/

	private TouchManager touchManager;

	void Start () {
		//Inicializamos el path para evitar errores;
		initPos = new Vector3(0,0,100000);
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
		touchManager = GameObject.Find ("GameManager/TouchManager").GetComponent<TouchManager> ();
		tier = 0;
		subType = -1;
	}

	/// <summary>
	/// Solicita creeps basicos a la pool.
	/// </summary>
	void Create(){
		
		CreepScript creep = pool.GetCreep (0);
		if (creep != null) {
			//creep.creep.transform.position = spawnPoints[nextSP];
			creep.creep.transform.position = thisTransform.position;
			/*TEMPORAL PARA QUE SE VEAN/*/
			float angle = Random.Range (0, 360);
			creep.creep.transform.position += new Vector3 (Mathf.Cos (angle * Mathf.Deg2Rad) * 2, Mathf.Sin (angle * Mathf.Deg2Rad) * 2, 0);
			/****************************/
			creep.creep.SetActive (true);
			spawnedCreeps.Add(creep.creepScript);
			creep.creepScript.OriginSpawn = this;
			textNumberCreeps.Add ();
			numberCreeps++;
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
		CreepScript creep = pool.GetCreep (tier,subTier,subType);
		if (creep != null) {
			creep.creep.transform.position = thisTransform.position;
			/*TEMPORAL PARA QUE SE VEAN/*/
			float angle = Random.Range (0, 360);
			creep.creep.transform.position += new Vector3 (Mathf.Cos (angle * Mathf.Deg2Rad) * 2, Mathf.Sin (angle * Mathf.Deg2Rad) * 2, 0);
			/****************************/
			creep.creep.SetActive (true);
			spawnedCreeps.Add(creep.creepScript);
			creep.creepScript.OriginSpawn = this;
			textNumberCreeps.Add ();
			if (subType != -1)
				creepsTier.Add (creep.creepScript);
			numberCreeps++;
			if (skillTierActive) {
				actualEvolveCreep.skill.Use (creep.creepScript);
			}
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
	public void CreepDead(Creep creep){
		numberCreeps--;
		//Si no es tier 0 se elimina de la lista de creeps de tier
		if (creep.tier > 0)
			creepsTier.Remove (creep);
	}

	/// <summary>
	/// Asigna el path al ultimo punto de ruta
	/// </summary>
	/// <param name="_path">Path.</param>
	public void SetPath(Vector3 _initPos){
		//Debug.Log ("New Path");
		initPos = _initPos;
		print("Z "+initPos.z);
		if(spawnedCreeps.Count > 0)
			InvokeRepeating("SendCreeps",0,sendRate);
		//actualWayPoint = lastWayPoint;
	}
	void SendCreeps(){
		
		int count;
		if(spawnedCreeps.Count == 0)
			return;
		if(sendCuantity > spawnedCreeps.Count){
			count = spawnedCreeps.Count - 1;
		}else{
			count = sendCuantity;
		}
		for(int i = count; i >= 0;i--){
			if(spawnedCreeps[i] != null){
				spawnedCreeps[i].initPos = initPos;
				spawnedCreeps.RemoveAt(i);
			}
		}
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
			//pathfinder.StartFindPath(wayPoints[wayPoints.Count - 2].position,wayPoint.position,wayPoints[wayPoints.Count - 2].SetPath);
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


	/// <summary>
	/// Evoluciona el spawn al tipo nuevo de creep.
	/// </summary>
	/// <param name="typeCreep">Type creep. Tipo del creep nuevo que se va a generar</param>
	public void EvolveSpawn(int typeCreep){
		CreepEvolve newCreep;
		tier++;
		if (tier == 1)
			newCreep = pool.tier1Evolve [typeCreep];
		else if (tier == 2)
			newCreep = pool.tier2Evolve [typeCreep];
		else
			newCreep = pool.tier3Evolve [typeCreep];
		//cambiamos el tipo del creep
		subTier = typeCreep; 
		//el subtipo se deja a -1 = No evolucionado
		subType = -1;
		//cambiamos el rate 
		spawnRateTier = newCreep.spawnRate;
		//cancelamos la creacion anterior de tier
		CancelInvoke ("CreateTier");
		Invoke ("CreateTier", 1f / spawnRateTier);
		actualEvolveCreep = newCreep;
		//////////////PARA PRUEBAS
		GetComponent<SpriteRenderer> ().color = new Color (0, ((float)tier) / 3f, ((float)tier) / 3f);
	}

	/// <summary>
	/// Evolves the creep.
	/// </summary>
	/// <param name="typeCreep">Type creep.</param>
	public void EvolveCreep(int typeCreep){
		CreepEvolve newCreep;
		if (typeCreep == 0)
			newCreep = actualEvolveCreep.evolveA;
		else
			newCreep = actualEvolveCreep.evolveB;
		//Limpiamos la lista de creeps evolucionados
		creepsTier.Clear();
		//Cambiamos el subtipo o sea la evolucion
		subType = typeCreep;
		//cambiamos el rate 
		spawnRateTier = newCreep.spawnRate;
		//cancelamos la creacion anterior de tier
		CancelInvoke ("CreateTier");
		Invoke ("CreateTier", 1f / spawnRateTier);
		actualEvolveCreep = newCreep;
	}

	/// <summary>
	/// Usa la habilidad de tier del Spawn sobre todos los creeps oportunos
	/// </summary>
	public void UseSkill(){
		if (!skillTierActive) {
			skillTierActive = true;
			foreach (Creep creep in creepsTier) {
				actualEvolveCreep.skill.Use (creep);
			}
			Invoke ("RemoveSkill", actualEvolveCreep.skill.timeBoost);
		}
	}

	/// <summary>
	/// Elimina el efecto de la habilidad del spawn sobre los creeps oportunos
	/// </summary>
	public void RemoveSkill(){
		skillTierActive = false;
		foreach (Creep creep in creepsTier) {
			actualEvolveCreep.skill.Remove (creep);
			creep.GetComponent<SpriteRenderer> ().color = actualEvolveCreep.creep.GetComponent<SpriteRenderer> ().color;
		}
	}

	/// <summary>
	/// Raises the pointer click event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		touchManager.SelectSpawn (this);
	}
}
