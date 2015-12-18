using UnityEngine;
using System.Collections;
using System.Threading;

//Maneja los threads dependiendo de los nucleos.
public class ThreadManager {

	//Crea la pool de Threads
	public static void EnQueue(ParseQueue data){
		ThreadPool.QueueUserWorkItem(CallbackDamage,data);
	}

	//LLama al metodo Damage
	private static void  CallbackDamage(object data){
		ParseQueue temp = (ParseQueue) data;
		temp.unit.Damage(temp.damage,temp.armorPen,temp.typeAttack);
	}
}
