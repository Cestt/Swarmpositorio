using UnityEngine;
using System.Collections;

public class LevelEditor : MonoBehaviour {

	public float pixelPerUnit;
	public Vector2 nativeResolution;
	public Vector2 worldSize;
	public Grid grid;

	void Start () {
		//grid = GameObject.Find("A*Path").GetComponent<Grid>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetOrthographicSize(){
		Camera.main.orthographicSize = (nativeResolution.x / 2)* (1 /(pixelPerUnit));
	}
}
