using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Squad : MonoBehaviour {
	
	public enum squadType{
		Swarm,
		Humanos
	}

	public squadType tipoEscuadra; 
	public Transform agentsSquad;
	public float speedAlongPath;
	public int maxLeaderOffset;
	public float maxRandomMovement;
	public int maxRowElements = 6;
	[HideInInspector]
	public Vector3[] path;
	[HideInInspector]
	public List<UnitSquad> Agents = new List<UnitSquad>();
	public int geneCost;
	public int bioCost;
	[Tooltip ("Numero de unidades necesarias del escuadron origen para crear este escuadron")]
	public int unitCost; 
	public Skill skill;

	public List<Squad> evolves = new List<Squad>();

	void Awake(){
		Agents.Clear();
		foreach(Transform agent in agentsSquad){
			if(agent.name != "Leader"){
				Agents.Add(agent.GetComponent<UnitSquad>());
				agent.GetComponent<UnitSquad>().tipoUnidad = tipoEscuadra;
			}
				
		}

		int i = 0;
		int j = 0;
		print(Agents.Count);
		foreach(UnitSquad a in Agents){
			
			a.GetComponent<UnitSquad>().squad = this;

			if(tipoEscuadra == squadType.Swarm){
				if(a.startPos.z == 1000){
					a.startPos = new Vector3(Random.Range(-maxRandomMovement,maxRandomMovement),
						Random.Range(-maxLeaderOffset/2,maxLeaderOffset/2),0);
				}
			}

			else if(tipoEscuadra == squadType.Humanos){
				a.startPos = new Vector3((-maxLeaderOffset + (((maxLeaderOffset*2)/maxRowElements) * j)),(-maxLeaderOffset/4) * i,0);
				j++;
				if(j == maxRowElements){
					i++;
					j = 0;
				}
			}
		}
	}

	void Start(){
		StartCoroutine(Compute());
	}

	public void Launch(){
		StartCoroutine(Compute());
	}

	IEnumerator Compute(){
		while(true){
			if(tipoEscuadra == squadType.Swarm){
				for(int i = 0; i < Agents.Count;i++){
					if(Agents[i] != null){
						Vector3 a = new Vector3((Agents[i].startPos.x +transform.position.x + Random.Range(-maxRandomMovement,maxRandomMovement)) - Agents[i].transform.position.x,
							(Agents[i].startPos.y + transform.position.y + Random.Range(-maxRandomMovement,maxRandomMovement)) - Agents[i].transform.position.y,0);
						Agents[i].goTo= a.normalized;
					}
				}

				yield return new WaitForSeconds(1f);
			}else if (tipoEscuadra == squadType.Humanos){
				for(int i = 0; i < Agents.Count;i++){
					if(Agents[i] != null){
							if(transform.position.y > 0){
								Agents[i].goTo = (transform.position + Agents[i].startPos);
							}else if(transform.position.y < 0){
								Agents[i].goTo = (transform.position - Agents[i].startPos);
							}else if(transform.position.y < 0){
								Agents[i].goTo =(transform.position - Agents[i].startPos);
							}
					}
				}
			}
			yield return null;

		}

	}
	public void SetPath(Vector3[] callBackData){
		StopAllCoroutines();
		StartCoroutine(Compute());
		path = callBackData;
		StartCoroutine(MoveAlongPath());
	}

	/// <summary>
	/// Mueve el heroe a lo largo de path.
	/// </summary>
	IEnumerator MoveAlongPath(){
		//Punto de la ruta en la que se encuentra
		int targetIndex = 0;
		if(path != null){
			Vector3 currentWayPoint = path[0];
			//Mantiene el bucle de movimiento.
			bool loop =  true;
			while(loop){

				if(transform.position == currentWayPoint){

					targetIndex++;
					if(targetIndex >= path.Length){
						loop = false;

					}
					//Comprueba si sigue en el path.
					if(targetIndex < path.Length)
						currentWayPoint = path[targetIndex];
				}
				transform.position = Vector3.MoveTowards(transform.position,currentWayPoint,speedAlongPath * Time.fixedDeltaTime);
				yield return new WaitForEndOfFrame();
			}
			path = null;

		}

	}

	public void StartAttack(){
		foreach(UnitSquad agent in Agents){
			agent.StartAttack();
		}
	}

	public void UseSkill(){
		List<Unit> units = new List<Unit> ();
		foreach (CreepSquad cs in Agents)
			units.Add (cs);
		skill.Use (units);
	}

	public void Evolve(int type){
		Squad sqEvolve = evolves [type];
		int killUnits = sqEvolve.unitCost;
		if (Agents.Count < killUnits)
			return;
		for (int i = Agents.Count - 1; killUnits > 0; i--) {
			Destroy (Agents [i].gameObject);
			Agents.RemoveAt (i);
			killUnits--;



		}
		Instantiate (sqEvolve, transform.position, Quaternion.identity);
		if (Agents.Count == 0)
			Destroy (gameObject);
	}
}
