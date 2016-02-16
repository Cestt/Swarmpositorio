using UnityEngine;
using System.Collections;

public class CreepEvolve : MonoBehaviour {

	//Creep al que se va a evolucionar
	public Creep creep;

	public CreepEvolve evolveA;
	public CreepEvolve evolveB;
	//Skill de la evolucion
	public Skill skill;

	public float spawnRate;
	public int numPool;
	public int costGen;
	public int costBio;
	public Sprite imageButtonCreepA;
	public Sprite imageButtonCreepB;
	public Sprite imageButtonSkill;

}
