using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	Spawn spawnSelected = null;
	PathFinding pathfinder;
	Camera camera;

	void Start(){
		pathfinder = GameObject.Find("GameManager/PathFinder").GetComponent<PathFinding>();
		camera = Camera.main;
	}

	void FixedUpdate() {
	
		if(Input.GetMouseButtonUp(1)){
			if(spawnSelected != null){
				pathfinder.StartFindPath(spawnSelected.thisTransform.position,camera.ScreenToWorldPoint(Input.mousePosition),spawnSelected.SetPath);
			}else{
				Debug.Log("No Spawn selected");
			}
		}
	}


	public void SelectSpawn(Spawn spawn){
		spawnSelected = spawn;
	}
}
