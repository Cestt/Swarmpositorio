#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;


namespace LevelUtilities{
	public class LevelInit : MonoBehaviour {
		
		// Creates a new scene
		private static void NewScene () {
			EditorApplication.SaveCurrentSceneIfUserWantsTo ();
			EditorApplication.NewScene ();
		}
		// Remove all the elements of the scene
		public static void CleanScene () {
			GameObject[] allObjects = Object.FindObjectsOfType<GameObject>
				();
			foreach (GameObject go in allObjects) {
				GameObject.DestroyImmediate (go);
			}
		}
		// Creates a new scene capable to be used as a level
		public static void NewEmptyLevel () {
			NewScene ();
			CleanScene ();
			EditorApplication.SaveScene();
		}

		public static void NewBasicLevel () {
			NewScene ();
			GameObject Manager = new GameObject("Level Manager");
			EditorApplication.SaveScene();
		}
	}
}
#endif


