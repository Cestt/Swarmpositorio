using UnityEngine;
using System.Collections;
using System.Threading;
using System.Linq;

public class TouchManager : MonoBehaviour {

<<<<<<< HEAD

	public object  selected = null;
	public Spawn hq;
	public Squad selectedSquad;
=======
	Hero hero;
	public Spawn selected = null;
	public Spawn hq;
>>>>>>> origin/master
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
<<<<<<< HEAD
		hq = GameObject.Find ("T0Spawn").GetComponent<Spawn> ();
=======
		selected = GameObject.Find ("T0Spawn").GetComponent<Spawn> ();
		hq = selected;
		selected.transform.FindChild ("ProductionBar").GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 0.3f);
>>>>>>> origin/master
		buildSpawn = transform.FindChild ("BuildSpawn").gameObject;
		camera = Camera.main;

	}

	void FixedUpdate() {
<<<<<<< HEAD


		if (Input.GetMouseButtonUp (0) & !isBuilding) {
			pos = camera.ScreenToWorldPoint (Input.mousePosition);
			Collider[] colls = new Collider[5];
			//Comprobacion tocar UI
			int collsNum = Physics2D.OverlapCircleNonAlloc (pos, 0.01f, colls, 1 << LayerMask.NameToLayer ("UI"));
			if(collsNum < 1){
				//Comprobacion tocar Spawn
				collsNum = Physics.OverlapSphereNonAlloc (pos, 0.01f, colls, 1 << LayerMask.NameToLayer ("Spawn"));
				if(collsNum < 1){
					//Comprobacion tocar Squad
					collsNum = Physics2D.OverlapCircleNonAlloc (pos, 0.01f, colls, 1 << LayerMask.NameToLayer ("Squad"));
					if(collsNum < 1){
						collsNum = Physics2D.OverlapCircleNonAlloc (pos, 0.01f, colls, 1 << LayerMask.NameToLayer ("Hero"));
						if(collsNum < 1){
							selected = null;
						}else{
							selected = colls[0].GetComponent<Hero>();
						}
					}else{
						selected = colls[0].GetComponent<UnitSquad>().squad;
					}
				}
			}

		
		} else 
			if (Input.GetMouseButtonUp (1) & !isBuilding) {

				if (selected != null) {
					pos = camera.ScreenToWorldPoint (Input.mousePosition);
					int collsNum;
					Collider[] colls = new Collider[5];
					if(selected.Equals(typeof(Spawn))){
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

					else if(selected.Equals(typeof(Squad))){
						Squad squad = (Squad)selected;
						pathfinder.StartFindPath(squad.transform.position, pos,squad.SetPath);
						
					}

					else if(selected.Equals(typeof(Hero))){
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

=======
		if (Input.GetMouseButtonUp (1)) {
			if (selected != null) {
				pos = camera.ScreenToWorldPoint (Input.mousePosition);
				if (selected.initPos.z == 100000)  {
					Debug.Log ("MI PRIMER PATH");
					//Selected.AddWayPoint (new WayPoint (pos), false);

					Node node = grid.NodeFromWorldPosition(pos);
					node.heatCost[grid.index] = 0;
					HeatMapUpdater tempUpdater = heatmanager.heatmapsList.First(x => x.index == grid.index);
					tempUpdater.Abort();
					heatmanager.heatmapsList.Remove(tempUpdater);
					grid.con = true;
					pathfinder.StartFindPathHeat(selected.path[0].worldPosition, node, selected.path, selected.SetPath,grid.index);
					grid.index++;

					selected.index = grid.index;
					node.heatCost.Add(grid.index,0);
					selected.path = null;
					grid.con = false;
					pathfinder.StartFindPathHeat(selected.thisTransform.position, node, selected.path, selected.SetPath,grid.index);
					posSP = pos;
					spawnPoint = 0;
				}else{
					
				}
			} else {
				Debug.Log ("Mismo nodo");
			}
		} 
		else if (Input.GetMouseButtonUp (0) & !isBuilding && hero.state != FSM.States.Charge) {
			
			if (Input.GetKey (KeyCode.Q)) {
				Debug.Log ("CHARGE");
				Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
				hero.UseSkill (0, new Vector3(pos.x,pos.y,0));
				return;
			}
			bool shiftPressed = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
			int collsNum = Physics2D.OverlapCircleNonAlloc (camera.ScreenToWorldPoint (Input.mousePosition), 0.01f, new Collider2D[5], 1 << LayerMask.NameToLayer ("UI"));
			if (collsNum < 1) {
				Collider[] colls = new Collider[5];
				pos = camera.ScreenToWorldPoint (Input.mousePosition);
				collsNum = Physics.OverlapSphereNonAlloc(new Vector3(pos.x,pos.y,0), 0.01f, colls,  1 << LayerMask.NameToLayer("Human"));
				if(collsNum < 1){
					pos = camera.ScreenToWorldPoint (Input.mousePosition);
					pathfinder.StartFindPath(hero.thisTransform.position, pos, hero.SetPath);
				}else{
					pathfinder.StartFindPath(hero.thisTransform.position, colls[0].transform.position, hero.SetPathAttack);
					hero.target = colls[0].GetComponent<Unit>();
				
			}
			
>>>>>>> origin/master
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

	/// <summary>
	/// Se ha pulsado un nuevo spawn y se cambia el spawn seleccionado.
	/// </summary>
	/// <param name="spawn">Spawn. Spawn que es seleccionado</param>
	public void SelectSpawn(Spawn spawn){
		selected.transform.FindChild ("ProductionBar").GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0.3f);
		Debug.Log ("Change Spawn: " + spawn);
		selected = spawn ;
		selected.transform.FindChild ("ProductionBar").GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 0.3f);
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
<<<<<<< HEAD
	public void UseSpawnSkill(){
		Spawn spawn = (Spawn) selected;
		spawn.UseSkill ();
=======
	public void AddBioPool(){
		selected.AddBioPool();
>>>>>>> origin/master
	}

	/// <summary>
	/// Asigna una unidad para que conquiste un power point
	/// </summary>
<<<<<<< HEAD
	public void AddBioPool(){
		Spawn spawn = (Spawn) selected;
		spawn.AddBioPool();
	}
	public void UnitToPowerPoint(PowerPoint powerPoint){
		Hero hero = (Hero) selected;
=======
	/// <param name="powerPoint">Power point.</param>
	public void UnitToPowerPoint(PowerPoint powerPoint){
>>>>>>> origin/master
		hero.ConquestPowerPoint (powerPoint);
	}

	/// <summary>
	/// Crea una unidad en el HQ del tipo pasado
	/// </summary>
	/// <param name="type">Tipo de la unidad</param>
	public void CreateUnit(int type){
<<<<<<< HEAD
		hq.CreateCreep (type);
	}

	public void SkillSquad(){
		Squad squad = (Squad) selected;
		squad.UseSkill ();
	}
	public void EvolveSquad(int type){
		Squad squad = (Squad) selected;
		squad.Evolve (type);
=======

>>>>>>> origin/master
	}
}
