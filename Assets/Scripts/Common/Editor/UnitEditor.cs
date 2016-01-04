using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Clase propia para mostrar todos los Custom editor.
/// </summary>
[CustomEditor (typeof(Unit))]
public class UnitEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Unit unitScript = (Unit)target;

		//Obtiene la clase de los tipos de ataque
		if (unitScript.typesAttacks == null)
			unitScript.typesAttacks = GameObject.Find ("GameManager/TypesAttacks").GetComponent<TypesAttacks> ();
		//DrawDefaultInspector ();

		//Saca en inspector las variables publicas
		unitScript.life = EditorGUILayout.IntField ("Life", unitScript.life);
		unitScript.armor = EditorGUILayout.IntField ("Armor", unitScript.armor);
		unitScript.canAttack = EditorGUILayout.Toggle ("Can Attack?", unitScript.canAttack);

		//Guarda el listado de los tipos de ataque y los muestra como una lista
		List<string> listOfTypes = new List<string>();
		foreach (TypesAttacks.TypesStruct type in unitScript.typesAttacks.types) {
			listOfTypes.Add(type.name);
		}
		unitScript.weaknessType = EditorGUILayout.Popup ("Weakness Type",unitScript.weaknessType ,listOfTypes.ToArray());

		//Si puede atacar se muestra el listado de habilidades
		if (unitScript.canAttack) {
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("skills"), true);
			serializedObject.ApplyModifiedProperties();
		}

		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}

	}
}

[CustomEditor(typeof(Human))]
public class HumanEditor : UnitEditor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
	}
}

[CustomEditor(typeof(Creep))]
public class CreepEditor : UnitEditor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		Creep creepScript = (Creep)target;

		creepScript.detectionRadius = EditorGUILayout.FloatField("Detection Radius",creepScript.detectionRadius);
		creepScript.speedAlongPath = EditorGUILayout.FloatField("Speed Along Path",creepScript.speedAlongPath);
	}
}

