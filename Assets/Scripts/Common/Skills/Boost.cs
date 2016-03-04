using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boost {

	public enum statList{
		Armor,
		Damage
	}
	[Tooltip("Atributo afectado")]
	public statList stat;
	[Tooltip("Float porcentual en base 1. EJ: 0.5= +50%, -0.2 = -20%, 1.0 = +100% (duplica)")]
	public float boostPercent;
	//Valor previo del atributo

	public void Apply(Unit unit){

		unit.numBoosts++;
		unit.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 1);

		switch (stat){
		case statList.Armor:
			unit.armor = BoostPercent (unit.armor, unit.armorBase, true);
			break;
		case statList.Damage:
			unit.damageBoost = BoostPercent (unit.damageBoost, unit.damageBoostBase, true);
			break;
		}
	}

	public void Remove(Unit unit){

		unit.numBoosts--;
		unit.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);

		switch (stat){
		case statList.Armor:
			unit.armor = BoostPercent (unit.armor, unit.armorBase, false);
			break;
		case statList.Damage:
			unit.damageBoost = BoostPercent (unit.damageBoost, unit.damageBoostBase, false);
			break;
		}
	}

	private int BoostPercent(int actualValue, int iniValue, bool isApply){
		if (isApply)
			return (int)((actualValue / iniValue + boostPercent) * iniValue);
		else
			return (int)((actualValue / iniValue - boostPercent) * iniValue);
	}
}
