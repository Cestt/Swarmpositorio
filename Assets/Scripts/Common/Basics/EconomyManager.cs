using UnityEngine;
using System.Collections;

public class EconomyManager : MonoBehaviour {

	public static int gene;
	public static int biomatter;
	public static int newSpawnCostGene;
	public static int newSpawnCostBio;
	public static Pool pool;

	public int geneIni;
	public int biomatterIni;
	public int newSpawnGene;
	public int newSpawnBio;

	void Awake(){
		gene = geneIni;
		biomatter = biomatterIni;
		newSpawnCostGene = newSpawnGene;
		newSpawnCostBio = newSpawnBio;
		pool = GameObject.Find ("Pool").GetComponent<Pool> ();
	}

	/// <summary>
	/// Devuelve lo que cuesta en genes comprar una evolucion
	/// </summary>
	/// <returns>Los genes que cuesta la evolucion</returns>
	/// <param name="tier">Tier.</param>
	/// <param name="subTier">Sub tier.</param>
	/// <param name="subType">Sub type.</param>
	public static int GetCreepEvolveCostGene(int tier, int subTier, int subType){
		/*switch (tier) {
			case 1:
				if (subType == -1) {
					return pool.tier1Evolve [subTier].costBuyGen;
				} else if (subType == 0) {
					return pool.tier1Evolve [subTier].evolveA.costBuyGen;
				} else if (subType == 1) {
					return pool.tier1Evolve [subTier].evolveB.costBuyGen;
				}
				break;
			case 2:
				if (subType == -1) {
					return pool.tier2Evolve [subTier].costBuyGen;
				} else if (subType == 0) {
					return pool.tier2Evolve [subTier].evolveA.costBuyGen;
				} else if (subType == 1) {
					return pool.tier2Evolve [subTier].evolveB.costBuyGen;
				}
				break;
			case 3:
				if (subType == -1) {
					return pool.tier3Evolve [subTier].costBuyGen;
				} else if (subType == 0) {
					return pool.tier3Evolve [subTier].evolveA.costBuyGen;
				} else if (subType == 1) {
					return pool.tier3Evolve [subTier].evolveB.costBuyGen;
				}
				break;
		}*/
		return -1;
	}

	/// <summary>
	/// Devuelve lo que cuesta en biomateria comprar una evolucion
	/// </summary>
	/// <returns>La biomateria que cuesta la evolucion</returns>
	/// <param name="tier">Tier.</param>
	/// <param name="subTier">Sub tier.</param>
	/// <param name="subType">Sub type.</param>
	public static int GetCreepEvolveCostBio(int tier, int subTier, int subType){
		/*switch (tier) {
		case 1:
			if (subType == -1) {
				return pool.tier1Evolve [subTier].costBuyBio;
			} else if (subType == 0) {
				return pool.tier1Evolve [subTier].evolveA.costBuyBio;
			} else if (subType == 1) {
				return pool.tier1Evolve [subTier].evolveB.costBuyBio;
			}
			break;
		case 2:
			if (subType == -1) {
				return pool.tier2Evolve [subTier].costBuyBio;
			} else if (subType == 0) {
				return pool.tier2Evolve [subTier].evolveA.costBuyBio;
			} else if (subType == 1) {
				return pool.tier2Evolve [subTier].evolveB.costBuyBio;
			}
			break;
		case 3:
			if (subType == -1) {
				return pool.tier3Evolve [subTier].costBuyBio;
			} else if (subType == 0) {
				return pool.tier3Evolve [subTier].evolveA.costBuyBio;
			} else if (subType == 1) {
				return pool.tier3Evolve [subTier].evolveB.costBuyBio;
			}
			break;
		}*/
		return -1;
	}
}
