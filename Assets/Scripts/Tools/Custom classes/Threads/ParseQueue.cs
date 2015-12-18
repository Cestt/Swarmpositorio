using UnityEngine;
using System.Collections;

[System.Serializable]
public class ParseQueue  {

	public Unit unit;
	public int damage;
	public int armorPen;
	public int typeAttack;

	public ParseQueue(Unit _unit,int _damage,int _armorPen,int _typeAttack){
		unit = _unit;
		damage = _damage;
		armorPen = _armorPen;
		typeAttack = _typeAttack;
	}

}
