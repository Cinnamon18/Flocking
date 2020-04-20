using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaneMaker : MonoBehaviour {

	public GameObject player;
	public Material terrainMaterial;

	public GameObject shrub;
	public GameObject tree;
	public GameObject rock;

	readonly float planeSize = 10f;  // size of the planes
	readonly int generationRadius = 75; // amnt to search in any direction
	Dictionary<Vector2Int, GameObject> terrainTiles = new Dictionary<Vector2Int, GameObject>();

	public Vector2Int curGridPos {
		get { return new Vector2Int((int)(player.transform.position.x / planeSize), (int)(player.transform.position.z / planeSize)); }
	}
	private Vector2Int lastGridPos;

	void Start() {
		StartCoroutine(createNewPlanes());
	}

	void Update() {
		Vector3 cam_pos = player.transform.position;
		// if the camera has moved from the current tile, make new tiles
		if (curGridPos != lastGridPos) {
			StartCoroutine(createNewPlanes());
		}

		lastGridPos = curGridPos;
	}

	private IEnumerator createNewPlanes() {
		for (int x = -generationRadius + curGridPos.x; x < generationRadius + curGridPos.x; x++) {
			for (int y = -generationRadius + curGridPos.y; y < generationRadius + curGridPos.y; y++) {
				if (!terrainTiles.ContainsKey(new Vector2Int(x, y))) {
					Vector2Int planePos = new Vector2Int(x, y);
					terrainTiles[planePos] = create_new_plane(new Vector2(planePos.x * planeSize, planePos.y * planeSize));

					// create new shrubs
				}
			}
			yield return null;
		}
	}

	// create a new plane
	private GameObject create_new_plane(Vector2 planePos) {
		// create a new plane
		GameObject s = GameObject.CreatePrimitive(PrimitiveType.Plane);
		s.name = "Plane " + planePos.x + ", " + planePos.y;

		Destroy(s.GetComponent<Collider>());

		s.transform.localScale = new Vector3(planeSize / 2f, 1, planeSize / 2f);
		s.transform.position = new Vector3(planePos.x, 0, planePos.y);
		s.GetComponent<MeshFilter>().mesh = createTerrainMesh(s.transform.position);

		spawnFlora(s.GetComponent<MeshFilter>().mesh.vertices[4] + s.transform.position);

		Renderer rend = s.GetComponent<Renderer>();
		rend.material = terrainMaterial;
		// Texture2D texture = makeATexture();
		// rend.material.mainTexture = texture;

		return s;
	}

	public int texture_width = 64;
	public int texture_height = 64;
	public float scale = 2;
	Texture2D makeATexture() {
		// create the texture and an array of colors that will be copied into the texture
		Texture2D texture = new Texture2D(texture_width, texture_height);
		Color[] colors = new Color[texture_width * texture_height];

		// create the Perlin noise pattern in "colors"
		for (int i = 0; i < texture_width; i++)
			for (int j = 0; j < texture_height; j++) {
				float x = scale * i / (float)texture_width;
				float y = scale * j / (float)texture_height;
				float t = Mathf.PerlinNoise(x, y);                          // Perlin noise!

				colors[j * texture_width + i] = new Color(t, t, t, 1.0f);  // gray scale values (r = g = b)
			}

		texture.SetPixels(colors);
		texture.Apply();
		return (texture);
	}

	// create a mesh that consists of two triangles that make up a quad
	private Mesh createTerrainMesh(Vector3 center) {
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[5];

		//TODO potential enhancement: recursively make more sub tris. 
		// vertices for a quad
		verts[0] = new Vector3(1, 0, -1);
		verts[1] = new Vector3(1, 0, 1);
		verts[2] = new Vector3(-1, 0, 1);
		verts[3] = new Vector3(-1, 0, -1);
		verts[4] = new Vector3(0, 0, 0);

		for (int i = 0; i < verts.Length; i++) {
			verts[i] = assignHeightFunc(verts[i], center);
		}

		// create uv coordinates
		Vector2[] uv = new Vector2[5];

		uv[0] = new Vector2(1, 0);
		uv[1] = new Vector2(1, 1);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(0, 0);
		uv[4] = new Vector2(0.5f, 0.5f);

		// two triangles for the face
		int[] tris = new int[4 * 3];  // need 3 vertices per triangle

		tris[0] = 0;
		tris[1] = 4;
		tris[2] = 1;
		tris[3] = 1;
		tris[4] = 4;
		tris[5] = 2;
		tris[6] = 2;
		tris[7] = 4;
		tris[8] = 3;
		tris[9] = 3;
		tris[10] = 4;
		tris[11] = 0;

		// save the vertices and triangles in the mesh object
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.uv = uv;  // save the uv texture coordinates

		mesh.RecalculateNormals();  // automatically calculate the vertex normals
		return (mesh);
	}

	public readonly Vector2 perlinOffset = new Vector2(10000.1f, 10000.1f);
	public readonly float perlinScale = 6f;
	public readonly float translateHeight = -400;

	private Vector3 assignHeightFunc(Vector3 noHeightInputVerts, Vector3 worldSpaceCenter) {
		Vector2 perlinInput = new Vector2(
			perlinOffset.x + worldSpaceCenter.x + noHeightInputVerts.x * (planeSize / 2),
			perlinOffset.y + worldSpaceCenter.z + noHeightInputVerts.z * (planeSize / 2)
		);

		float height = 0;
		// height += Mathf.PerlinNoise(perlinInput.x, perlinInput.y);
		height += Mathf.PerlinNoise(perlinInput.x * Mathf.Pow(2, -5), perlinInput.y * Mathf.Pow(2, -5)) / Mathf.Pow(2, -5 / 4f);
		height += Mathf.PerlinNoise(perlinInput.x * Mathf.Pow(2, -6), perlinInput.y * Mathf.Pow(2, -6)) / Mathf.Pow(2, -6 / 3f);
		height += Mathf.PerlinNoise(perlinInput.x * Mathf.Pow(2, -8), perlinInput.y * Mathf.Pow(2, -8)) / Mathf.Pow(2, -8 / 2f);
		height += Mathf.PerlinNoise(perlinInput.x * Mathf.Pow(2, -10), perlinInput.y * Mathf.Pow(2, -10)) / Mathf.Pow(2, -10 / 1.5f);

		height *= perlinScale;
		height += translateHeight;

		return new Vector3(noHeightInputVerts.x, height, noHeightInputVerts.z);
	}

	private void spawnFlora(Vector3 pos) {
		if (pos.y < -30 || Random.value < 0.95f) {
			return;
		}

		GameObject objToCreate = null;
		if (pos.y < -15) {
			objToCreate = shrub;
		} else if (pos.y > 75) {
			objToCreate = (Random.value > 0.5f) ? rock : shrub;
		} else {
			objToCreate = Random.value > 0.5f ? tree : Random.value > 0.5f ? rock : shrub;
		}


		Instantiate(objToCreate, pos + new Vector3(Random.Range(-2f, 2f), Random.Range(-.5f, 0), Random.Range(-2f, 2f)), objToCreate.transform.rotation);
	}

}