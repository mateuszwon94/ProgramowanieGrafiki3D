using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Jest to tylko tymczasowe wiec jakos komentowac nie bede 
*/

public class PutMenOnHex : MonoBehaviour {

	public GameObject terrain;
	public GameObject men;
	GameObject where, curHex, endHex;
	List<GameObject> path = new List<GameObject>();
	List<GameObject> previousPath = new List<GameObject>();
	int x, y;

	static bool menOnHex = false;
	static bool endOfPath = false;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < Random.Range(500, 1000); i++) {
			x = Random.Range(0, 74);
			y = Random.Range(0, 74);
			if (terrain.GetComponent<SpawnHexes>().hexGrid[x, y] != null && terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().IsAvaliable()) {
				terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().ChangeHexColor(Color.red);
				terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().ChangeAvaliablity();
			}
			else {
				i--;
			}
		}
	}

	void Update(){
		curHex = terrain.GetComponent<MouseOnHex>().currentHex;
		if (curHex != null) {
			if (Input.anyKey) {
				if (Input.GetButton("Mouse 1")) {
					if (!menOnHex) {
						if (curHex.GetComponent<hexProperties>().IsAvaliable()) {
							where = curHex;
							where.GetComponent<hexProperties>().PutOnHex(men);
							menOnHex = true;
						}
					}/*
					else if (!endOfPath && (where != curHex)) {
						if (curHex.GetComponent<hexProperties>().IsAvaliable()) {
							endHex = curHex;
							path = men.GetComponent<PathFindingAStar>().FindPathTo(where, endHex);
							endOfPath = true;
						}
					}*/
				}
			}
		}

		if (where != curHex && menOnHex && curHex != null) {
			if (curHex.GetComponent<hexProperties>().IsAvaliable()) {
				endHex = curHex;
				previousPath = path;
				path = men.GetComponent<PathFindingAStar>().FindPathTo(where, endHex);
			}
		}

		if (path != null && endHex != null) {
			foreach (GameObject currentHex in previousPath) {
				currentHex.GetComponent<hexProperties>().ChangeHexColor(Color.blue);
			}
			foreach (GameObject currentHex in path) {
				currentHex.GetComponent<hexProperties>().ChangeHexColor(Color.white);
			}
			endHex.GetComponent<hexProperties>().ChangeHexColor(Color.yellow);
		}
	}
}
