using UnityEngine;

namespace Boyds {
	public class WiggleBoydyPart : MonoBehaviour {

		public float wiggleSpeed = 0.5f;
		public float wiggleMagnitude = 20f;

		private float mySeed;

		public void Start() {
			mySeed = Random.value;
		}

		public void Update() {
			this.transform.eulerAngles = new Vector3(
				(Mathf.PerlinNoise(wiggleSpeed * Time.time, 100000.1f * mySeed) - 0.5f) * wiggleMagnitude, 0, 0
			);
		}
	}
}