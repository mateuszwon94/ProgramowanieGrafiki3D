using UnityEngine;
using System.Collections;

public class SelectUnit : MonoBehaviour {

	SpawnHexes hexes;
	GameObject selectedUnit;
	GameObject selectedHex;
	Camera camera;

	RaycastHit hitInfo;

	void Start () {
		selectedUnit = new GameObject();
		camera = gameObject.GetComponent<Camera>();
		hexes = GameObject.Find("Terrain").GetComponent<SpawnHexes>();
	}

	void Update () {
		if (Input.GetButton("Mouse 1") && hexes != null) {
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

			if (Physics.Raycast(ray, out hitInfo)) {
				selectedHex = hexes.FindHexWithPosition (hitInfo.point.x, hitInfo.point.y);
				if (selectedHex != null) {
					Debug.Log("Works...?");

					selectedHex.GetComponent<MeshRenderer>().materials[0].color = Color.cyan;
				}
			}
		}
		else if (Input.GetButton("Mouse 1") && hexes == null) Debug.Log("The value \"hexes\" is null.");
	}
}
