using UnityEngine;

public class Flock : MonoBehaviour {

	public Configuration config;
	public GameObject boydPrefab;
	
	List<Boyd> boyds = new List<Boyd>();
	Vector2 boundaryBoxSize = new Vector2(45, 45);

	public void Start() {
		for (int i = 0; i < config.creatureCount; i++) {
			boyds.Add(Instantiate(boydPrefab));
		}
	}

}