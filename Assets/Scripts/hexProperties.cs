using System.Collections;
using UnityEngine;

public class hexProperties : MonoBehaviour {

	GameObject onHex = null;

	public GameObject[] hexNeighbors = new GameObject[6]; //hexNeighbors[0] => hex na pozycji (posX,posY-1) [hex "pod"] nastepnie zgodnie z ruchem wskazowek zegara

	bool isAvaliable = true;		//czy można na hexie stanac
	public bool isVisable = true;	//Uzywane tymczasowo to wykluczenia z planszy hexow tak zeby sama plansza byla hexem 

	public int hexPosX;				//pozycja X hexa na plaszczyznie
	public int hexPosY;				//pozycja Y hexa na plaszczyznie
	public int hexPosZ;				//uzywane w niektorych algorytmach (ulatwia obliczenia)

	public bool isMouseOn = false;	//czy myszka jest nad hexem
	public bool isToggle = false;	//czy hex zostal zaznaczony

	public void ChangeVisibility(int size) {
		/* 
		 * Zmienia widzialnosc hexa.
		 * Wykorzystywane przez funkcje usuwajaca niepotrzebne hexy
		*/
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

	public bool IsAvaliable() { //Zwraca dostepnosc hexa
		return isAvaliable;
	}

	public GameObject GetFromHex() { //zwraca obiekt znajdujacy sie na hexie
		return onHex;
	}

	public GameObject PopFromHex() { //zdejmuje obiekt z hexa i go zwraca
		GameObject sth = onHex;
		onHex = null;
		return sth;
	}

	public void PutOnHex(GameObject Sth) { //kladzie obiekt na hexie
		Sth.transform.position.Set(0f, 0f, 0f);
		Sth.transform.Translate(gameObject.transform.position.x, gameObject.transform.position.y + GameObject.FindGameObjectWithTag("Terrain").GetComponent<SpawnHexes>().hex.GetComponent<HexMesh>().heightOverTerrain, gameObject.transform.position.z);
		onHex = Sth;
	}

	public void ChangeHexColor(Color color) { //zmienia kolor hexa (napisane by ulatwic se zycie i nie pieprzyc z tak dlugim kodem)
		gameObject.GetComponent<MeshRenderer>().materials[0].color = color;
	}
    
}
