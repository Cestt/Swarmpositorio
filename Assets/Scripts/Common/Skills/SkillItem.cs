using UnityEngine;
using System.Collections;

public class SkillItem {

	[HideInInspector]
	public bool end;
	public virtual void Update (float deltaTime){}
	public virtual void Remove (){}
}
