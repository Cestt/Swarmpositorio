using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LevelEditor))]
public class EditorLevelEditor : Editor {

	float previousPixelPerUnit; 

	public override void OnInspectorGUI(){

		LevelEditor levelEditorScript = (LevelEditor) target;


		levelEditorScript.nativeResolution = EditorGUILayout.Vector2Field("Camera Size",levelEditorScript.nativeResolution);

			EditorGUILayout.Space();

		levelEditorScript.pixelPerUnit = EditorGUILayout.FloatField("Pixels per Unit",levelEditorScript.pixelPerUnit,GUILayout.MaxWidth(250));

			if(levelEditorScript.pixelPerUnit <= 0.1f){
				levelEditorScript.pixelPerUnit = 0.1f;
				EditorGUILayout.HelpBox("Limit pixel per unit is 0.1 or greater",MessageType.Warning,true);
			}
			if(previousPixelPerUnit != levelEditorScript.pixelPerUnit)
				levelEditorScript.SetOrthographicSize();


		previousPixelPerUnit = levelEditorScript.pixelPerUnit;

			EditorGUILayout.Space();

		levelEditorScript.worldSize = EditorGUILayout.Vector2Field("World Size",levelEditorScript.worldSize);
			if(levelEditorScript.pixelPerUnit >= levelEditorScript.worldSize.x)
				EditorGUILayout.HelpBox("Pixel per Unit cannot exceed World heigth",MessageType.Error,true);
			if(levelEditorScript.pixelPerUnit >= levelEditorScript.worldSize.x)
				EditorGUILayout.HelpBox("Pixel per Unit cannot exceed World width",MessageType.Error,true);
			if(levelEditorScript.worldSize.x <= 0||levelEditorScript.worldSize.y <= 0){

				if(levelEditorScript.worldSize.x <= 0){
					levelEditorScript.worldSize.x = 0;
				}
				if(levelEditorScript.worldSize.y <= 0){
					levelEditorScript.worldSize.y = 0;
				}
				EditorGUILayout.HelpBox("World Size cannot be negative",MessageType.Warning,true);
			}
			

		if(GUI.changed){
			EditorUtility.SetDirty(levelEditorScript);
		}
	}
}
