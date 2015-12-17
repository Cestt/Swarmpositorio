﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(Skill))]
public class SkillEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Skill skillScript = (Skill)target;
		
		if (skillScript.typesAttacks == null)
			skillScript.typesAttacks = GameObject.Find ("GameManager/TypesAttacks").GetComponent<TypesAttacks> ();
		DrawDefaultInspector ();
		List<string> listOfTypes = new List<string>();
		foreach (TypesAttacks.TypesStruct type in skillScript.typesAttacks.types) {
			listOfTypes.Add(type.name);
		}
		skillScript.typeDamage = EditorGUILayout.Popup ("Damage Type",skillScript.typeDamage ,listOfTypes.ToArray());
		
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
		
	}
}
