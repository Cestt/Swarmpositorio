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
				Selected.AddWayPoint(new WayPoint(camera.ScreenToWorldPoint (Input.mousePosition)));
				pathfinder.StartFindPath(Selected.thisTransform.position,camera.ScreenToWorldPoint (Input.mousePosition),Selected.SetPath);

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
