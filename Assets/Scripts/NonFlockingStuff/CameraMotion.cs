using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

	public float translate_factor = 10;

	// Move the camera, and maybe create a new plane
	void Update() {
		// get the horizontal and verticle controls (arrows, or WASD keys)
		float dz = Input.GetAxis("Vertical");
		float dx = Input.GetAxis("Horizontal");
		float dy = Input.GetAxis("EQ"); //Altitude

		float timeScaleTransFact = translate_factor * Time.deltaTime;

		// move the camera based on the keyboard input.
		if (Camera.current != null) {
			Camera.current.transform.Translate(new Vector3(0, 0, dz * timeScaleTransFact));
			Camera.current.transform.Translate(dx * timeScaleTransFact, 0, 0);
			Camera.current.transform.Translate(0, dy * timeScaleTransFact, 0, Space.World);
		}
	}
}
