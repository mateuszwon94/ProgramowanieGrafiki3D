using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PutMenOnHex : MonoBehaviour {

	public GameObject terrain;
	// Use this for initialization
	void Start () {
		List<GameObject> path = new List<GameObject>();
		int x, y;

		for (int i = 0; i < Random.Range(500, 1000); i++) {
			x = Random.Range(0, 74);
			y = Random.Range(0, 74);
			if (terrain.GetComponent<SpawnHexes>().hexGrid[x, y] != null && terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().IsAvaliable()) {
				terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().ChangeHexColor(Color.red);
			}
			else {
				i--;
			}
		}
		while (true) {
			x = Random.Range(0, 74);
			y = Random.Range(0, 74);
			if (terrain.GetComponent<SpawnHexes>().hexGrid[x, y] != null && terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().IsAvaliable())
				break;
		}
		GameObject hex = terrain.GetComponent<SpawnHexes>().hexGrid[x, y];
		hex.GetComponent<hexProperties>().PutOnHex(gameObject);


		while (true) {
			x = Random.Range(0, 74);
			y = Random.Range(0, 74);
			if (terrain.GetComponent<SpawnHexes>().hexGrid[x, y] != null && terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().IsAvaliable())
				break;
		}

		path = gameObject.GetComponent<PathFindingAStar>().FindPathTo(hex, terrain.GetComponent<SpawnHexes>().hexGrid[x, y]);

		if (path != null) {
			foreach (GameObject currentHex in path) {
				currentHex.GetComponent<hexProperties>().ChangeHexColor(Color.white);
			}
		}
		terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().ChangeHexColor(Color.yellow);
	}
}
