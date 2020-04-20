using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
	public float mouseSensitivity = 100.0f;

	private float rotY = 0.0f;
	private float rotX = 0.0f;

	void Start() {
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
	}

	void Update() {
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");

		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;

		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		transform.rotation = localRotation;
	}
}