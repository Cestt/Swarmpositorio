using UnityEngine;
using System.Collections;

public class BoostArea : BoostItem {
	GameObject follow;
	Vector3 pos;

	public BoostArea(Boost[] listOfBoosts,float _timeBoost, BoostManager _manager, GameObject _follow) 
		: base(listOfBoosts,_timeBoost,_manager)
	{
		follow = _follow;
		pos = follow.transform.position;
	}

	public BoostArea(Boost[] listOfBoosts,float _timeBoost, BoostManager _manager, Vector3 _pos) 
		: base(listOfBoosts,_timeBoost,_manager)
	{
		pos = _pos;
	}
	// Update is called once per frame
	void Update () {
		if (follow != null) {
			pos = follow.transform.position;
		}
		CheckArea ();
	}

	private void CheckArea(){
		
	}
}
