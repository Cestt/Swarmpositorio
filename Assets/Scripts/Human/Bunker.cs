using UnityEngine;
using System.Collections;

public class Bunker : Building {


	public override void Dead ()
	{
		GameObject.Find ("HQHuman").GetComponent<HeadQuarters> ().DestroyBunker (this);
		Destroy (thisGameObject);
	}
}
