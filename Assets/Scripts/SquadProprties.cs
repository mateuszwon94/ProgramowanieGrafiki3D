using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquadProprties : MonoBehaviour {

	public int menInSquad;

	public GameObject squadRootHex;

	public List<GameObject> squad;

	public int actionPoints;
	public int actionPointsActual;

	public int howManyToMove;
	public int howManyToAttack;
	public int howManyToFire;

	public bool canFire;

	public int unitStrength;
	public int unitDurability;
	public int unitArmour;

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
		foreach (GameObject neighbour in hexNeighbours) {
			if (neighbour != null && neighbour.GetComponent<hexProperties>().IsAvaliable())
				unitHexes.Add(neighbour);
		}
		int k = 0;
		while (unitHexes.Count < mis + 1 ) {
			if (hexNeighbours[k].GetComponent<hexProperties>().hexNeighbors == null) {
				continue;
			}
			foreach (GameObject neighbour in hexNeighbours[k].GetComponent<hexProperties>().hexNeighbors) {
				if (neighbour != null && neighbour.GetComponent<hexProperties>().IsAvaliable())
					unitHexes.Add(neighbour);
			}
			k++;
		}

		for (int j = 0; j < mis; j++){
			squad.Add((GameObject)Instantiate(what, new Vector3(0, 0, 0), Quaternion.identity));
			if (i == 0) {
				squad[j].transform.name = "Arcger "+j.ToString();
			}
			else if (i == 1){
				squad[j].transform.name = "Infantry " + j.ToString();
			}
			else if (i == 2){
				squad[j].transform.name = "Cavalery " + j.ToString();
			}
			else if (i == 3) {
				squad[j].transform.name = "Cavalery " + j.ToString();
			}
			int which = Random.Range(0, unitHexes.Count - 1);
			GameObject unitHex = unitHexes[which];
			squad[j].GetComponent<UnitProperties>().init(hp, gameObject, unitHex);
			unitHexes.Remove(unitHex);
		}
		
	}

	/*void Update() {
		if (squadRootHex.GetComponent<hexProperties>().isMouseOn) {
			
		}
	}*/
}
