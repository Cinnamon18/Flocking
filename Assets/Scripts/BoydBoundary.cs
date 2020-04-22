using UnityEngine;

public class BoydBoundary : MonoBehaviour {
	public BoydBoundary partner;

	private Vector3 partnerRelPos;

	void Start() {
		Vector3 relPos = partner.transform.position - this.transform.position;
		partnerRelPos = new Vector3(Mathf.Abs(relPos.x), Mathf.Abs(relPos.y), Mathf.Abs(relPos.z));
	}

	void OnTriggerEnter(Collider other) {
		Vector3 boydRelPos = other.gameObject.transform.position - this.transform.position;
		if (partnerRelPos.x > partnerRelPos.y && partnerRelPos.x > partnerRelPos.z) {
			other.transform.position = partner.transform.position + Vector3.Scale(boydRelPos, new Vector3(-3f, 1, 1));
		} else if (partnerRelPos.y > partnerRelPos.x && partnerRelPos.y > partnerRelPos.z) {
			other.transform.position = partner.transform.position + Vector3.Scale(boydRelPos, new Vector3(1, -3f, 1));
		} else if (partnerRelPos.z > partnerRelPos.x && partnerRelPos.z > partnerRelPos.y) {
			other.transform.position = partner.transform.position + Vector3.Scale(boydRelPos, new Vector3(1, 1, -3f));
		} else {
			Debug.LogError("Warning, critical system failure in the rear carbine valve of your BMW 376i-f. Have your car serviced immediately.");
		}
	}
}