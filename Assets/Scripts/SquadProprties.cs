using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquadProprties : MonoBehaviour {

	public int menInSquad;

	public GameObject squadRootHex;

	public List<GameObject> squad;

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

	public float mouseOnFadeSpeed = 0.1f;
	public float fadeTolerance = 0.5f;

	public void init(int mis, GameObject what, int actP, int hmtm, int hmta, int hmtf, bool cf, int us, int ud, int ua, GameObject hex, int i, int hp) {
		menInSquad = mis;

		actionPoints = actionPointsActual = actP;

		howManyToMove = hmta;
		howManyToAttack = hmta;
		howManyToFire = hmtf;

		canFire = cf;

		unitStrength = us;
		unitDurability = ud;
		unitArmour = ua;

		squadRootHex = hex;

		GameObject[] hexNeighbours = squadRootHex.GetComponent<hexProperties>().hexNeighbors;
		List<GameObject> unitHexes = new List<GameObject>();
		unitHexes.Add(squadRootHex);
		GameObject neighbour;
		for (int k = 0 ; unitHexes.Count < mis + 1 ; k++) {
			for (int l = 0 ; l < 6 ; l++) {
				neighbour = unitHexes[k].GetComponent<hexProperties>().hexNeighbors[l];
				if (neighbour != null && neighbour.GetComponent<hexProperties>().IsAvaliable() && unitHexes.IndexOf(neighbour) == -1 && neighbour.GetComponent<hexProperties>().IsFree())
					unitHexes.Add(neighbour);
			}
		}

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
			int which = Random.Range(0, unitHexes.Count - 1);
			GameObject unitHex = unitHexes[which];
			squad[j].GetComponent<UnitProperties>().init(hp, gameObject, unitHex, j);
			unitHexes.Remove(unitHex);
		}
	}
}
