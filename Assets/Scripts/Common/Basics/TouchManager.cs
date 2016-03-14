using UnityEngine;
using System.Collections;
using System.Threading;
using System.Linq;

public class TouchManager : MonoBehaviour {



	public object  selected = null;
	public Spawn hq;
	Hero hero;
	PathFinding pathfinder;
	Camera camera;
	Grid grid;
	HeatMapManager heatmanager;
	private int spawnPoint;
	Vector3 posSP;
	Vector3 pos;
	//Objeto de construccion de spawn
	GameObject buildSpawn;
	//Booleano para saber si esta construyendo
	bool isBuilding = false;


	void Start(){
		pathfinder = GameObject.Find("GameManager/PathFinder").GetComponent<PathFinding>();
		heatmanager = GameObject.Find("GameManager/PathFinder").GetComponent<HeatMapManager>();
		grid = GameObject.Find("GameManager/PathFinder").GetComponent<Grid>();
		hq = GameObject.Find ("T0Spawn").GetComponent<Spawn> ();
		selected = GameObject.Find ("T0Spawn").GetComponent<Spawn> ();
		buildSpawn = transform.FindChild ("BuildSpawn").gameObject;
		camera = Camera.main;

	}

	void FixedUpdate() {


		if (Input.GetMouseButtonUp (0) & !isBuilding) {
			pos = camera.ScreenToWorldPoint (Input.mousePosition);
			pos = new Vector3 (pos.x, pos.y, 0);
			Collider2D[] colls = new Collider2D[5];
			//Comprobacion tocar UI
			int collsNum = Physics2D.OverlapCircleNonAlloc (pos, 0.01f, colls, 1 << LayerMask.NameToLayer ("UI"));
			if(collsNum < 1){
				RaycastHit ray;
				if (Physics.Raycast (camera.ScreenToWorldPoint (Input.mousePosition), new Vector3 (0, 0, 1), out ray)) {
					QuitSelected ();
					int layerMask = ray.collider.gameObject.layer;
					if (layerMask == LayerMask.NameToLayer ("Spawn")) {
						Debug.Log ("Spawn");
						SelectSpawn( ray.collider.GetComponent<Spawn> ());
					} else if (layerMask == LayerMask.NameToLayer ("Creep")) {
						Debug.Log ("Creep");
						selected = ray.collider.GetComponent<UnitSquad> ().squad;
					} else if (layerMask == LayerMask.NameToLayer ("Hero")) {
						Debug.Log ("Hero");
						selected = ray.collider.GetComponent<Hero>();
					}
				} else {
					
					Debug.Log ("Nothing touching");
				}
				//Comprobacion tocar Spawn
				/*Collider[] colls2 = new Collider[5];
				collsNum = Physics.OverlapSphereNonAlloc (pos, 0.01f, colls2, 1 << LayerMask.NameToLayer ("Spawn"));
				if(collsNum < 1){
					//Comprobacion tocar Squad
					collsNum = Physics.OverlapSphereNonAlloc (pos, 0.01f, colls2, 1 << LayerMask.NameToLayer ("Creep"));
					if(collsNum < 1){
						collsNum = Physics.OverlapSphereNonAlloc (pos, 0.01f, colls2, 1 << LayerMask.NameToLayer ("Hero"));
						if(collsNum < 1){
							QuitSelected ();
							selected = null;
						}else{
							selected = colls2[0].GetComponent<Hero>();
						}
					}else{
						selected = colls2[0].GetComponent<UnitSquad>().squad;
					}
				}*/
			}

		
		} else 
			if (Input.GetMouseButtonUp (1) & !isBuilding) {
				

				if (selected != null) {
					pos = camera.ScreenToWorldPoint (Input.mousePosition);
					RaycastHit ray;
					if (Physics.Raycast (camera.ScreenToWorldPoint (Input.mousePosition), new Vector3 (0, 0, 1), out ray)) {
						int layerMask = ray.collider.gameObject.layer;
						if (layerMask == LayerMask.NameToLayer("PowerPoint") && (selected.GetType() == typeof(Squad) || selected.GetType() == typeof(Hero))){
							((Unit)selected).ConquestPowerPoint (ray.collider.GetComponent<PowerPoint> ());
						}
					}
					int collsNum;
					Collider[] colls = new Collider[5];
					if(selected.GetType() == typeof(Spawn)){
						Spawn spawn = (Spawn) selected;


						if (spawn.initPos.z == 100000)  {
							Debug.Log ("MI PRIMER PATH");
							Node node = grid.NodeFromWorldPosition(pos);

							node.heatCost[grid.index] = 0;
							pathfinder.StartFindPathHeat(spawn.thisTransform.position, node, null, spawn.SetPath,grid.index);
							spawn.index = grid.index;

						}else{
							Node node = grid.NodeFromWorldPosition(pos);
							HeatMapUpdater tempUpdater = heatmanager.heatmapsList.First(x => x.index == grid.index);
							tempUpdater.Abort();

							node.heatCost[grid.index] = 0;
							heatmanager.heatmapsList.Remove(tempUpdater);
							grid.con = true;
							pathfinder.StartFindPathHeat(spawn.path[0].worldPosition, node, spawn.path, spawn.SetPath,grid.index);
							spawn.index = grid.index;
							spawn.path = null;
							grid.con = false;
							pathfinder.StartFindPathHeat(spawn.thisTransform.position, node, spawn.path, spawn.SetPath,grid.index);
						}
					}

					else if(selected.GetType() == typeof(Squad)){
						Squad squad = (Squad)selected;
						pathfinder.StartFindPath(squad.transform.position, pos,squad.SetPath);
						
					}

					else if(selected.GetType() ==typeof(Hero)){
						Hero hero = (Hero)selected;
						collsNum = Physics.OverlapSphereNonAlloc(new Vector3(pos.x,pos.y,0), 0.01f, colls,  1 << LayerMask.NameToLayer("Human"));
						if(collsNum < 1){
							pos = camera.ScreenToWorldPoint (Input.mousePosition);
							pathfinder.StartFindPath(hero.thisTransform.position, pos, hero.SetPath);
						}else{
							pathfinder.StartFindPath(hero.thisTransform.position, colls[0].transform.position, hero.SetPathAttack);
							hero.target = colls[0].GetComponent<Unit>();

						}
					}

				} else {
					Debug.Log ("Nada Seleccionado");
				}


			}
	}




