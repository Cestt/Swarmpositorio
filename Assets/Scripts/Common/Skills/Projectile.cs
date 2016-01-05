﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed;
	private Vector3 dir;
	private Skill skill;
	private Unit owner;

	private float distance;
	private float travel;

	public void Ini(Unit _owner, Vector3 _dir, Skill _skill)
	{
		owner = _owner;
		dir = _dir;
		skill = _skill;
		distance = Vector2.Distance(owner.thisTransform.position,owner.target.thisTransform.position);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		angle += 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
	// Update is called once per frame
	void Update () {
		transform.position += dir * speed * Time.deltaTime;
		travel += speed * Time.deltaTime;
		if (travel >= distance) {
			skill.Attack(owner);
			Destroy(gameObject);
		}
	}
}