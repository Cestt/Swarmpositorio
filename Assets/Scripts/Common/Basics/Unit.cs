using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public int life; //Vida de la unidad
	public int armor; //Armadura de la unidad

	public void Damage(int damage, int armorPen){
		int damageReal = Mathf.Max (0, damage - Mathf.Max (0, armor - armorPen));
		life -= damageReal;
	}
}
