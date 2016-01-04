using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TypesAttacks :MonoBehaviour {

	/*[Serializable]
	public struct prueba
	{
		public string name;
		public float value;
	}

	public prueba[] arrayprueba;
*/
	//public string[] types = new string[]{"None"};
	
	[System.Serializable]
	public class TypesStruct{
		
		
		public string name;
		public float value;

		public TypesStruct(string n, float v){
			name = n;
			value = v;
		}
	}
	public TypesStruct[] types = new TypesStruct[]{new TypesStruct("None",1)};

}
