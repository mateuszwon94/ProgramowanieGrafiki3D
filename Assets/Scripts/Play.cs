using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Play : MonoBehaviour {
	public GameObject terrain;

	public GameObject archerSquad;
	public GameObject archerFigure;

	List<GameObject> myArmy = new List<GameObject>();
	List<GameObject> enemyArmy = new List<GameObject>();
	public GameObject where, curHex, endHex;
	public List<GameObject> path = new List<GameObject>();

	public GameObject whichSquadIsSelect;

	List<GameObject> oldHexes = new List<GameObject>();

	GameObject myArmys;
	GameObject enemyArmys;

	public GameObject GUI;
	int gameMode;

	int x, y;

	int i = 0;

	static bool menOnHex = false;
	static bool endOfPath = false;

	public float mouseOnFadeSpeed = 0.1f;
	public float fadeTolerance = 0.1f;

	void Awake() {
		myArmys = new GameObject("My Army");
		myArmys.AddComponent<ArmyProperties>();
		myArmys.GetComponent<ArmyProperties>().squads = myArmy;

		enemyArmys = new GameObject("Enemy Army");
		enemyArmys.AddComponent<ArmyProperties>();
		enemyArmys.GetComponent<ArmyProperties>().squads = enemyArmy;
	}

	void Start() {
		for (int i = 0 ; i < Random.Range(1500, 2000) ; i++) {
			x = Random.Range(0, 75);
			y = Random.Range(0, 75);
			if (terrain.GetComponent<SpawnHexes>().hexGrid[x, y] != null && terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().IsAvaliable()) {
				terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().ChangeHexColor(Color.red, 0);
				terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().ChangeAvaliablity();
			}
			else {
				i--;
			}
		}
		gameMode = GUI.GetComponent<GUIInput>().GetGameMode();
		for (int i = 0 ; i < 5 ; ++i) {
			do {
				x = Random.Range(0, 75);
				y = Random.Range(0, 75);
			} while (terrain.GetComponent<SpawnHexes>().hexGrid[x, y] == null || terrain.GetComponent<SpawnHexes>().hexGrid[x, y].GetComponent<hexProperties>().IsAvaliable() == false);
			enemyArmy.Add((GameObject)Instantiate(archerSquad, new Vector3(0, 0, 0), Quaternion.identity));
			enemyArmy[i].transform.parent = enemyArmys.transform;
			enemyArmy[i].transform.name = "Archer Squad " + i.ToString();
			enemyArmy[i].GetComponent<SquadProprties>().init(5, archerFigure, 30, 3, 6, 3, true, 4, 4, 2, 5, terrain.GetComponent<SpawnHexes>().hexGrid[x, y], 0, 50, enemyArmys,i, false, GUI.GetComponent<GUIInput>());
			enemyArmy[i].GetComponent<SquadProprties>().ChangeColor(Color.blue);
		}
	}

	void Update() {
		curHex = terrain.GetComponent<MouseOnHex>().currentHex;
		gameMode = GUI.GetComponent<GUIInput>().GetGameMode();
		if (gameMode < 0)
			DeploymentMode();
		else {
			BeforEvryGameMode();

			if (gameMode == 0)
				NonMode();
			else if (gameMode == 1)
				MovementMode();
			else if (gameMode == 2)
				FirementMode();
		}
	}

	void DeploymentMode() {
		if (curHex != null && Input.anyKey) {
			if (Input.GetButtonDown("Mouse 1") && curHex.GetComponent<hexProperties>().IsAvaliable() && curHex.GetComponent<hexProperties>().IsFree()) {
				myArmy.Add((GameObject)Instantiate(archerSquad, new Vector3(0, 0, 0), Quaternion.identity));
				myArmy[i].transform.parent = myArmys.transform;
				myArmy[i].transform.name = "Archer Squad " + i.ToString();
				myArmy[i].GetComponent<SquadProprties>().init(5, archerFigure, 30, 3, 6, 3, true, 4, 4, 2, 5, curHex, 0, 50, myArmys, i, true, GUI.GetComponent<GUIInput>());
				i++;
			}
		}

	}

	void BeforEvryGameMode() {
		whichSquadIsSelect = terrain.GetComponent<SelectUnit>().whichSquadIsSelect;

		if (whichSquadIsSelect == null && path.Count != 0) {
			ChangeVisibilityOfPath(path, true);
			path.Clear();
		}
	}

	void NonMode() {
		if (path.Count != 0) {
			ChangeVisibilityOfPath(path, true);
			path.Clear();
		}
	}

	void MovementMode() {
		if (curHex != null && Input.anyKey) {
			if (Input.GetButtonDown("Mouse 2") && curHex.GetComponent<hexProperties>().IsAvaliable() && curHex.GetComponent<hexProperties>().IsFree() && whichSquadIsSelect != null && path.Count == 0) {
				path = whichSquadIsSelect.GetComponent<SquadProprties>().FindPathTo(curHex);
				if (path != null) {
					ChangeVisibilityOfPath(path, false);
					endHex = path[path.Count - 1];
				}
			}
			else if (Input.GetButtonDown("Mouse 2") && curHex.GetComponent<hexProperties>().IsAvaliable() && curHex.GetComponent<hexProperties>().IsFree() && whichSquadIsSelect != null && path.Count != 0 && endHex != curHex) {
				ChangeVisibilityOfPath(path, true);
				oldHexes = new List<GameObject>(whichSquadIsSelect.GetComponent<SquadProprties>().unitHexes);
				path.Clear();
				path = whichSquadIsSelect.GetComponent<SquadProprties>().FindPathTo(curHex);
				if (path != null) {
					ChangeVisibilityOfPath(path, false);
					endHex = path[path.Count - 1];
				}
			}
			else if (Input.GetButtonDown("Mouse 2") && curHex.GetComponent<hexProperties>().IsAvaliable() && curHex.GetComponent<hexProperties>().IsFree() && whichSquadIsSelect != null && path.Count != 0 && endHex == curHex) {

				oldHexes = new List<GameObject>(whichSquadIsSelect.GetComponent<SquadProprties>().unitHexes);

				whichSquadIsSelect.GetComponent<SquadProprties>().MoveTo(path);

				List<GameObject> newHexes = new List<GameObject>(whichSquadIsSelect.GetComponent<SquadProprties>().unitHexes);

				terrain.GetComponent<SelectUnit>().ReSelectUnit(oldHexes, newHexes);

				ChangeVisibilityOfPath(path, true);
				path.Clear();
			}
		}
	}

	void FirementMode() {
		if (curHex != null && Input.anyKey) {
			if (Input.GetButtonDown("Mouse 2") && !curHex.GetComponent<hexProperties>().IsFree() && whichSquadIsSelect != null) {
				GameObject toWhatWantToFire = curHex.GetComponent<hexProperties>().GetFromHex().GetComponent<UnitProperties>().inWhichSquad;
				whichSquadIsSelect.GetComponent<SquadProprties>().Fire(toWhatWantToFire);

			}
		}
	}

	void ChangeVisibilityOfPath(List<GameObject> path, bool isVisible) {
		foreach (GameObject hex in path) {
			if (isVisible) {
				while ((hex.GetComponent<hexProperties>().GetSelectionAlpha(3)) > fadeTolerance) {
					hex.GetComponent<hexProperties>().SetSelectionAlpha(Mathf.Lerp(hex.GetComponent<hexProperties>().GetSelectionAlpha(3), 0, Time.deltaTime * mouseOnFadeSpeed), 3);
				}
			}
			else {
				while ((1 - hex.GetComponent<hexProperties>().GetSelectionAlpha(3)) > fadeTolerance) {
					hex.GetComponent<hexProperties>().SetSelectionAlpha(Mathf.Lerp(hex.GetComponent<hexProperties>().GetSelectionAlpha(3), 1, Time.deltaTime * mouseOnFadeSpeed), 3);
				}
			}

		}
	}
}
