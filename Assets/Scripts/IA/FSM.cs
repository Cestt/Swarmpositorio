using UnityEngine;
using System.Collections;

[System.Serializable]
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