	IEnumerator build(){
		yield return null;
		while(isBuilding){
			Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
			buildSpawn.transform.position = new Vector3(pos.x,pos.y);
			if (Input.GetMouseButtonUp (0)) {
				if (buildSpawn.GetComponent<BuildSpawn> ().Build ()) {
					buildSpawn.SetActive (false);
					isBuilding = false;
				}
			} else if (Input.GetMouseButtonUp (1)) {
				buildSpawn.SetActive (false);
				isBuilding = false;
			}
			yield return null;
		}
	}

	public void QuitSelected(){
	//	Debug.Log (selected + "-" + (selected.GetType() == typeof(Spawn)));
		if (selected != null && selected.GetType() == typeof(Spawn)) {
			Spawn spawn = (Spawn)selected;
			spawn.transform.FindChild ("ProductionBar").GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0.3f);
		}
	}

	/// <summary>
	/// Se ha pulsado un nuevo spawn y se cambia el spawn seleccionado.
	/// </summary>
	/// <param name="spawn">Spawn. Spawn que es seleccionado</param>
	public void SelectSpawn(Spawn _spawn){
		QuitSelected ();
		selected = _spawn ;
		_spawn.transform.FindChild ("ProductionBar").GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 0.3f);
	}

	/// <summary>
	/// Se ha puslado el boton para activar el modo construccion de Spawn.
	/// </summary>
	public void StartBuildSpawn(){
		buildSpawn.SetActive (true);
		isBuilding = true;
		StopCoroutine(build());
		StartCoroutine(build());
	}

	/// <summary>
	/// Se ha pulsado el boton para evolucionar el spawn, generando un nuevo tipo de creep.
	/// </summary>
	/// <param name="type">Type. Tipo de creep seleccionado</param>
	public void EvolveSpawn(int type){
		Spawn spawn = (Spawn) selected;
		spawn.EvolveSpawn (type);
	}

	/// <summary>
	/// Se ha pulsado el boton para evolucionar el creep del spawn a un nuevo tipo.
	/// </summary>
	/// <param name="type">Type. Tipo al que evoluciona el creep</param>
	public void EvolveCreep(int type){
		Spawn spawn = (Spawn) selected;
		spawn.EvolveCreep (type);
	}
		
	/// <summary>
	/// Añade una pool de biomateria en el spawn actual
	/// </summary>
	/*public void UseSpawnSkill(){
		Spawn spawn = (Spawn) selected;
		spawn.UseSkill ();

	}
*/
	/// <summary>
	/// Asigna una unidad para que conquiste un power point
	/// </summary>

	public void AddBioPool(){
		Spawn spawn = (Spawn) selected;
		spawn.AddBioPool();
	}
	
	/// <param name="powerPoint">Power point.</param>
	public void UnitToPowerPoint(PowerPoint powerPoint){
		Hero hero = (Hero) selected;
		hero.ConquestPowerPoint (powerPoint);
	}

	/// <summary>
	/// Crea una unidad en el HQ del tipo pasado
	/// </summary>
	/// <param name="type">Tipo de la unidad</param>
	public void CreateUnit(int type){
		hq.CreateCreep (type);
	}

	public void SkillSquad(){
		Squad squad = (Squad) selected;
		squad.UseSkill ();
	}
	public void EvolveSquad(int type){
		Squad squad = (Squad) selected;
		squad.Evolve (type);
	}
}
