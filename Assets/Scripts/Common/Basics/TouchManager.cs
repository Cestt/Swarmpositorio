using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	Spawn spawnSelected;

	// Use this for initialization
	void Start () {
	
	}


	public void ClickSpawn(Spawn spawn){
		spawnSelected = spawn;
		Debug.Log (spawn.name);
	}
}
