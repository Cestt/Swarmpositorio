using UnityEngine;
using UnityEditor;
using LevelUtilities;

public class NewLevel{
	[MenuItem("Tools/New Level/Empty Level")]
	private static void NewEmptyLevel () {
		LevelInit.NewEmptyLevel();
	}
	[MenuItem("Tools/New Level/Basic Level")]
	private static void NewBasicLevel () {
		LevelInit.NewBasicLevel();
	}
}
