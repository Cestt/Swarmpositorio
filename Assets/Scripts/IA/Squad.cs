using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Squad : MonoBehaviour {

	public List<CreepSquad> Agents = new List<CreepSquad>();
	public Unit leader = null;
	public int maxSeparation;
	Vector3 alignment;
	Vector3 cohesion;
	Vector3 separation;
	Vector3 follow;
	[HideInInspector]
	public Vector3 result;
	Vector3 cero = new Vector3(0,0,0);
	public float max;

	void Awake(){
		foreach(Unit a in Agents){
			if(a != leader)
				a.GetComponent<CreepSquad>().squad = this;
		}
	}

	void Start(){
		leader = this.GetComponent<Unit>();
		StartCoroutine(Compute());
	}

	public void Launch(){
		StartCoroutine(Compute());
	}

	IEnumerator Compute(){
		while(true){
			for(int i = 0; i < Agents.Count;i++){
				if(Agents[i] != null){
					if(Agents[i] != leader){
						if(Agents[i].startPos.z == 1000){
							Agents[i].startPos = new Vector3(Random.Range(-max,max),
								Random.Range(-maxSeparation/2,maxSeparation/2),0);
						}else{
							Vector3 a = new Vector3((Agents[i].startPos.x +leader.thisTransform.position.x + Random.Range(-max,max)) - Agents[i].transform.position.x,
								(Agents[i].startPos.y + leader.thisTransform.position.y + Random.Range(-max,max)) - Agents[i].transform.position.y,0);
							Agents[i].goTo= a.normalized;
						}

						
					}
				}
			}

			yield return new WaitForSeconds(1f);
		}

	}

	IEnumerator ComputeFollow(Vector3 pos){
		Vector3 temp = leader.thisTransform.position - pos; 
		follow = temp.normalized;
		yield return null;
	}

	IEnumerator ComputeAlignement(Unit thisAgent){
		int neighbours = 0;
		Vector3 oVector3 = Vector3.zero;
		Vector3 junk = new Vector3(thisAgent.speed,thisAgent.speed,0);
		for(int i = 0; i < Agents.Count - 1;i++){
			if(Agents[i] != null){
				if(thisAgent != Agents[i]){
					if(Agents[i] != leader){
						if(Vector3.Distance(oVector3,Agents[i].thisTransform.position) < maxSeparation){
							oVector3 += junk;
							neighbours++;
						}
					}
				}
			}
		}
		if(neighbours == 0){
			alignment = Vector3.zero;
			yield return null;
		}else{
			oVector3 /= neighbours;
			alignment = oVector3.normalized;
			yield return null;
		}
		
	}

	IEnumerator ComputeCohesion(Unit thisAgent){
		int neighbours = 0;
		Vector3 oVector3 = Vector3.zero;
		for(int i = 0; i < Agents.Count - 1;i++){
			if(Agents[i] != null){
				if(thisAgent != Agents[i]){
					if(Vector3.Distance(oVector3,Agents[i].thisTransform.position) < maxSeparation){
						oVector3 += Agents[i].thisTransform.position;
						neighbours++;
					}
				}
			}
		}
		if(neighbours == 0){
			cohesion = Vector3.zero;
			yield return null;
		}else{
			oVector3 /= neighbours;
			oVector3 -=  thisAgent.thisTransform.position;
			cohesion = oVector3.normalized;
			yield return null;
		}

	}
	IEnumerator ComputeSeparation(Unit thisAgent){
		int neighbours = 0;
		Vector3 oVector3 = Vector3.zero;
		for(int i = 0; i < Agents.Count - 1;i++){
			if(Agents[i] != null){
				if(thisAgent != Agents[i]){
					if(Agents[i] != leader){
						if(Vector3.Distance(oVector3,Agents[i].thisTransform.position) < maxSeparation){
							oVector3 += (Agents[i].thisTransform.position - thisAgent.thisTransform.position)* -1;
							neighbours++;
						}
					}

				}
			}
		}
		if(neighbours == 0){
			separation = Vector3.zero;
			yield return null;
		}else{
			oVector3 /= neighbours;
			//separation = oVector3.normalized *Random.Range(0f,separationForce);
			yield return null;
		}

	}
}
