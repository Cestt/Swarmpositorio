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

				} else if (grid.NodeFromWorldPosition (pos).worldPosition != grid.NodeFromWorldPosition (Selected.wayPoints [Selected.wayPoints.Count - 1].position).worldPosition){ 
					bool shiftPressed = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
					Selected.AddWayPoint (new WayPoint (pos), shiftPressed);
					if (!shiftPressed) {
						Debug.Log ("No shift");
						pathfinder.StartFindPath (Selected.thisTransform.position, pos, Selected.SetPath);
					}
			}else{
				Debug.Log("Mismo nodo");
			}
			} else {
				Debug.Log ("No Spawn selected");
			}
		}else if(Input.GetMouseButtonUp(0)){
			Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
			pathfinder.StartFindPath(hero.thisTransform.position,pos,hero.SetPath);
		}
	}


	public void SelectSpawn(Spawn spawn){
		Selected = spawn ;
	}

}
