using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SquadProprties : MonoBehaviour {

	public int menInSquad;
	public int deadMen = 0;

	public int startHP;

	public GameObject squadRootHex;

	public GameObject inWhichArmy;
	public int whichInArmy;

	public List<GameObject> squad;
	public List<GameObject> deadMens = null;

	public List<GameObject> unitHexes = new List<GameObject>();

	public bool isSelected = false;

	public int actionPoints;
	public int actionPointsActual;

	public int howManyToMove;
	public int howManyToAttack;
	public int howManyToFire;

	public bool canFire;

	public int unitStrength;
	public int unitDurability;
	public int unitArmour;

	public int unitAccuracy;

	public bool isControlByHuman;

	public float mouseOnFadeSpeed = 0.1f;
	public float fadeTolerance = 0.5f;

	public GameObject ObrazeniaBox;
	public GameObject[] Obrazenia = new GameObject[5];

	public List<GameObject> path = new List<GameObject>();

	public void init(int mis, GameObject what, int actP, int hmtm, int hmta, int hmtf, bool cf, int us, int ud, int ua, int uac, GameObject hex, int i, int hp, GameObject army, int wia, bool icbh, GUIInput GUI) {

		ObrazeniaBox = GUI.obrazenia;
		Obrazenia = GUI.obrazeniaTexts;

		menInSquad = mis;

		actionPoints = actionPointsActual = actP;

		howManyToMove = hmta;
		howManyToAttack = hmta;
		howManyToFire = hmtf;

		canFire = cf;

		startHP = hp;

		unitStrength = us;
		unitDurability = ud;
		unitArmour = ua;

		unitAccuracy = uac;

		inWhichArmy = army;
		whichInArmy = wia;

		isControlByHuman = icbh;

		SetSquadRoot(hex);

		List<GameObject> UH = new List<GameObject>(unitHexes);

		for (int j = 0 ; j < mis ; j++) {
			squad.Add((GameObject)Instantiate(what, new Vector3(0, 0, 0), Quaternion.identity));
			if (i == 0) {
				squad[j].transform.name = "Arcger " + j.ToString();
			}
			else if (i == 1) {
				squad[j].transform.name = "Infantry " + j.ToString();
			}
			else if (i == 2) {
				squad[j].transform.name = "Cavalery " + j.ToString();
			}
			else if (i == 3) {
				squad[j].transform.name = "Cavalery " + j.ToString();
			}
			int which = UnityEngine.Random.Range(0, UH.Count - 1);
			squad[j].GetComponent<UnitProperties>().init(hp, gameObject, UH[which], j);
			UH.RemoveAt(which);
		}
		foreach (GameObject HEX in UH)
			unitHexes.Remove(HEX);

	}

	public void ChangeColor(Color what) {
		foreach (GameObject unit in squad)
			unit.GetComponent<UnitProperties>().ChangeColor(what);
	}

	void SetSquadRoot(GameObject hex) {
		squadRootHex = hex;

		GameObject[] hexNeighbours = squadRootHex.GetComponent<hexProperties>().hexNeighbors;
		List<GameObject> previousSquadHexes = unitHexes;
		unitHexes.Clear();
		unitHexes.Add(squadRootHex);
		GameObject neighbour;
		for (int k = 0 ; unitHexes.Count < menInSquad + 1 ; k++) {
			for (int l = 0 ; l < 6 ; l++) {

				neighbour = unitHexes[k].GetComponent<hexProperties>().hexNeighbors[l];
				if (neighbour != null && neighbour.GetComponent<hexProperties>().IsAvaliable() && unitHexes.IndexOf(neighbour) == -1 && neighbour.GetComponent<hexProperties>().IsFree()) {
					if (previousSquadHexes.IndexOf(neighbour) != -1 || neighbour.GetComponent<hexProperties>().IsFree()) {
						unitHexes.Add(neighbour);
					}
				}
			}
		}
	}

	public List<GameObject> FindPathTo(GameObject where) {
		List<GameObject> squadHexes = new List<GameObject>();

		foreach (GameObject unit in squad) {
			squadHexes.Add(unit.GetComponent<UnitProperties>().unitHex);
		}

		path = gameObject.GetComponent<PathFindingAStar>().FindPathTo(squadRootHex, where, squadHexes);
		return path;
	}

	public void MoveTo(List<GameObject> path) {
		SetSquadRoot(path[path.Count - 1]);

		List<GameObject> UH = new List<GameObject>(unitHexes);

		foreach (GameObject unit in squad) {
			int which = UnityEngine.Random.Range(0, UH.Count - 1);
			unit.GetComponent<UnitProperties>().MoveTo(UH[which]);
			UH.RemoveAt(which);
		}
		foreach (GameObject HEX in UH)
			unitHexes.Remove(HEX);

	}

	public void MoveTo(GameObject where) {
		MoveTo(FindPathTo(where));
	}

	public void TakeFireDamege(int howManyBullets, int shooterAccuracy, int bulletStrength) {
		for (int whichBullet = 0 ; whichBullet < howManyBullets ; ++whichBullet) {
			bool[] hitChance = new bool[10];
			for (int i = 0 ; i < shooterAccuracy ; ++i)
				hitChance[i] = true;
			for (int i = shooterAccuracy ; i < 10 ; ++i)
				hitChance[i] = false;

			if (hitChance[UnityEngine.Random.Range(0, 10)]) {
				bool[] damageChance = new bool[100];
				int damageProbability = (int)(Math.Round(0.251 * Math.Log((double)bulletStrength / (double)unitDurability) + 0.5033, 2) * 100);

				for (int i = 0 ; i < damageProbability ; ++i)
					damageChance[i] = true;
				for (int i = damageProbability ; i < 100 ; ++i)
					damageChance[i] = false;

				if (damageChance[UnityEngine.Random.Range(0, 100)]) {
					bool[] armourProtectionChance = new bool[10];
					for (int i = 0 ; i < unitArmour ; ++i)
						armourProtectionChance[i] = true;
					for (int i = unitArmour ; i < 10 ; ++i)
						armourProtectionChance[i] = false;

					if (armourProtectionChance[UnityEngine.Random.Range(0, 10)]) {
						squad[UnityEngine.Random.Range(0, squad.Count)].GetComponent<UnitProperties>().TakeFireDamege(bulletStrength);
						Obrazenia[whichBullet].GetComponent<Text>().text = "Archer " + whichBullet.ToString() + " z Archer Squad " + whichInArmy.ToString() + " O 7";
					}
					else {
						Obrazenia[whichBullet].GetComponent<Text>().text = "Archer " + whichBullet.ToString() + " z Archer Squad " + whichInArmy.ToString() + " A";
					}
				}
				else {
					Obrazenia[whichBullet].GetComponent<Text>().text = "Archer " + whichBullet.ToString() + " z Archer Squad " + whichInArmy.ToString() + " Z";
				}
			}
			else {
				Obrazenia[whichBullet].GetComponent<Text>().text = "Archer " + whichBullet.ToString() + " z Archer Squad " + whichInArmy.ToString() + " T";
			}
		}
	}

	public void Fire(GameObject toWhatWantToFire) {
		ObrazeniaBox.SetActive(true);
		toWhatWantToFire.GetComponent<SquadProprties>().TakeFireDamege(menInSquad, unitAccuracy, 7);
	}
}