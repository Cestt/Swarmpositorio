﻿using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SpawnCreep(){
		GameObject clone = Instantiate(Resources.Load("Creep", typeof(GameObject)),transform.position,Quaternion.identity) as GameObject;
		clone.name = "Creep";
	}
}