using UnityEngine;
using System.Collections;

public class UITest : MonoBehaviour {
	TextMesh creepMesh;
	int numero = 0;
	Pool pool;
	// Use this for initialization
	TextMesh geneMesh;
	TextMesh bioMesh;

	void Start () {
		creepMesh = GetComponent<TextMesh> ();
		pool = GameObject.Find ("Pool").GetComponent<Pool> ();
		geneMesh = GameObject.Find ("GeneText/Number").GetComponent<TextMesh> ();
		bioMesh = GameObject.Find ("BioMatterText/Number").GetComponent<TextMesh> ();
	}



	// Update is called once per frame
	void Update () {
		geneMesh.text = "" + EconomyManager.gene;
		bioMesh.text = "" + EconomyManager.biomatter;
	}

	public void Add(){
		numero++;
		creepMesh.text = "" + numero;
	}

	public void Remove(){
		numero--;
		creepMesh.text = "" + numero;
	}
}
