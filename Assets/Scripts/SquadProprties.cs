using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SquadProprties : MonoBehaviour {

	public int menInSquad;
	public int deadMen = 0;

	double A, B, C, D;

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
	public int unitAttackSkil;
	public int unitDurability;
	public int unitArmour;

	public int unitAccuracy;

	public bool isControlByHuman;

	public float mouseOnFadeSpeed = 0.1f;
	public float fadeTolerance = 0.5f;

	public GameObject ObrazeniaBox;
	public GameObject[] Obrazenia = new GameObject[5];

	public List<GameObject> path = new List<GameObject>();

	public void init(int mis, GameObject what, int ActionPoints, int PointsToMove, int PointsToAttack, int PointsToFire, bool CanFire, int Strenght, int AttackSkills, int Durability, int Armour, int Accuracy, GameObject hex, int i, int HP, GameObject whichArmy, int WhichInArmy, bool ControlByHuman, GUIInput GUI) {
		//Funkcja inicjalizujaca oddzial
		ObrazeniaBox = GUI.obrazenia;
		Obrazenia = GUI.obrazeniaTexts;

		menInSquad = mis;

		actionPoints = actionPointsActual = ActionPoints;

		howManyToMove = PointsToMove;
		howManyToAttack = PointsToAttack;
		howManyToFire = PointsToFire;

		canFire = CanFire;

		startHP = HP;

		unitStrength = Strenght;
		unitAttackSkil = AttackSkills;
		unitDurability = Durability;
		unitArmour = Armour;

		unitAccuracy = Accuracy;

		inWhichArmy = whichArmy;
		whichInArmy = WhichInArmy;

		isControlByHuman = ControlByHuman;

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
			squad[j].GetComponent<UnitProperties>().init(HP, gameObject, UH[which], j);
			UH.RemoveAt(which);
		}
		foreach (GameObject HEX in UH)
			unitHexes.Remove(HEX);

		List<Vector2> punkty = new List<Vector2>();
		punkty.Add(new Vector2(10f, 0.99f));
		punkty.Add(new Vector2(1f, 0.5f));
		punkty.Add(new Vector2(0.1f, 0.01f));

		double[] AB = LogarithmRegresion(punkty);

		A = AB[0];
		B = AB[1];

		punkty.Clear();
		punkty.Add(new Vector2(10f, 0.8f));
		punkty.Add(new Vector2(1f, 0.5f));
		punkty.Add(new Vector2(0.1f, 0.2f));

		double[] CD = LogarithmRegresion(punkty);

		C = CD[0];
		D = CD[1];
	}

	public void ChangeColor(Color what) {
		//zmiania kolor calego oddzialu
		foreach (GameObject unit in squad)
			unit.GetComponent<UnitProperties>().ChangeColor(what);
	}

	void SetSquadRoot(GameObject hex) {
		//ustawia okreslony hex jako hex glowny i rozstawia wokol niego oddzial
		squadRootHex = hex;

		List<GameObject> previousSquadHexes = unitHexes;
		unitHexes.Clear();
		if (squadRootHex.GetComponent<hexProperties>().IsAvaliable() && squadRootHex.GetComponent<hexProperties>().IsFree())
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

	void SetSquadRoot(List<GameObject> hexes) {
		squadRootHex = hexes[UnityEngine.Random.Range(0, hexes.Count)];
	}

	public List<GameObject> FindPathTo(GameObject where) {
		//znajduje sciezke od glownego hexa do okreslonego
		List<GameObject> squadHexes = new List<GameObject>();

		foreach (GameObject unit in squad) {

			squadHexes.Add(unit.GetComponent<UnitProperties>().unitHex);
		}

		path = gameObject.GetComponent<PathFindingAStar>().FindPathTo(squadRootHex, where, squadHexes);
		return path;
	}

	public void MoveTo(List<GameObject> path) {
		//rusza oddzial po okreslonej sciezce
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
		//rusza oddzial do konkretnego miejsca
		MoveTo(FindPathTo(where));
	}

	public void TakeFireDamege(int howManyBullets, int shooterAccuracy, int bulletStrength) {
		//przyjmowanie obrazen od ostrzalu przez oddzial
		for (int whichBullet = 0 ; whichBullet < howManyBullets ; ++whichBullet) {
			bool[] hitChance = new bool[10];

			for (int i = 0 ; i < shooterAccuracy ; ++i)
				hitChance[i] = true;
			for (int i = shooterAccuracy ; i < 10 ; ++i)
				hitChance[i] = false;

			if (hitChance[UnityEngine.Random.Range(0, 10)]) {
				bool[] damageChance = new bool[100];
				int damageProbability = DamageProbability((double)bulletStrength);

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
		//Strzelanie przez oddzial
		ObrazeniaBox.SetActive(true);
		toWhatWantToFire.GetComponent<SquadProprties>().TakeFireDamege(menInSquad, unitAccuracy, 7);
	}

	double[] LogarithmRegresion(List<Vector2> points) {
		//Funkcja potrzebna do wyznaczenia prawdopodobienstwa zranienia
		double[] coefficients = new double[2];
		double avrX, avrY;
		double sumXY = 0f;
		double sumX = 0f;
		double sumY = 0f;
		double sumXX = 0f;
		double N = (double)points.Count;
		foreach (Vector2 point in points) {
			sumX += Math.Log(point.x);
			sumXX += Math.Log(point.x) * Math.Log(point.x);
			sumXY += Math.Log(point.x) * point.y;
			sumY += point.y;
		}
		avrX = sumX / N;
		avrY = sumY / N;
		coefficients[0] = (N * sumXY - sumX * sumY) / (N * sumXX - sumX * sumX);
		coefficients[1] = avrY - coefficients[0] * avrX;
		return coefficients;
	}

	int DamageProbability(double attackStrenght) {
		//funkcja zwraca prawdopodobienstwo zranienia
		return (int)(Math.Round(A * Math.Log(attackStrenght / (double)unitDurability) + B, 2) * 100);
	}
	int HitProbability(double atackSkils) {
		//funkcja zwraca prawdopodobienstwo zranienia
		return (int)(Math.Round(C * Math.Log(atackSkils / (double)unitAttackSkil) + D, 2) * 100);
	}

	public void TakeAttackDamage(int howManyHits, int hitterAttackSkil, int hitterStrenght) {
		for (int whichHit = 0 ; whichHit < howManyHits ; ++whichHit) {
			bool[] hitChance = new bool[100];
			int hitProbability = HitProbability((double)hitterAttackSkil);

			for (int i = 0 ; i < hitProbability ; ++i)
				hitChance[i] = true;
			for (int i = hitProbability ; i < 100 ; ++i)
				hitChance[i] = false;

			if (hitChance[UnityEngine.Random.Range(0, 100)]) {
				bool[] damageChance = new bool[100];
				int damageProbability = DamageProbability((double)hitterStrenght);

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
						squad[UnityEngine.Random.Range(0, squad.Count)].GetComponent<UnitProperties>().TakeFireDamege(hitterStrenght);
						Obrazenia[whichHit].GetComponent<Text>().text = "Archer " + whichHit.ToString() + " z Archer Squad " + whichInArmy.ToString() + " O " + hitterStrenght.ToString();
					}
					else {
						Obrazenia[whichHit].GetComponent<Text>().text = "Archer " + whichHit.ToString() + " z Archer Squad " + whichInArmy.ToString() + " A";
					}
				}
				else {
					Obrazenia[whichHit].GetComponent<Text>().text = "Archer " + whichHit.ToString() + " z Archer Squad " + whichInArmy.ToString() + " Z";
				}
			}
			else {
				Obrazenia[whichHit].GetComponent<Text>().text = "Archer " + whichHit.ToString() + " z Archer Squad " + whichInArmy.ToString() + " T";
			}
		}
	}


	public void Attack(GameObject who) {
		ObrazeniaBox.SetActive(true);
		/*MoveTo(who.GetComponent<SquadProprties>().squadRootHex);
		for (int i = 0 ; i < squad.Count ; ++i) {
			GameObject hex;
			int j = i;
			int k = 0;
			do {
				Debug.Log(k);
				Debug.Log(j);
				int which = UnityEngine.Random.Range(0, 6);
				hex = GetComponent<SquadProprties>().squad[j].GetComponent<UnitProperties>().unitHex.GetComponent<hexProperties>().hexNeighbors[which];
				++k;
				if (k > 6) {
					j = (j + 1) % 6;
					k = 0;
				}

			} while (hex.GetComponent<hexProperties>().IsAvaliable() || hex.GetComponent<hexProperties>().IsFree());
			squad[i].GetComponent<UnitProperties>().MoveTo(hex);*/
			who.GetComponent<SquadProprties>().TakeAttackDamage(squad.Count, unitAttackSkil, unitStrength);
		/*}
		List<GameObject> UH = new List<GameObject>();
		foreach (GameObject unit in squad)
			UH.Add(unit.GetComponent<UnitProperties>().unitHex);
		SetSquadRoot(UH);*/
	}
}