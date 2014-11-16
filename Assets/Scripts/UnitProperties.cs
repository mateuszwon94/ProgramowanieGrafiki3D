using UnityEngine;
using System.Collections;

public class UnitProperties : MonoBehaviour {
	public int menHP;
	public GameObject unitHex;
    
	public GameObject inWhichSquad;

	public void init(int hp, GameObject squad, GameObject hex) {
		gameObject.transform.parent = squad.transform;
		menHP = hp;
		inWhichSquad = squad;
		unitHex = hex;
		unitHex.GetComponent<hexProperties>().PutOnHex(gameObject);
	}
}
