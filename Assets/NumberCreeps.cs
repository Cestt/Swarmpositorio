using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Threading;

public class NumberCreeps : MonoBehaviour {
	TextMesh mesh;
	int numero = 0;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh> ();
	}
	
	public void Add(){
		numero++;
		mesh.text = "" + numero;
	}

	public void Remove(){
		numero--;
		mesh.text = "" + numero;
	}
}
