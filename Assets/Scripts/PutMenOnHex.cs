using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Jest to tylko tymczasowe wiec jakos komentowac nie bede 
*/

public class PutMenOnHex : MonoBehaviour {

	public GameObject terrain;
	public GameObject men;
    //public GameObject infantry;
    public GameObject archerSquad;
	public GameObject archerFigure;
    //public GameObject cavalery;
    //public GameObject special;
	List<GameObject> myArmy = new List<GameObject>();
	List<GameObject> enemyArmy = new List<GameObject>();
	GameObject where, curHex, endHex;
	List<GameObject> path = new List<GameObject>();

	GameObject myArmys;
	GameObject enemyArmys;

	bool mouseIsDown = false;

	int x, y;
	
	int i = 0;

	static bool menOnHex = false;
	static bool endOfPath = false;
	
	void Start () {
		myArmys = new GameObject("My Army");
		enemyArmys = new GameObject("Enemy Army");
		for (int i = 0; i < Random.Range(1500, 2000); i++) {
			x = Random.Range(0, 74);
			y = Random.Range(0, 74);
			if (terrain.GetComponent<SpawnHexes>().hexGrid[x, y] != null && terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().IsAvaliable()) {
				terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().ChangeHexColor(Color.red, 0);
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
				if (Input.GetButton("Mouse 1") && !mouseIsDown && curHex.GetComponent<hexProperties>().IsAvaliable() && curHex.GetComponent<hexProperties>().IsFree()) {//&& curHex.GetComponent<hexProperties>().hexPosX < 10) {
					myArmy.Add((GameObject)Instantiate(archerSquad, new Vector3(0, 0, 0), Quaternion.identity));
					myArmy[i].transform.parent = myArmys.transform;
					myArmy[i].transform.name = "Archer Squad " + i.ToString();
					myArmy[i].GetComponent<SquadProprties>().init(5, archerFigure, 30, 3, 6, 3, true, 4, 4, 2, curHex, 0, 50);
					mouseIsDown = true;
					i++;
				}
				else if (Input.GetButton("Mouse 1") && !mouseIsDown && !curHex.GetComponent<hexProperties>().IsFree()) {
					curHex.GetComponent<hexProperties>().onHex.GetComponent<UnitProperties>().ToggleUnit();
					//curHex.onHex.inWhichSquad.ToggleSquad(curHex.onHex.whichInSquad)
				}
				else {
					mouseIsDown = false;
				}
			}
		}
	}
}
