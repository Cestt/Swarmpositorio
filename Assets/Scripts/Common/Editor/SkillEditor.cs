using UnityEngine;
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
		//DrawDefaultInspector ();
		//Variables publicas 
		skillScript.typeSkill = (Skill.typesSkill)EditorGUILayout.EnumPopup(new GUIContent("Skill type",
			"Tipos de habilidad\n 1)Instant - Daño instantaneo. Ejemplo ataque de un creep.\n 2)Projectile - Dispara el proyectil que se tenga en el prefab\n 3)Boost - Cambia uno o varios stats de la unidad"), skillScript.typeSkill);
		skillScript.coolDown = EditorGUILayout.FloatField("CoolDown",skillScript.coolDown);
		skillScript.damage = EditorGUILayout.IntField ("Damage", skillScript.damage);
		skillScript.armorPen = EditorGUILayout.IntField ("Armor Penetration", skillScript.armorPen);
		skillScript.range = EditorGUILayout.FloatField("Range",skillScript.range);
		//Enum de la lista de los tipos de ataque
		List<string> listOfTypes = new List<string>();
		foreach (TypesAttacks.TypesStruct type in skillScript.typesAttacks.types) {
			listOfTypes.Add(type.name);
		}
		skillScript.typeDamage = EditorGUILayout.Popup ("Damage Type",skillScript.typeDamage ,listOfTypes.ToArray());
		//Si es proyectil proyecta todo lo demas
		if (skillScript.typeSkill == Skill.typesSkill.Projectile) {
			skillScript.projectile = (GameObject)EditorGUILayout.ObjectField ("Projectile", skillScript.projectile, typeof(GameObject));
			skillScript.enemyPenetration = EditorGUILayout.IntField ("EnemyPenetration", skillScript.enemyPenetration);
		} else if (skillScript.typeSkill == Skill.typesSkill.Boost || skillScript.typeSkill == Skill.typesSkill.BoostSpawn) {
			skillScript.timeBoost = EditorGUILayout.FloatField (new GUIContent ("Time Boost", "Tiempo que dura el boost"), skillScript.timeBoost);
			serializedObject.Update ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("boosts"), true);
			serializedObject.ApplyModifiedProperties ();
		} else if (skillScript.typeSkill == Skill.typesSkill.Charge) {
			serializedObject.Update ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("charge"), true);
			serializedObject.ApplyModifiedProperties ();
		}
		skillScript.haveExtraSkill = EditorGUILayout.Toggle ("ExtraSkill?", skillScript.haveExtraSkill);
		if (skillScript.haveExtraSkill) {
			serializedObject.Update ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("extraSkill"), true);
			serializedObject.ApplyModifiedProperties ();
		}
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}

	}
}
