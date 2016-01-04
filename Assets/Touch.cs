using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour {

	PathFinding pathfind;
	GameObject  square;
	Grid grid;
	// Use this for initialization
	void Start () {
		pathfind = GetComponent<PathFinding>();
		grid = GetComponent<Grid>();
		square = GameObject.Find("Spawn");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			pathfind.StartFindPath(square.transform.position,Camera.main.ScreenToWorldPoint(Input.mousePosition),callback);
		}
		if(Input.GetMouseButtonDown(1)){
			grid.StartRebuildGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	void callback(Vector3[] paths){
		square.GetComponent<Spawn>().path = paths;
	}
}
