using UnityEngine;

public class Boyd : MonoBehaviour {
	public Configuration config;

	void Start() {
		config = FindComponentInScene<Configuration>();
	}

	void FixedUpdate() {
		if (config.flockCenteringActive) {
			Vector3 flockCenteringForce = calcflockCentering();
		}
		if (config.velocityMatchingActive) {
			Vector3 velocityMatchingForce = calcvelocityMatching();
		}
		if (config.collisionAvoidanceActive) {
			Vector3 collisionAvoidanceForce = calccollisionAvoidance();
		}
		if (config.wanderingActive) {
			Vector3 wanderingForce = calcwandering();
		}

		Vector3 finalForce = flockingVarsNormalized.x * flockCenteringForce +
			flockingVarsNormalized.y * velocityMatchingForce +
			flockingVarsNormalized.z * collisionAvoidanceForce +
			flockingVarsNormalized.w * wanderingForce;
		
		GetComponent<Rigidbody>().AddForce(finalForce);
	}
}