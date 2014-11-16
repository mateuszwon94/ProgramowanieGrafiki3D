using UnityEngine;
using System.Collections;

public class UnitProperties : MonoBehaviour {
	public int menHP;
	public GameObject unitHex;
    
	public GameObject inWhichSquad;

	public int whichInSquad;

	public bool isToggled = false;

	public void init(int hp, GameObject squad, GameObject hex, int wis) {
		gameObject.transform.parent = squad.transform;
		menHP = hp;
		inWhichSquad = squad;
		unitHex = hex;
		unitHex.GetComponent<hexProperties>().PutOnHex(gameObject);
		whichInSquad = wis;
	}

	public void ToggleUnit() {
		inWhichSquad.GetComponent<SquadProprties>().ToggleSquad(whichInSquad);
	}

}
