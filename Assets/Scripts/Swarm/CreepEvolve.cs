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
	[Tooltip ("Genes que cuesta comprar la mejora")]
	public int costBuyGen;
	[Tooltip ("Biomateria que cuesta comprar la mejora")]
	public int costBuyBio;
	public Sprite imageButtonCreepA;
	public Sprite imageButtonCreepB;
	public Sprite imageButtonSkill;

}
