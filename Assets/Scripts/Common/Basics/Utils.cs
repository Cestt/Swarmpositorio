﻿using UnityEngine;
using System.Collections;

public static class Utils {

	public static void LookAt2D(Transform source, Transform target, float offset = -90){
		Vector3 dir = target.position - source.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		angle += offset;
		source.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}