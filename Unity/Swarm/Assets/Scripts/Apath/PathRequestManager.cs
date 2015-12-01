using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PathRequestManager : MonoBehaviour {

	List<PathRequest> pathRequestQueue = new List<PathRequest>();
	PathRequest currentPathRequest;

	static PathRequestManager manager;
	PathFinding pathFinding;

	bool isProcessingPath;

	void Awake(){
		manager = this;
		pathFinding = GetComponent<PathFinding>(); 
	}

	public static PathRequest  RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback){
		PathRequest newPathRequest = new PathRequest(pathStart,pathEnd,callback);
		manager.pathRequestQueue.Add(newPathRequest);
		manager.TryProcessNext();
		return newPathRequest;
	}
	public static void RemoveFromQeue(PathRequest request){
		manager.pathRequestQueue.Remove(request);
	}

	void TryProcessNext(){
		if(!isProcessingPath & pathRequestQueue.Count > 0){
			currentPathRequest = pathRequestQueue[pathRequestQueue.Count - 1];
			pathRequestQueue.RemoveAt(pathRequestQueue.Count - 1);
			isProcessingPath = true;
			pathFinding.StartFindPath(currentPathRequest.pathStart,currentPathRequest.pathEnd);
		}
	}
	public void FinishedProcessingPath(Vector3[] path, bool success){
		currentPathRequest.callback(path,success);
		isProcessingPath = false;
		TryProcessNext();
	}

	public struct PathRequest{
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[], bool> callback;

		public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callback){
			pathStart = _pathStart;
			pathEnd = _pathEnd;
			callback = _callback;
		}
	}
}
