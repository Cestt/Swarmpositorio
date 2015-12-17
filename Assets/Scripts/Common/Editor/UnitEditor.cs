using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(Unit))]
public class UnitEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Unit unitScript = (Unit)target;

		if (unitScript.typesAttacks == null)
			unitScript.typesAttacks = GameObject.Find ("GameManager/TypesAttacks").GetComponent<TypesAttacks> ();
		DrawDefaultInspector ();
		List<string> listOfTypes = new List<string>();
		foreach (TypesAttacks.TypesStruct type in unitScript.typesAttacks.types) {
			listOfTypes.Add(type.name);
		}
		unitScript.weaknessType = EditorGUILayout.Popup ("Weakness Type",unitScript.weaknessType ,listOfTypes.ToArray());

		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}

	}
}

[CustomEditor(typeof(Human))]
public class HumanEditor : UnitEditor {
}

