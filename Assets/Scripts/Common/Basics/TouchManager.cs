﻿using UnityEngine;
using System.Collections;
using System.Threading;

public class TouchManager : MonoBehaviour {

	Hero hero;
	Spawn Selected = null;
	PathFinding pathfinder;
	Camera camera;
	Grid grid;
	private int spawnPoint;
	Vector3 posSP;
	//Objeto de construccion de spawn
	GameObject buildSpawn;
	//Booleano para saber si esta construyendo
	bool isBuilding = false;

	void Start(){
		hero = GameObject.Find("Hero").GetComponent<Hero>();
		pathfinder = GameObject.Find("GameManager/PathFinder").GetComponent<PathFinding>();
		camera = Camera.main;
		int cores=  System.Environment.ProcessorCount;
		ThreadPool.SetMaxThreads(cores,cores*2);
		Selected = GameObject.Find ("T0Spawn").GetComponent<Spawn> ();
		grid = GameObject.Find("GameManager/PathFinder").GetComponent<Grid>();
		buildSpawn = transform.FindChild ("BuildSpawn").gameObject;
	}

	void FixedUpdate() {
		if (isBuilding) {
			Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
			buildSpawn.transform.position = new Vector3(pos.x,pos.y);
			if (Input.GetMouseButtonUp (0)) {
				buildSpawn.GetComponent<BuildSpawn> ().Build ();
			} else if (Input.GetMouseButtonUp (1)) {
				buildSpawn.SetActive (false);
				isBuilding = false;
			}
		} else {
			if (Input.GetMouseButtonUp (1)) {
				if (Selected != null) {
					Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
					if (Selected.path == null) {
						Debug.Log ("MI PRIMER PATH");
						Selected.AddWayPoint (new WayPoint (pos), false);
						pathfinder.StartFindPath (Selected.thisTransform.position, pos, Selected.SetPath);
						posSP = pos;
						spawnPoint = 0;
						CancelInvoke ();
					} else if (grid.NodeFromWorldPosition (pos).worldPosition != grid.NodeFromWorldPosition (Selected.wayPoints [Selected.wayPoints.Count - 1].position).worldPosition) { 
						bool shiftPressed = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
						Selected.AddWayPoint (new WayPoint (pos), shiftPressed);
						if (!shiftPressed) {
							Debug.Log ("No shift");
							pathfinder.StartFindPath (Selected.thisTransform.position, pos, Selected.SetPath);
							posSP = pos;
							spawnPoint = 0;
							CancelInvoke ();
						}
					} else {
						Debug.Log ("Mismo nodo");
					}
				} else {
					Debug.Log ("No Spawn selected");
				}
			} else if (Input.GetMouseButtonUp (0)) {
				bool shiftPressed = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
				if (shiftPressed) {
					Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
					Pool pool = GameObject.Find ("Pool").GetComponent<Pool> ();
					pos -= new Vector3 (4, 4, 0);
					int count = 0;
					for (int i=0; i < 399; i++){
						CreepScript creep = pool.GetCreep (0);
						if (creep != null) {
							creep.creep.transform.position = new Vector3(pos.x,pos.y);
							creep.creep.SetActive (true);
							creep.creepScript.OriginSpawn = Selected;
							count++;
							if (count == 19) {
								count = 0;
								pos += new Vector3 (-9, 0.5f, 0);
							} else {
								pos += new Vector3 (0.5f, 0, 0);
							}
						}
					}
					return;
				}
				int collsNum = Physics2D.OverlapCircleNonAlloc (camera.ScreenToWorldPoint (Input.mousePosition), 0.01f, new Collider2D[5], 1 << LayerMask.NameToLayer ("UI"));
				if (collsNum < 1) {
					Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
					pathfinder.StartFindPath (hero.thisTransform.position, pos, hero.SetPath);
				}
			}
		}
	}


	public void SelectSpawn(Spawn spawn){
		Debug.Log ("Change Spawn: " + spawn);
		Selected = spawn ;
	}

	public void StartBuildSpawn(){
		buildSpawn.SetActive (true);
		isBuilding = true;
	}
}
