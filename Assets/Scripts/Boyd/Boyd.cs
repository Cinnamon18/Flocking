using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boyds {
	public class Boyd : MonoBehaviour {
		private Configuration config;
		private Flock flock;

		private List<KeyValuePair<Boyd, float>> neighborDistances = new List<KeyValuePair<Boyd, float>>();

		void Start() {
			config = FindObjectOfType<Configuration>();
			flock = FindObjectOfType<Flock>();
			GetComponent<Rigidbody>().AddForce(new Vector3(3f, 2f, 1f) * 100);

			if (config.leaveTrails) {
				GetComponent<ParticleSystem>().Play();
			} else {
				GetComponent<ParticleSystem>().Stop();
			}
		}

		void FixedUpdate() {
			neighborDistances.Clear();
			foreach (Boyd boyd in flock.boyds) {
				if (boyd != this) {
					if (Vector3.Distance(boyd.transform.position, this.transform.position) < config.boydAttractionRadius) {
						neighborDistances.Add(new KeyValuePair<Boyd, float>(boyd, Vector3.Distance(boyd.transform.position, this.transform.position)));
					}
				}
			}

			Rigidbody rb = GetComponent<Rigidbody>();

			rb.velocity *= 1 - config.friction;

			Vector3 finalForce = config.flockingVarsNormalized.x * calcflockCentering() +
				config.flockingVarsNormalized.y * calcvelocityMatching() +
				config.flockingVarsNormalized.z * calccollisionAvoidance() +
				config.flockingVarsNormalized.w * calcwandering();

			rb.AddForce(finalForce);

			// Clamp velocity
			if (rb.velocity.magnitude > config.creatureVelocityMax) {
				rb.velocity = Vector3.Normalize(rb.velocity) * config.creatureVelocityMax;
			} else if (rb.velocity.magnitude < config.creatureVelocityMin) {
				rb.velocity = Vector3.Normalize(rb.velocity) * config.creatureVelocityMin;
			}
		}

		public Vector3 calcflockCentering() {
			Vector3 centroid = new Vector3(0, 0, 0);

			if (neighborDistances.Count == 0) {
				return centroid;
			}

			foreach (KeyValuePair<Boyd, float> neighborDistance in neighborDistances) {
				centroid += neighborDistance.Key.transform.position;
			}
			centroid /= neighborDistances.Count;
			centroid -= transform.position;
			return centroid;
		}
		public Vector3 calcvelocityMatching() {
			Vector3 avgVelo = new Vector3(0, 0, 0);

			if (neighborDistances.Count == 0) {
				return avgVelo;
			}

			foreach (KeyValuePair<Boyd, float> neighborDistance in neighborDistances) {
				avgVelo += neighborDistance.Key.GetComponent<Rigidbody>().velocity;
			}
			avgVelo /= neighborDistances.Count;
			avgVelo = avgVelo - GetComponent<Rigidbody>().velocity;

			return avgVelo;
		}
		public Vector3 calccollisionAvoidance() {
			Vector3 avoidance = new Vector3(0, 0, 0);

			List<KeyValuePair<Boyd, float>> colNeighborDists = neighborDistances.Where(neighborDistance => neighborDistance.Value < config.boydRepulsionRadius).ToList();

			if (colNeighborDists.Count == 0) {
				return avoidance;
			}

			foreach (KeyValuePair<Boyd, float> colNeighborDist in colNeighborDists) {
				//Avoidance: apply more force as boyds approach. Ramp that force to large if they drop below a maually defined clipping threshold.
				avoidance +=
					(0.5f / Mathf.Max(colNeighborDist.Value - config.boydFirmRepulsionRadius, 0.01f)) *
					(transform.position - colNeighborDist.Key.transform.position);
			}
			return avoidance;
		}

		public Vector3 calcwandering() {
			Vector3 wander = Vector3.Normalize(transform.forward) * config.boydForwardPropulsion;
			wander += transform.right * Random.Range(-10f, 10f);
			wander += transform.up * Random.Range(-10f, 10f);
			return wander;
		}
	}
}