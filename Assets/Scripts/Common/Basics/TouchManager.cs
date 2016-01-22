using UnityEngine;
using System.Collections;
using System.Threading;
public class TouchManager : MonoBehaviour {

	Spawn Selected = null;
	PathFinding pathfinder;
	Camera camera;
	Hero hero;
	public GameObject wayPointPrefab;

	void Start(){
		pathfinder = GameObject.Find("GameManager/PathFinder").GetComponent<PathFinding>();
		Selected = GameObject.Find("T0Spawn").GetComponent<Spawn>();
		camera = Camera.main;
		hero = GameObject.Find("Hero").GetComponent<Hero>();
		int a;
		int b;
	}

	void Update() {
	
		if(Input.GetMouseButtonUp(1)){
			if(Selected != null){
				Vector3 pos  = camera.ScreenToWorldPoint (Input.mousePosition);
				if (Selected.path == null) {
					Debug.Log ("MI PRIMER PATH");
					Selected.AddWayPoint (new WayPoint (pos), false);
					pathfinder.StartFindPath (Selected.thisTransform.position, pos, Selected.SetPath);

				} else {
					bool shiftPressed = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
					Selected.AddWayPoint (new WayPoint (pos), shiftPressed);
					if (!shiftPressed) {
						pathfinder.StartFindPath (Selected.thisTransform.position, pos, Selected.SetPath);
					}
				}
			}else{
				Debug.Log("No Spawn selected");
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
