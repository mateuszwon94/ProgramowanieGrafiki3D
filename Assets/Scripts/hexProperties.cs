using System.Collections;
using UnityEngine;

public class hexProperties : MonoBehaviour {

	GameObject onHex = null;

	public GameObject[] hexNeighbors = new GameObject[6]; //hexNeighbors[0] => hex na pozycji (posX,posY-1) [hex "pod"] nastepnie zgodnie z ruchem wskazowek zegara

	bool isAvaliable = true;
	public bool isVisable = true;

	public int hexPosX;
	public int hexPosY;
	public int hexPosZ;

	public bool isMouseOn = false;
	public bool isToggle = false;

	public void ChangeVisibility(int size) {
		int howManyToCut;
		if ((size%2)==1) {
			howManyToCut = size/2;
		}
		else {
			howManyToCut = (size/2) -1;
		}

		if ( hexPosX + hexPosY < howManyToCut ) {
			isVisable = false;
		}
		else if (hexPosX + hexPosY > 2*size-howManyToCut-2) {
			isVisable = false;
		}
	}

	public bool IsAvaliable() {
		return isAvaliable;
	}

	public GameObject GetFromHex() {
		return onHex;
	}

	public GameObject PopFromHex() {
		GameObject sth = onHex;
		onHex = null;
		return sth;
	}

	public void PutOnHex(GameObject Sth) {
		Sth.transform.position.Set(0f, 0f, 0f);
		Sth.transform.Translate(gameObject.transform.position.x, gameObject.transform.position.y + GameObject.FindGameObjectWithTag("Terrain").GetComponent<SpawnHexes>().hex.GetComponent<HexMesh>().heightOverTerrain, gameObject.transform.position.z);
		onHex = Sth;
	}

	public void ChangeHexColor(Color color) {
		gameObject.GetComponent<MeshRenderer>().materials[0].color = color;
	}
    
}
