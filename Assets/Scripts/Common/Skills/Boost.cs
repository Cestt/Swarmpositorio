using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boost {

	public enum statList{
		Armor
	}
	[Tooltip("Atributo afectado")]
	public statList stat;
	[Tooltip("Float multiplicador. EJ: 0.5=Mitad 2=Doble")]
	public float boostPercent;
	//Valor previo del atributo
	private int previousValue;

	public void Apply(Unit unit){

		switch (stat){
		case statList.Armor:
			previousValue = unit.armor;
			unit.armor = (int)(unit.armor * boostPercent);
			unit.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 1);
			break;
		}
	}

	public void Remove(Unit unit){
		switch (stat){
		case statList.Armor:
			unit.armor = previousValue;
			//unit.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
			break;
		}
	}
}
