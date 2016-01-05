using UnityEngine;
using System.Collections;

public class Hero : Unit {
	
	[HideInInspector]
	public Vector3[] path;//Path del heroe
	public float speedAlongPath;//Velocidad del heroe a lo largo del path.


	/// <summary>
	/// Callback que recibe el path y lo inicia.
	/// </summary>
	/// <param name="callBackData">Path del heroe.</param>
	public void StartPath(Vector3[] callBackData){
		StopAllCoroutines();
		path = callBackData;
		StartCoroutine(MoveAlongPath());

	}

	
	/// <summary>
	/// Mueve el heroe a lo largo de path.
	/// </summary>
	IEnumerator MoveAlongPath(){
		print("Starting");
		//Punto de la ruta en la que se encuentra
		int targetIndex = 0;
		if(path != null){
			Vector3 currentWayPoint = path[0];
			//Mantiene el bucle de movimiento.
			bool loop =  true;
			while(loop){

				if(thisTransform.position == currentWayPoint){
					
					targetIndex++;
					if(targetIndex >= path.Length){
						loop = false;

					}
					//Comprueba si sigue en el path.
					if(targetIndex < path.Length)
						currentWayPoint = path[targetIndex];
				}
				thisTransform.position = Vector3.MoveTowards(thisTransform.position,currentWayPoint,speedAlongPath * Time.fixedDeltaTime);
				yield return new WaitForEndOfFrame();
			}
			path = null;

		}
		
	}
}
