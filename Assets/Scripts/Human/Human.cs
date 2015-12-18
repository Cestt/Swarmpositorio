using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : Unit {
	public Unit target2;

	void Awake () {
		base.Awake ();
		target = target2;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.S))
			skills [0].Use (this);
	}


}
