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

	public Vector2 centerOfHex;

	public bool isMouseOn = false;	//czy myszka jest nad hexem
	public bool isToggle = false;	//czy hex zostal zaznaczony

	public float mouseOnFadeSpeed = 0.1f;
	public float fadeTolerance = 0.05f;

	public void ChangeVisibility(int size) {
		/* 
		 * Zmienia widzialnosc hexa.
		 * Wykorzystywane przez funkcje usuwajaca niepotrzebne hexy
		*/
		int howManyToCut;
		if ((size % 2) == 1) {
			howManyToCut = size / 2;
		}
		else {
			howManyToCut = (size / 2) - 1;
		}

		if (hexPosX + hexPosY < howManyToCut) {
			isVisable = false;
		}
		else if (hexPosX + hexPosY > 2 * size - howManyToCut - 2) {
			isVisable = false;
		}
	}

	public void ChangeAvaliablity() {
		if (isAvaliable)
			isAvaliable = false;
		else
			isAvaliable = true;
	}

	public void ChangeAvaliablity(bool forWhat) {
		isAvaliable = forWhat;
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
		Sth.transform.position = transform.position;
		onHex = Sth;
	}

	public void ChangeHexColor(Color color, int whichMaterial) { //zmienia kolor hexa (napisane by ulatwic se zycie i nie pieprzyc z tak dlugim kodem)
		gameObject.GetComponent<MeshRenderer>().materials[whichMaterial].color = color;
	}

	void SetSelectionAlpha (float newA) {
		Color tempColor = gameObject.GetComponent<MeshRenderer>().materials[1].color;
		tempColor.a = newA;
		gameObject.GetComponent<MeshRenderer>().materials[1].color = tempColor;
	}

	float GetSelectionAlpha () {
		return gameObject.GetComponent<MeshRenderer>().materials[1].color.a;
	}

	void Update () {
		if (isMouseOn == true && (1 - GetSelectionAlpha()) > fadeTolerance) {
			SetSelectionAlpha (Mathf.Lerp (GetSelectionAlpha (), 1, Time.deltaTime * mouseOnFadeSpeed));
		}
		else if (isMouseOn == false && GetSelectionAlpha() > fadeTolerance) {
			SetSelectionAlpha (Mathf.Lerp (GetSelectionAlpha (), 0, Time.deltaTime * mouseOnFadeSpeed));
		}
	}

}
