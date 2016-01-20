using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	Spawn spawnSelected = null;
	PathFinding pathfinder;
	Camera camera;
	public GameObject wayPointPrefab;

	void Start(){
		pathfinder = GameObject.Find("GameManager/PathFinder").GetComponent<PathFinding>();
		camera = Camera.main;
	}

	void FixedUpdate() {
	
		if(Input.GetMouseButtonUp(1)){
			if(spawnSelected != null){
				Vector3 pos = camera.ScreenToWorldPoint (Input.mousePosition);
				//GameObject wayPoint = (GameObject)Instantiate (wayPointPrefab, pos, Quaternion.identity);
				//wayPoint.transform.parent = spawnSelected.thisTransform;
				//spawnSelected.AddWayPoint (wayPoint.GetComponent <WayPoint>());
				spawnSelected.AddWayPoint(new WayPoint(pos));

				Debug.Log ("Raton: " + pos);
				pathfinder.StartFindPath(spawnSelected.thisTransform.position,pos,spawnSelected.SetPath);
			}else{
				Debug.Log("No Spawn selected");
			}
		}
	}


	public void SelectSpawn(Spawn spawn){
		spawnSelected = spawn;
	}
}
