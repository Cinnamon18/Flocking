using System;
using UnityEngine;

public class Configuration : MonoBehaviour {

	public int seed = 0;
	[Tooltip("Randomizes seed (overwrites manually inputed value above)")]
	public bool randomizeSeed = false;
	[Header("Creature Variables")]
	[Range(1, 500)]
	public int creatureCount = 50;
	public float creatureVelocityMin = 0.5f;
	public float creatureVelocityMax = 100f;
	public bool leaveTrails = true;
	public float friction = 0.05f;
	public float boydForwardPropulsion = 1f;
	[Header("Flocking Variables")]
	[Tooltip("Radius in which boyds will consider other boyds in centroid calculations")]
	public float boydAttractionRadius = 10;
	[Tooltip("Radius in which boyds will consider other to avoid")]
	public float boydRepulsionRadius = 5;
	[Tooltip("A boyd's \"physical radius\" inside of which a very large force will be applied to prevent clipping.")]
	public float boydFirmRepulsionRadius = 0.5f;
	[Header("Flocking weights (need not sum to 1)")]
	[SerializeField]
	private bool flockCenteringActive = true;
	[SerializeField]
	private float flockCenteringWeight = 1;
	[SerializeField]
	private bool velocityMatchingActive = true;
	[SerializeField]
	private float velocityMatchingWeight = 1;
	[SerializeField]
	private bool collisionAvoidanceActive = true;
	[SerializeField]
	private float collisionAvoidanceWeight = 1;
	[SerializeField]
	private bool wanderingActive = true;
	[SerializeField]
	private float wanderingWeight = 1;

	[HideInInspector]
	public Vector4 flockingVarsNormalized;

	void Start() {
		UnityEngine.Random.InitState(randomizeSeed ? (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) : seed);
	}

	void Update() {
		flockingVarsNormalized = new Vector4(
			flockCenteringWeight * (flockCenteringActive ? 1 : 0),
			velocityMatchingWeight * (velocityMatchingActive ? 1 : 0),
			collisionAvoidanceWeight * (collisionAvoidanceActive ? 1 : 0),
			wanderingWeight * (wanderingActive ? 1 : 0)
		).normalized;
	}
}