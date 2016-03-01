using UnityEngine;
using System.Collections;

public class PowerPoint : MonoBehaviour {
	[Tooltip ("Puntos necesarios para conquistar")]
	public int maxPoints = 100;
	[Tooltip ("Puntos que genera una unidad por segundo al conquistar")]
	public int pointsPerSecond = 20;

	[Tooltip ("Power generado por segundo")]
	public float powerProduction;
	private Unit unit;


	bool conquered;
	//Jugador que lo esta conquistando
	int actualPlayer;
	//Puntos de conquista
	public float points;
	private float minDist;
	int player;

	SpriteRenderer spriteRenderer;

	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		points = 0;
		actualPlayer = -1;
		conquered = false;
	}

	void Update(){
		if (unit != null) {
			if (actualPlayer == -1) {
				actualPlayer = player;
			}
			if (actualPlayer == player && !conquered){
				//float newDist = Vector3.Distance (transform.position, unit.transform.position);
				//if (minDist >= newDist) {
				//	minDist = newDist;
				spriteRenderer.color = new Color (1, 1 - (points / (float)maxPoints), 1 - (points / (float)maxPoints));
				points += Time.deltaTime * pointsPerSecond;
				if (points >= maxPoints) {
					points = maxPoints;
					conquered = true;
					spriteRenderer.color = new Color (1, 0, 0);
					Invoke ("GeneratePower", 1.0f / powerProduction);
				} 
				/*}else {
					unit.DeletePowerPoint ();
					unit = null;
				}*/
			} else if (actualPlayer != player) {
				//float newDist = Vector3.Distance (transform.position, unit.transform.position);
				//if (minDist >= newDist) {
				//	minDist = newDist;
				spriteRenderer.color = new Color (1, 1 - (points / (float)maxPoints), 1 - (points / (float)maxPoints));
				float minus = 	Time.deltaTime * pointsPerSecond;
				if (!conquered)
					minus *= 2;
				points -= minus;
				if (points <= 0) {
					points = 0;
					actualPlayer = -1;
					conquered = false;
					spriteRenderer.color = new Color (1, 1, 1);
					CancelInvoke ();
				} /*else {
						unit.DeletePowerPoint ();
						unit = null;
					}
				}*/
			}
		} else if (!conquered) {
			if (points > 0) {
				spriteRenderer.color = new Color (1, 1 - (points / maxPoints), 1 - (points / maxPoints));
				points -= Time.deltaTime * pointsPerSecond;
				if (points <= 0) {
					points = 0;
				}
			}
		} else if (points < maxPoints) {
			spriteRenderer.color = new Color (1, 1 - (points / (float)maxPoints), 1 - (points / (float)maxPoints));
			points += Time.deltaTime * pointsPerSecond;
			if (points >= maxPoints) {
				points = maxPoints;
			}
		}
	}

	/// <summary>
	/// Genera el recurso
	/// </summary>
	private void GeneratePower(){
		EconomyManager.biomatter++;
		Invoke ("GeneratePower", 1.0f / powerProduction);
	}

	/// <summary>
	/// Cambia la unidad que va a conquistar el punto
	/// </summary>
	/// <param name="_unit">Unit.</param>
	/// <param name="_player">Player.</param>
	public void SetUnit(Unit _unit, int _player){
		if (unit == null){
			unit = _unit;
			player = _player;
			//minDist = Vector3.Distance (unit.thisTransform.position, transform.position);
		}
	}

	/// <summary>
	/// La unidad deja de conquistar el punto
	/// </summary>
	public void RemoveUnit(){
		unit = null;
	}
}
