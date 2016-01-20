using UnityEngine;
using System.Collections;

public class WayPoint{

	public Vector3 position;
	//Path al siguiente punto de ruta
	public Vector3[] path;
	//Siguiente punto de ruta
	public WayPoint nextWayPoint;
	//Numeros de creeps que tienen que pasar por este punto de ruta
	public int numCreeps;
	//Spawn al que pertence
	Spawn spawn;

	PathFinding pathfinder;

	public WayPoint(){
		
	}

	public WayPoint(Vector3 pos){
		Debug.Log ("Nuevo wayPoint: " + pos);
		position = pos;
	}
	/// <summary>
	/// Inicializa el punto de ruta
	/// </summary>
	/// <param name="_spawn">Spawn. Spawn propietario</param>
	/// <param name="creeps">Creeps. Numero de creeps de inicio</param>
	public void Ini(Spawn _spawn, int _numCreeps){
		spawn = _spawn;
		path = null;
		numCreeps = _numCreeps;
		pathfinder = GameObject.Find("GameManager/PathFinder").GetComponent<PathFinding>();
	}

	/// <summary>
	/// Se calcula el nuevo path al nuevo punto de ruta
	/// </summary>
	/// <param name="next">Next. Siguiente punto de ruta</param>
	public void NewPath(WayPoint next){
		nextWayPoint = next;
		nextWayPoint.Ini (spawn, numCreeps);
		pathfinder.StartFindPath(position,next.position,this.SetPath);
	}

	/// <summary>
	/// Callback para asignar el path
	/// </summary>
	/// <param name="_path">Path.</param>
	public void SetPath(Vector3[] _path){
		path = _path;
	}

	/// <summary>
	/// Añade un creep y a sus sucesivos puntos de ruta
	/// </summary>
	public void AddCreep(){
		numCreeps++;
		if (nextWayPoint != null)
			nextWayPoint.AddCreep ();
	}

	public void RemoveCreep(){
		numCreeps--;
		if (numCreeps <= 0) {
			spawn.RemoveWayPoint ();
		}
	}
}
