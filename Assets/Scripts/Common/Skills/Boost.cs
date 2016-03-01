using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boost {

	public enum statList{
		Armor
	}
	[Tooltip("Atributo afectado")]
	public statList stat;
	[Tooltip("Float porcentual en base 1. EJ: 0.5=50% de boost, -0.2 = -20%")]
	public float boostPercent;
	//Valor previo del atributo

	public void Apply(Unit unit){

		switch (stat){
		case statList.Armor:
			unit.armor = (int)((unit.armor/unit.armorBase + boostPercent)*unit.armorBase);
			unit.numBoosts++;
			unit.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 1);
			break;
		}
	}

	public void Remove(Unit unit){
		switch (stat){
		case statList.Armor:
			unit.armor = (int)((unit.armor/unit.armorBase - boostPercent)*unit.armorBase);
			unit.numBoosts--;
			unit.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
			break;
		}
	}
}
