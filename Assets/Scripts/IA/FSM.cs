using UnityEngine;
using System.Collections;

public static class FSM {

	//Posibles estados de la IA
	public enum States{
		Idle,
		Move,
		Chase,
		Attack,
		Dead
	};

}
