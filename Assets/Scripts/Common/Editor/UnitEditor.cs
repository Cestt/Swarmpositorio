﻿using UnityEngine;
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
		Human humanScript = (Human)target;
	
		humanScript.detectionRadius = EditorGUILayout.FloatField("Detection Radius",humanScript.detectionRadius);
		humanScript.detectionCreepsRadius = EditorGUILayout.FloatField("Detection Creeps Radius",humanScript.detectionCreepsRadius);
		humanScript.speedAlongPath = EditorGUILayout.FloatField("Speed Along Path",humanScript.speedAlongPath);
		humanScript.biomatterGain = EditorGUILayout.IntField ("Bio Matter Gain", humanScript.biomatterGain);
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
}

[CustomEditor(typeof(Creep))]
public class CreepEditor : UnitEditor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		Creep creepScript = (Creep)target;

		creepScript.tier = EditorGUILayout.IntField(new GUIContent ("Tier"
			,"Tier al que pertenece el creep."),creepScript.tier);
		/*creepScript.subTier = EditorGUILayout.IntField (new GUIContent ("SubTier"
			, "La primera cifra marca creep base y la segunda la evolucion\nEjemplo: 1 = Comadreja, 11=Comadreja A\n2=Runner, 21=Runner A"), creepScript.subTier);
		*/
		creepScript.detectionRadius = EditorGUILayout.FloatField("Detection Radius",creepScript.detectionRadius);
		creepScript.speedAlongPath = EditorGUILayout.FloatField("Speed Along Path",creepScript.speedAlongPath);
		creepScript.costGene = EditorGUILayout.IntField(new GUIContent("Cost Gene","Coste en genes para producir el creep"),creepScript.costGene);
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
}

/*[CustomEditor(typeof(Building))]
public class BuildingEditor : UnitEditor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		Building buildScript = (Building)target;
		buildScript.detectionCreepsRadius = EditorGUILayout.FloatField("Detection Creeps Radius",buildScript.detectionCreepsRadius);

	}
}*/

[CustomEditor(typeof(HeadQuarters))]
public class HeadQuartersEditor : UnitEditor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		//DrawDefaultInspector ();
		HeadQuarters hqScript = (HeadQuarters)target;
		hqScript.detectionCreepsRadius = EditorGUILayout.FloatField("Detection Creeps Radius",hqScript.detectionCreepsRadius);
		hqScript.spawnTime = EditorGUILayout.FloatField(new GUIContent ("Spawn Time", "Medido en Soldados/segundos"),hqScript.spawnTime);
		hqScript.reqGeneration = EditorGUILayout.IntField (new GUIContent ("Req. Generation", "Medido en Req/segundos"), hqScript.reqGeneration);

		serializedObject.Update();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("prefabsSquads"), true);
		serializedObject.ApplyModifiedProperties();
		hqScript.bunkerCostReq = EditorGUILayout.IntField ("BunkerCostReq", hqScript.bunkerCostReq);
		hqScript.prefabBunker = (GameObject)EditorGUILayout.ObjectField ("Prefab Bunker", hqScript.prefabBunker, typeof(GameObject));
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
}

[CustomEditor(typeof(Bunker))]
public class BunkerEditor : UnitEditor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		//DrawDefaultInspector ();
		Bunker bunkerScript = (Bunker)target;
		bunkerScript.detectionCreepsRadius = EditorGUILayout.FloatField("Detection Creeps Radius",bunkerScript.detectionCreepsRadius);
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
}