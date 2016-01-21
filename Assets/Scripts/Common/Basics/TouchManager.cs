using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	Spawn Selected = null;
	PathFinding pathfinder;
	Camera camera;
	public GameObject wayPointPrefab;

	void Start(){
		pathfinder = GameObject.Find("GameManager/PathFinder").GetComponent<PathFinding>();
		camera = Camera.main;
	}

	void FixedUpdate() {
	
		if(Input.GetMouseButtonUp(1)){
			if(Selected != null){
				//GameObject wayPoint = (GameObject)Instantiate (wayPointPrefab, pos, Quaternion.identity);
				//wayPoint.transform.parent = spawnSelected.thisTransform;
				//spawnSelected.AddWayPoint (wayPoint.GetComponent <WayPoint>());
				if (Selected.path == null) {
					Debug.Log ("PRIMER PATH");
					Selected.AddWayPoint (new WayPoint (camera.ScreenToWorldPoint (Input.mousePosition)), false);
					pathfinder.StartFindPath (Selected.thisTransform.position, camera.ScreenToWorldPoint (Input.mousePosition), Selected.SetPath);

				} else {
					bool shiftPressed = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
					Selected.AddWayPoint (new WayPoint (camera.ScreenToWorldPoint (Input.mousePosition)), shiftPressed);
					if (!shiftPressed) {
						pathfinder.StartFindPath (Selected.thisTransform.position, camera.ScreenToWorldPoint (Input.mousePosition), Selected.SetPath);
					}
				}
			}else{
				Debug.Log("No Spawn selected");
			}
		}else if(Input.GetMouseButtonUp(0)){
			//Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
			//pathfinder.StartFindPath(Selected.thisTransform.position,pos,Selected.SetPath);
		}
	}


	public void SelectSpawn(Spawn spawn){
		Selected = spawn ;
	}

}
