using System;
using UnityEngine;

public class Configuration : MonoBehaviour {

	public int seed = 0;
	[Tooltip("Randomizes seed (overwrites manually inputed value above)")]
	public bool randomizeSeed = false;
	[Header("Creature Variables")]
	[Range(1, 500)]
	public int creatureCount = 50;
	public float creatureVelocityMin = 1f;
	public float creatureVelocityMax = 10f;
	public bool leaveTrails;
	[Header("Flocking Variables (need not sum to 1)")]
	[ShowInEditor]
	private bool flockCenteringActive;
	[ShowInEditor]
	private float flockCenteringWeight;
	[ShowInEditor]
	private bool velocityMatchingActive;
	[ShowInEditor]
	private float velocityMatchingWeight;
	[ShowInEditor]
	private bool collisionAvoidanceActive;
	[ShowInEditor]
	private float collisionAvoidanceWeight;
	[ShowInEditor]
	private bool wanderingActive;
	[ShowInEditor]
	private float wanderingWeight;

	[HideInEditor]
	public Vector4 flockingVarsNormalized;


	void Start() {
		UnityEngine.Random.InitState(randomizeSeed ? (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) : seed);
	}

	void Update() {
		flockingVarsNormalized = new Vector4(
			flockCenteringWeight * (flockCenteringWeight ? 1 : 0),
			velocityMatchingWeight * (velocityMatchingWeight ? 1 : 0),
			collisionAvoidanceWeight * (collisionAvoidanceWeight ? 1 : 0),
			wanderingWeight * (wanderingWeight ? 1 : 0)
		).normalized;
	}
}