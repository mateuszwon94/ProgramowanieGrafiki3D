using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitProperties : MonoBehaviour {
	public int menHP;
	public GameObject unitHex;

	public GameObject inWhichSquad;

	public int whichInSquad;

	public GameObject helthBar;

	public void init(int hp, GameObject squad, GameObject hex, int wis) {
		gameObject.transform.parent = squad.transform;
		menHP = hp;
		inWhichSquad = squad;
		unitHex = hex;
		unitHex.GetComponent<hexProperties>().PutOnHex(gameObject);
		whichInSquad = wis;
	}

	public void ChangeColor(Color what) {
		gameObject.GetComponentInChildren<MeshRenderer>().material.color = what;
	}

	public void MoveTo(GameObject hex){
		GameObject myself = unitHex.GetComponent<hexProperties>().PopFromHex();
		hex.GetComponent<hexProperties>().PutOnHex(myself);
		unitHex = hex;
	}

	public void TakeFireDamege(int howMany) {
		menHP -= howMany;
		helthBar.GetComponent<HelthBar>().ReScale(menHP, inWhichSquad.GetComponent<SquadProprties>().startHP);
		if (menHP < 0)
			Die();
	}

	public void Die() {
		SquadProprties mySquad = inWhichSquad.GetComponent<SquadProprties>();
		mySquad.squad.RemoveAt(whichInSquad);
		for (int i = 0 ; i < inWhichSquad.GetComponent<SquadProprties>().squad.Count ; ++i)
			mySquad.squad[i].GetComponent<UnitProperties>().whichInSquad = i;
		--mySquad.menInSquad;
		++mySquad.deadMen;
		if (mySquad.deadMens == null)
			mySquad.deadMens = new List<GameObject>();
		mySquad.deadMens.Add(gameObject);
		gameObject.SetActive(false);

		unitHex.GetComponent<hexProperties>().PopFromHex();

		inWhichSquad.GetComponent<SquadProprties>().unitHexes.Remove(unitHex);

		unitHex = null;
		whichInSquad = -1;
		menHP = 0;
		
	}
}
