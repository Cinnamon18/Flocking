using System.Collections.Generic;
using UnityEngine;
using Boyds;

public class Flock : MonoBehaviour {

	public Configuration config;
	public GameObject boydPrefab;

	[HideInInspector]
	public List<Boyd> boyds = new List<Boyd>();
	private float boundaryBoxSize = 45;

	private bool lastLeaveTrails = true;

	public void Start() {
		MakeBoyds();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			MakeBoyds();
		}

		if (config.leaveTrails != lastLeaveTrails) {
			if (config.leaveTrails) {
				foreach (Boyd boyd in boyds) {
					boyd.GetComponent<ParticleSystem>().Play();
				}
			} else {
				foreach (Boyd boyd in boyds) {
					boyd.GetComponent<ParticleSystem>().Stop();
				}
			}
			lastLeaveTrails = config.leaveTrails;
		}
	}

	public void MakeBoyds() {
		if (boyds.Count != 0) {
			foreach (Boyd boyd in boyds) {
				Destroy(boyd.gameObject);
			}
			boyds.Clear();
		}

		for (int i = 0; i < config.creatureCount; i++) {
			boyds.Add(Instantiate(boydPrefab, new Vector3(
				Random.Range(-boundaryBoxSize, boundaryBoxSize),
				Random.Range(-boundaryBoxSize, boundaryBoxSize),
				Random.Range(-boundaryBoxSize, boundaryBoxSize)
			), boydPrefab.transform.rotation).GetComponent<Boyd>());
		}
	}

}