using UnityEngine;
using System.Collections;

public abstract class NodeSerializer : MonoBehaviour, ISerializationCallbackReceiver {


	public void OnAfterDeserialize()
	{
		Deserialize();
	}
	public void OnBeforeSerialize()
	{
		Serialize();
	}
	private void Serialize()
	{
	}
	private void Deserialize()
	{
	}
}
