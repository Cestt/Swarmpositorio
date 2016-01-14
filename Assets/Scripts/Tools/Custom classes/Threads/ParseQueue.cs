using UnityEngine;
using System.Collections;
/// <summary>
/// Clase de ayuda para pasar datos a la cola del thread;
/// </summary>
[System.Serializable]
public class ParseQueue  {

	public Unit unit;
	public int damage;
	public int armorPen;
	public int typeAttack;
	public Unit enemy;

	public ParseQueue(Unit _unit,int _damage,int _armorPen,int _typeAttack,Unit _enemy){
		unit = _unit;
		damage = _damage;
		armorPen = _armorPen;
		typeAttack = _typeAttack;
		enemy = _enemy;
	}

}
