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
	//Mira si esta siendo destruido
	bool isRemoved;

	public WayPoint(){
		
	}

	public WayPoint(Vector3 pos){
		position = new Vector3(pos.x,pos.y,0);
		isRemoved = false;
		numCreeps = 0;
	}
	/// <summary>
	/// Inicializa el punto de ruta
	/// </summary>
	/// <param name="_spawn">Spawn. Spawn propietario</param>
	/// <param name="creeps">Creeps. Numero de creeps de inicio</param>
	public void Ini(Spawn _spawn){
		spawn = _spawn;
		path = null;
		//numCreeps = _numCreeps;

	}


	/// <summary>
	/// Callback para asignar el path
	/// </summary>
	/// <param name="_path">Path.</param>
	public void SetPath(Vector3[] _path){
		if (path == null)
			Debug.Log ("Error");
		else
			Debug.Log ("Hola " + path);
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

	/// <summary>
	/// Elimina un creep. En caso de que se llegue a 0 se elimina
	/// </summary>
	public void RemoveCreep(){
		numCreeps--;
		if (numCreeps <= 0 && !isRemoved) {
			isRemoved = true;
			spawn.RemoveWayPoint ();
		}
	}
}
