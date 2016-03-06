using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	public int offset = 2;
	public float speed = 2;

	public float zoomMin = 7.2f;
	public float zoomMax = 15;
	public float scrollSpeed = 2;
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftControl)) {
			if (Input.mousePosition.x >= Screen.width - offset) {
				transform.position += new Vector3 (speed, 0, 0) * Time.deltaTime; 
			} else if (Input.mousePosition.x <= 0 + offset) {
				transform.position -= new Vector3 (speed, 0, 0) * Time.deltaTime; 
			}
			if (Input.mousePosition.y >= Screen.height - offset) {
				transform.position += new Vector3 (0, speed, 0) * Time.deltaTime; 
			} else if (Input.mousePosition.y <= 0 + offset) {
				transform.position -= new Vector3 (0, speed, 0) * Time.deltaTime; 
			}
		}
		//Debug.Log (Input.GetAxis ("Mouse ScrollWheel"));
		Camera.main.orthographicSize += Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed * -1;
		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, zoomMin, zoomMax);
	}
}