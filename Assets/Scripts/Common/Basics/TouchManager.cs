using UnityEngine;
using System.Collections;
using System.Threading;

public class TouchManager : MonoBehaviour {

	Hero hero;
	Spawn Selected = null;
	PathFinding pathfinder;
	Camera camera;
	public GameObject wayPointPrefab;
	Grid grid;
	private int spawnPoint;
	Vector3 posSP;

	void Start(){
		hero = GameObject.Find("Hero").GetComponent<Hero>();
		pathfinder = GameObject.Find("GameManager/PathFinder").GetComponent<PathFinding>();
		camera = Camera.main;
		int cores=  System.Environment.ProcessorCount;
		ThreadPool.SetMaxThreads(cores,cores*2);
		Selected = GameObject.Find ("T0Spawn").GetComponent<Spawn> ();
		grid = GameObject.Find("GameManager/PathFinder").GetComponent<Grid>();
	}

	void FixedUpdate() {
	
		if(Input.GetMouseButtonUp(1)){
			if (Selected != null) {
				Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
				if (Selected.path == null) {
					Debug.Log ("MI PRIMER PATH");
					Selected.AddWayPoint (new WayPoint (pos), false);
					pathfinder.StartFindPath (Selected.thisTransform.position, pos, Selected.SetPath);
					posSP = pos;
					spawnPoint = 0;
					CancelInvoke ();
					Invoke ("PathSpawnPoint", 0);
				} else if (grid.NodeFromWorldPosition (pos).worldPosition != grid.NodeFromWorldPosition (Selected.wayPoints [Selected.wayPoints.Count - 1].position).worldPosition){ 
					bool shiftPressed = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
					Selected.AddWayPoint (new WayPoint (pos), shiftPressed);
					if (!shiftPressed) {
						Debug.Log ("No shift");
						//pathfinder.StartFindPath (Selected.thisTransform.position, pos, Selected.SetPath);
						posSP = pos;
						spawnPoint = 0;
						CancelInvoke ();
						Invoke ("PathSpawnPoint", 0);
					}
			}else{
				Debug.Log("Mismo nodo");
			}
			} else {
				Debug.Log ("No Spawn selected");
			}
		}else if(Input.GetMouseButtonUp(0)){
			
			int collsNum = Physics2D.OverlapCircleNonAlloc (camera.ScreenToWorldPoint (Input.mousePosition), 0.01f, new Collider2D[5], 1 << LayerMask.NameToLayer ("UI"));
			if (collsNum < 1) {
				Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
				pathfinder.StartFindPath (hero.thisTransform.position, pos, hero.SetPath);
			}
		}
	}

	private void PathSpawnPoint(){
		if (spawnPoint == 0)
			pathfinder.StartFindPath (Selected.spawnPoints [0], posSP, Selected.SetPathSP01);
		else if (spawnPoint == 1)
			pathfinder.StartFindPath (Selected.spawnPoints [1], posSP, Selected.SetPathSP02);
		else if (spawnPoint == 2)
			pathfinder.StartFindPath (Selected.spawnPoints [2], posSP, Selected.SetPathSP03);
		else if (spawnPoint == 3)
			pathfinder.StartFindPath (Selected.spawnPoints [3], posSP, Selected.SetPathSP04);
		else if (spawnPoint == 4)
			pathfinder.StartFindPath (Selected.spawnPoints [4], posSP, Selected.SetPathSP05);
		else if (spawnPoint == 5)
			pathfinder.StartFindPath (Selected.spawnPoints [5], posSP, Selected.SetPathSP06);
		else if (spawnPoint == 6)
			pathfinder.StartFindPath (Selected.spawnPoints [6], posSP, Selected.SetPathSP07);
		else if (spawnPoint == 7)
			pathfinder.StartFindPath (Selected.spawnPoints [7], posSP, Selected.SetPathSP08);
		else {
			CancelInvoke ();
			return;
		}
		spawnPoint++;
		Invoke ("PathSpawnPoint", 0.2f);
	}

	public void SelectSpawn(Spawn spawn){
		Debug.Log ("Change Spawn: " + spawn);
		Selected = spawn ;
	}

}
