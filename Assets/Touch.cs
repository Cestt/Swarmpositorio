using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour {

	PathFinding pathfind;
	Hero  square;
	Grid grid;
	GameObject hero;
	// Use this for initialization
	void Start () {
		pathfind = GetComponent<PathFinding>();
		grid = GetComponent<Grid>();
		hero = GameObject.Find("Hero");
		square = hero.GetComponent<Hero>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			square.path = null;
			pathfind.StartFindPath(hero.transform.position,Camera.main.ScreenToWorldPoint(Input.mousePosition),square.StartPath);
		}
		if(Input.GetMouseButtonDown(1)){
			grid.StartRebuildGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	void callback(Vector3[] paths){
		square.GetComponent<Spawn>().path = paths;
	}
}
