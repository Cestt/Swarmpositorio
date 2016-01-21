using UnityEngine;
using System.Collections;
using System;

public class ApathQueue  {

	public Vector3 startPosition;
	public Vector3 endPosition;
	public Action<Vector3[]> callback;

	public ApathQueue(Vector3 _startPosition, Vector3 _endPosition, Action<Vector3[]> _callback){
		startPosition = _startPosition;
		endPosition = _endPosition;
		callback = _callback;
	}
}
