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
	public Node[] path = null;

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

	private PathFinding pathfinder;

	/***GENES CONFIGURACION***/
	[Tooltip ("Gen que genera un creep de tier 0")]
	public int geneGain = 100;
	//Porcentaje (de 0 a 1) de creeps asignado a la generacion
	private float geneSpeed;
	//Boolean para saber si se esta ejecutando los invoke de crear tier 0 y generacion
	private bool[] invokeGene = new bool[2];

	/***BIOMATERIA CONFIGURACION***/
	//Lleva el numero de piscinas creadas
	[HideInInspector]
	public int numBioPools;

	[Tooltip ("Genes que cuesta crear las piscinas. Debe tener el mismo tamaño que el array de producción que se encuentra a continuacion")]
	public int[] costBioPoolGene;
	[Tooltip ("Produccion de biomateria de las piscinas. Se debe poner de forma acumulativa y se mide en Biomateria/segundo." +
		"\nEjemplo: Si tenemos tres piscinas y cada una genera 2 de biomateria por segundo el array es [2,4,6] de tal forma que con tres piscinas se generan 6 por segundo")]
	public int[] biomatterProduction ; //Medido den X/segundo
	//Lista de los creeps de tier
	private List<Creep> creepsTier = new List<Creep>();

	private List<Creep> spawnedCreeps = new List<Creep>();

	/***************************
	 * SOLO PARA LAS PRUEBAS DE LA BARRA DE GENERACION*/
	private SpriteRenderer[] spritesGene = new SpriteRenderer[5];
	/**************************/

	private TouchManager touchManager;
	public int index;

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
		numBioPools = 0;
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
		}
		Invoke("CreateTier",1f / spawnRateTier);
	}

	/// <summary>
	/// Genera genes en funcion del numero asignado de creeps
	/// </summary>
	void GenerateGen(){
		EconomyManager.gene += geneGain;
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
	public void SetPath(Vector3 _initPos,Node[] _path){
		if(path == null){
			path = _path;
		}else{
			Node[] temp = new Node[path.Length + _path.Length];
			path.CopyTo(temp,0);
			_path.CopyTo(temp,path.Length);
			path = temp;
		}
		initPos = _initPos;
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
				spawnedCreeps[i].index = index;
				spawnedCreeps[i].initPos = initPos;
				spawnedCreeps.RemoveAt(i);
			}
		}
	}
		

	/// <summary>
	/// Cambia la velocidad de generacion de genes
	/// </summary>
	/// <param name="percent">Nuevo porcentaje de generacion</param>
	public void ChangeGeneration(float percent){
		//Debug.Log ("Cambio: " + percent);
		if (geneSpeed == 0) {
			CancelInvoke ("Create");
			invokeGene [0] = false;
			geneSpeed = 1;
			spritesGene [2].color = new Color (0, 1, 0);
		} else {
			CancelInvoke ("GenerateGen");
			invokeGene [1] = false;
			geneSpeed = 0;
			spritesGene [2].color = new Color (1, 1, 1);
		}
		//geneSpeed = percent;
		if (!invokeGene[1] && geneSpeed>0) {
			invokeGene [1] = true;
			Invoke ("GenerateGen", 1f/  (spawnRate * geneSpeed));
		} else if (!invokeGene[0] && geneSpeed <1 ) {
			invokeGene [0] = true;
			Invoke("Create",1f/ (spawnRate * (1f-geneSpeed)));
		}


		/*
		float actual = 0.25f;
		for (int i = 1; i < 5; i++) {
			if (actual <= percent) {
				spritesGene [i].color = new Color (0, 1, 0);
			}else
				spritesGene [i].color = new Color (1, 1, 1);
			actual += 0.25f;

		}*/
	}


	/// <summary>
	/// Evoluciona el spawn al tipo nuevo de creep.
	/// </summary>
	/// <param name="typeCreep">Type creep. Tipo del creep nuevo que se va a generar</param>
	public void EvolveSpawn(int typeCreep){
		CreepEvolve newCreep;
		tier++;
		/*if (tier == 1)
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
		EconomyManager.gene -= EconomyManager.GetCreepEvolveCostGene(tier,subTier,subType);
		EconomyManager.biomatter -= EconomyManager.GetCreepEvolveCostBio (tier, subTier, subType);
		spawnRateTier = newCreep.spawnRate;
		//cancelamos la creacion anterior de tier
		CancelInvoke ("CreateTier");
		Invoke ("CreateTier", 1f / spawnRateTier);
		actualEvolveCreep = newCreep;
		//////////////PARA PRUEBAS
		GetComponent<SpriteRenderer> ().color = new Color (0, ((float)tier) / 3f, ((float)tier) / 3f);*/
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
		EconomyManager.gene -= EconomyManager.GetCreepEvolveCostGene(tier,subTier,subType);
		EconomyManager.biomatter -= EconomyManager.GetCreepEvolveCostBio (tier, subTier, subType);
		//cambiamos el rate 
		spawnRateTier = newCreep.spawnRate;
		//cancelamos la creacion anterior de tier
		CancelInvoke ("CreateTier");
		Invoke ("CreateTier", 1f / spawnRateTier);
		actualEvolveCreep = newCreep;
	}


	public void GenerateBiomatter (){
		EconomyManager.biomatter ++;
		Invoke("GenerateBiomatter",1f/(float)biomatterProduction[numBioPools-1]);
	}

	public void CreateCreep(int type){
		GameObject sq = pool.GetCreepSquad (type);
		if (sq != null) {
			sq.gameObject.SetActive (true);
			float angle = Random.Range (0, 360);
			sq.transform.position = thisTransform.position;
			sq.transform.position += new Vector3 (Mathf.Cos (angle * Mathf.Deg2Rad) * 2, Mathf.Sin (angle * Mathf.Deg2Rad) * 2, 0);
		}
	}

	public void AddBioPool(){
		if (numBioPools >= costBioPoolGene.Length || numBioPools >= biomatterProduction.Length ||
		    EconomyManager.gene < costBioPoolGene [numBioPools])
			return;
		EconomyManager.gene -= costBioPoolGene[numBioPools];
		if (numBioPools == 0)
			Invoke("GenerateBiomatter",1f/(float)biomatterProduction[numBioPools]);
		numBioPools++;
	}
	
	/// <summary>
	/// Raises the pointer click event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		touchManager.SelectSpawn (this);
	}

	public override void Dead(){
		if (name == "T0Spawn") {
			Application.Quit ();
		} else
			Destroy (thisGameObject);
	}
}
