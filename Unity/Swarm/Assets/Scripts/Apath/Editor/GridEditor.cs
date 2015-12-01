using UnityEngine;
using System.Collections;
using UnityEditor;
 //Do not edit this script, its only for easy non coding configuration.
 
[CustomEditor(typeof(Grid))]
public class GridEditor : Editor {


	// Use this for initialization
	void Start () {
	
	}
	
	public override void OnInspectorGUI()
	{
		Grid gridScript = (Grid) target;


		DrawDefaultInspector();

		if(gridScript.unwalkableMask<= 7){
			EditorGUILayout.HelpBox("Please dont use the default masks", MessageType.Info);
		} 
		gridScript.gridWorldSize = EditorGUILayout.Vector2Field("Grid Size",gridScript.gridWorldSize);
		gridScript.nodeTemp = EditorGUILayout.FloatField("Node Size",gridScript.nodeTemp);
		if(gridScript.nodeTemp == 0.2f){
			EditorGUILayout.HelpBox("Limit node size is 0.2 or greater",MessageType.Warning,true);
		}
		if(gridScript.nodeTemp < 0.2f){
			gridScript.nodeTemp = 0.2f;
		}

		gridScript.displayGizmos = EditorGUILayout.Toggle("Display nodes",gridScript.displayGizmos);

		if(gridScript.gridWorldSize.x < gridScript.nodeTemp || gridScript.gridWorldSize.y < gridScript.nodeTemp){
			if(gridScript.gridWorldSize.x < gridScript.nodeTemp){
				EditorGUILayout.HelpBox("Node size exceeds Grid Size X",MessageType.Error,true);
			}

			if(gridScript.gridWorldSize.y < gridScript.nodeTemp){
				EditorGUILayout.HelpBox("Node size exceeds Grid Size Y",MessageType.Error,true);
			}

		}else{
			if(GUILayout.Button("Preview Grid")){
				gridScript.CreateGrid();
			}
		}





		if (GUI.changed)
			EditorUtility.SetDirty(gridScript);
	}
	void OnDrawGizmos(){
		Grid gridScript = (Grid) target;

		Gizmos.DrawWireCube(gridScript.gameObject.transform.position,new Vector3(gridScript.gridWorldSize.x,gridScript.gridWorldSize.y,1));
		
		if(gridScript.grid != null & gridScript.displayGizmos){
			
			foreach(Node n in gridScript.grid){
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (gridScript.nodeSize-.1f));
			}
		}
	}
}
