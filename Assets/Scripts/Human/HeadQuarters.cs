using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HeadQuarters : Building {

	//Prefabs para la creacion de unidades y estructuras
	public List<GameObject> prefabsSquads;

	public GameObject prefabBunker;

	//Tiempo de spawnear soldados
	public float spawnTime;
	//Requeriments generados por segundo
	public int reqGeneration;

	//Requeriments que tienen los humanos
	[HideInInspector]
	public int requeriments;

	//Coste de Req de crear un nuevo bunker
	public int bunkerCostReq;
	private List<Bunker> bunkers;

	public void Start(){
		Invoke ("CreateSquad", 0);
		requeriments = 0;
		Invoke ("GenerateRequeriments", 1f / (float)reqGeneration);
		bunkers = new List<Bunker> ();
	}
	/// <summary>
	/// Crea un soldado
	/// </summary>

	void Update(){
		if (requeriments >= bunkerCostReq) {
			BuildBunker ();
		}
	}
		
	/// <summary>
	/// Construye un bunker
	/// </summary>
	private void BuildBunker(){
		Vector3 posBuild = Vector3.zero;
		if (bunkers.Count == 0) {
			Vector3 dir = GameObject.Find ("T0Spawn").transform.position - thisTransform.position;
			dir.Normalize ();
			posBuild = thisTransform.position + (dir * 10);
		} else {
			bool choosePos = true;
			while (choosePos) {
				Vector3 dir = new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f),0);
				dir.Normalize ();
				posBuild = bunkers[Random.Range (0, bunkers.Count)].thisTransform.position + (dir * 10);
				choosePos = false;
				Collider[] colls = Physics.OverlapSphere (posBuild, 3);
				foreach (Collider col in colls)
					if (col.name.Contains ("Bunker") || col.name.Contains("HQ"))
						choosePos = true;
			}
		}
		Bunker bunker = ((GameObject)Instantiate (prefabBunker, posBuild, Quaternion.identity)).GetComponent<Bunker>();
		bunkers.Add (bunker);
		requeriments -= bunkerCostReq;
	}

	/// <summary>
	/// Elimina el bunker de la lista de bunkers
	/// </summary>
	/// <param name="bunker">Bunker.</param>
	public void DestroyBunker(Bunker bunker){
		bunkers.Remove (bunker);
	}

	/// <summary>
	/// Genera 1 de recurso principal de los humanos
	/// </summary>
	private void GenerateRequeriments(){
		requeriments++;
		Invoke ("GenerateRequeriments", 1f / (float)reqGeneration);
	}

	/// <summary>
	/// Crea un soldado
	/// </summary>
	private void CreateSquad(){
		float angle = Random.Range (0, 360);
		Instantiate (prefabsSquads[Random.Range(0,prefabsSquads.Count)], transform.position + new Vector3 (Mathf.Cos (angle * Mathf.Deg2Rad) * 5, Mathf.Sin (angle * Mathf.Deg2Rad) * 5, 0),Quaternion.identity);
		Invoke ("CreateSquad", (float)1f/spawnTime);
	}
}
