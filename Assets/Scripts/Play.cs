using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Play : MonoBehaviour {
	/*
	 * Glowny skrypt z rozgrywka
	 */

	public GameObject terrain;

	public GameObject archerSquad;  //przeciagniety w inspektorze prefam oddzialu lucznikow
	public GameObject archerFigure; //przeciagniaty w inspektorze prefab pojedynczego lucznika

	List<GameObject> myArmy = new List<GameObject>();    //lista oddzialow w mojej armii
	List<GameObject> enemyArmy = new List<GameObject>(); //lista oddzialow w armi przeciwnika
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

	public float mouseOnFadeSpeed = 0.1f;
	public float fadeTolerance = 0.1f;

	void Awake() {
		/*
		 * Tworzy podstawe armii mojej i komputera
		 */
		myArmys = new GameObject("My Army");
		myArmys.AddComponent<ArmyProperties>();
		myArmys.GetComponent<ArmyProperties>().squads = myArmy;

		enemyArmys = new GameObject("Enemy Army");
		enemyArmys.AddComponent<ArmyProperties>();
		enemyArmys.GetComponent<ArmyProperties>().squads = enemyArmy;
	}

	void Start() {
		/*
		 * Tworzy na planszy nieaktywne hexy przez ktore nie mozna przejsc
		 * Tworzy armie komputera
		 */
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
			enemyArmy[i].GetComponent<SquadProprties>().init(5, archerFigure, 30, 4, 6, 3, true, 4, 4, 4, 2, 5, terrain.GetComponent<SpawnHexes>().hexGrid[x, y], 0, 50, enemyArmys, i, false, GUI.GetComponent<GUIInput>());
			enemyArmy[i].GetComponent<SquadProprties>().ChangeColor(Color.blue);
		}
	}

	void Update() {
		/*
		 * Tu sie toczy rozrywka
		 */
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
			else if (gameMode == 3)
				AttackMode();
		}
	}

	void DeploymentMode() {
		/*
		 * Zarzadzanie trybem rozmieszczania
		 * Tworzzy odpowiednio armie gracza
		 */
		if (curHex != null && Input.anyKey) {
			if (Input.GetButtonDown("Mouse 1") && curHex.GetComponent<hexProperties>().IsAvaliable() && curHex.GetComponent<hexProperties>().IsFree()) {
				myArmy.Add((GameObject)Instantiate(archerSquad, new Vector3(0, 0, 0), Quaternion.identity));
				myArmy[i].transform.parent = myArmys.transform;
				myArmy[i].transform.name = "Archer Squad " + i.ToString();
				myArmy[i].GetComponent<SquadProprties>().init(5, archerFigure, 30, 4, 6, 3, true, 4, 4, 4, 2, 5, curHex, 0, 50, myArmys, i, true, GUI.GetComponent<GUIInput>());
				i++;
			}
		}

	}

	void BeforEvryGameMode() {
		/*
		 * Zadania ktore musza zostac rozpatrzone przed kazdym obiegiem normalnego trybu rozgrywki
		 * Odczytanie ktory oddzial jest zaznaczony
		 * Ewentualnie ukrycie powstalej gdzies wczesniej sciezki
		 */
		whichSquadIsSelect = terrain.GetComponent<SelectUnit>().whichSquadIsSelect;

		if (whichSquadIsSelect == null) {
			NonMode();
		}
	}

	void NonMode() {
		/*
		 * Uruchamiany jesli tryb gry jest ustawiony na 0
		 * Jesli jest istnieje jakas ciezka to ja znika
		 */
		if (path.Count != 0) {
			ChangeVisibilityOfPath(path, true);
			path.Clear();
		}
	}

	void MovementMode() {
		/*
		 * Zarzadzanie trybem ruchu
		 * wyznaczanie sciezki i przemieszczanie sie po niej
		 */
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
		/*
		 * Zarzadzanie trybem strzelania
		 * po prostu strzal
		 */
		if (curHex != null && Input.anyKey) {
			if (Input.GetButtonDown("Mouse 2") && !curHex.GetComponent<hexProperties>().IsFree() && whichSquadIsSelect != null) {
				GameObject toWhatWantToFire = curHex.GetComponent<hexProperties>().GetFromHex().GetComponent<UnitProperties>().inWhichSquad;
				whichSquadIsSelect.GetComponent<SquadProprties>().Fire(toWhatWantToFire);

			}
		}
	}

	void AttackMode() {
		if (curHex != null && Input.anyKey) {
			if (Input.GetButtonDown("Mouse 2") && !curHex.GetComponent<hexProperties>().IsFree() && whichSquadIsSelect != null) {
				GameObject whatWantToAttack = curHex.GetComponent<hexProperties>().GetFromHex().GetComponent<UnitProperties>().inWhichSquad;
				whichSquadIsSelect.GetComponent<SquadProprties>().Attack(whatWantToAttack);
			}
		}
	}

	void ChangeVisibilityOfPath(List<GameObject> path, bool isVisible) {
		/*
		 * Funkcja zmienia widocznosc calej sciezki
		 */
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
