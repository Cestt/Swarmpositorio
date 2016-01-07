using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour {

	PathFinding pathfind;
	Sprite  square;
	Grid grid;
	GameObject hero;
	// Use this for initialization
	void Start () {
		pathfind = GetComponent<PathFinding>();
		grid = GetComponent<Grid>();
		hero = GameObject.Find("Hero");
		square = hero.GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Color[] pixels = square.texture.GetPixels(130,180,100,100);
			for(int i = 0;i < pixels.Length;i++) {
				//pixels[i].r= 999;
			}
			square.texture.SetPixels(0,0,100,100,pixels);
			square.texture.Apply();
			print("Procesed pixels");
		}
		if(Input.GetMouseButtonDown(1)){
			grid.StartRebuildGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	void callback(Vector3[] paths){
		//square.GetComponent<Spawn>().path = paths;
	}
}
