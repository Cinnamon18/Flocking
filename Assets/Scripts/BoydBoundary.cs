using UnityEngine;

public class BoydBoundary : MonoBehaviour {
	public BoydBoundary partner;
	public Vector3 partnerRelPos;

	void Start() {
		Vector3 relPos = this.transform.position - partner.transform.position;
		partnerRelPos = new Vector3(Mathf.abs(relPos.x), Mathf.abs(relPos.x), Mathf.abs(relPos.x));
	}

	void OnCollisionEnter(Collision other) {
		Vector3 boydRelPos = this.transform.position - other.gameObject.transform.position;
		if (partnerRelPos.x > partnerRelPos.y && partnerRelPos.z) {
			other.transform.position = partner.transform.position + boydRelPos * new Vector3(-1, 0, 0);
		} else if (partnerRelPos.y > partnerRelPos.x && partnerRelPos.z) {
			other.transform.position = partner.transform.position + boydRelPos * new Vector3(0, -1, 0);
		} else if (partnerRelPos.z > partnerRelPos.x && partnerRelPos.y) {
			other.transform.position = partner.transform.position + boydRelPos * new Vector3(0, 0, -1);
		} else {
			Debug.LogError("Warning, critical system failure in the rear carbine valve of your BMW 376i-f. Have your car serviced immediately.");
		}
	}
}