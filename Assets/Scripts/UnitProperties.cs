using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitProperties : MonoBehaviour {
	public int menHP; 
	public GameObject unitHex;

	public GameObject inWhichSquad;

	public int whichInSquad;

	public GameObject helthBar;

	public void init(int HP, GameObject squad, GameObject hex, int WhichInSquad) {
		/*
		 * Funkcja inicjalizujaca jednostke
		 */
		gameObject.transform.parent = squad.transform;
		menHP = HP;
		inWhichSquad = squad;
		unitHex = hex;
		unitHex.GetComponent<hexProperties>().PutOnHex(gameObject);
		whichInSquad = WhichInSquad;
	}

	public void ChangeColor(Color what) {
		//zmienia kolor jednostki
		gameObject.GetComponentInChildren<MeshRenderer>().material.color = what;
	}

	public void MoveTo(GameObject hex){
		//przemiezcza jednostke
		GameObject myself = unitHex.GetComponent<hexProperties>().PopFromHex();
		hex.GetComponent<hexProperties>().PutOnHex(myself);
		unitHex = hex;
	}

	public void TakeFireDamege(int howMany) {
		//jednostka otrzymuje odpowiednia ilosc obrazen
		menHP -= howMany;
		helthBar.GetComponent<HelthBar>().ReScale(menHP, inWhichSquad.GetComponent<SquadProprties>().startHP);
		if (menHP < 0)
			Die();
	}

	public void Die() {
		//umieranie jednostki
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
