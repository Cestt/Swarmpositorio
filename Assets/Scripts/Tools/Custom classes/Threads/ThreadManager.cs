using UnityEngine;
using System.Collections;
using System.Threading;

//Maneja los threads dependiendo de los nucleos.
public class ThreadManager {

	//Crea la pool de Threads
	public void EnQueue(ParseQueue data){
		ThreadPool.QueueUserWorkItem(CallbackDamage,data);
	}

	//LLama al metodo Damage
	void CallbackDamage(object data){
		ParseQueue temp = (ParseQueue) data;
		temp.unit.Damage(temp.Damage,temp.armorPen,temp.typeAttack);
	}
}
