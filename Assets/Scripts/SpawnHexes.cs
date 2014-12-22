using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHexes : MonoBehaviour {

	static float sqrt3 = (float)Math.Sqrt(3);

	//wielkosc planszu, zarowno wiersze jak i kolumny
	public int size = 75;

	//Pozycja pierwszego hexa polozonego na planszy
	float startPosX = -(75f *1.5f* sqrt3)/2f;
	float startPosY = 500f;
	float startPosZ = -(75f / 2f) * 1.5f;

	public GameObject hex; //Tu jest przetrzymywany (ciekawe za co) prefab hexa
	public GameObject[,] hexGrid; //Tu znowu siatka wygenerowanych hexow

	GameObject currentHex;

	float spawnX;
	float spawnZ = 0f;

	void Awake() {
		hexGrid = new GameObject[size, size];
		GameObject grid = new GameObject("Grid"); //do ladnej organizacji w UnityEdytorze

		for (int i = 0; i < size; i++) {

			spawnX = sqrt3 * i;
			GameObject hexColumn = new GameObject("Hex Column " + i.ToString()); //tez do ladnej organizacji w UnityEdytorze
			hexColumn.transform.parent = grid.transform; //przypisanie HexKolumnie jako rodzica siatki

			for (int j = 0; j < size; j++) {

				currentHex = (GameObject)Instantiate(hex, new Vector3(startPosX + spawnX, startPosY, startPosZ + spawnZ), Quaternion.identity); //tworzenie hexa i ustawienie go na odpowiedniej pozycji
				currentHex.GetComponent<hexProperties>().centerOfHex = new Vector2(startPosX + spawnX, startPosZ + spawnZ);
				currentHex.transform.parent = hexColumn.transform; //Przypisanie hexowi jako rodzica HexKolumne
				currentHex.transform.name = "Hex (" + i.ToString() + "," + j.ToString() + ")"; //to też do ladnego wygladu w UnityEdytorze
				hexGrid[i, j] = currentHex;

				//Ustawienie odpowiedni wlasciwosci dla konkretnego hexa
				hexGrid[i, j].GetComponent<hexProperties>().hexPosX = i;
				hexGrid[i, j].GetComponent<hexProperties>().hexPosY = j;
				hexGrid[i, j].GetComponent<hexProperties>().hexPosZ = -i - j;

				//Usuwanie niepotrzebnych hexow z tablicy
				hexGrid[i, j].GetComponent<hexProperties>().ChangeVisibility(size);
				if (!hexGrid[i, j].GetComponent<hexProperties>().isVisable) {
					Destroy(hexGrid[i, j]);
					hexGrid[i, j] = null;
				}

				spawnZ += 1.5f;
				spawnX += sqrt3 / 2f;
			}
			spawnZ = 0;
		}

		//Ustawienie w tablicy sasiadow konkretnego hexa odpowiednich hexow :)
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				if (hexGrid[i, j] != null) {
					hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors = GetNeighbors(hexGrid[i, j]);
				}
			}
		}
	}

	GameObject[] GetNeighbors(GameObject currentHex) {
		//Obliczenie ktore hexy sa sasiadami tego podanego jako argument

		hexProperties currentProperties = currentHex.GetComponent<hexProperties>();
		int hexPosX = currentProperties.hexPosX;
		int hexPosY = currentProperties.hexPosY;

		GameObject[] neighbors = new GameObject[6];

		//Sprawdzenie czy hex o zadanych koordynatach istnieje. Jesli nie no to coz... hex jest biedny i ma mniej sasiadow i nie ma z kim pic
		try {
			neighbors[0] = hexGrid[hexPosX, hexPosY - 1];
		}
		catch (IndexOutOfRangeException) {
		}
		try {
			neighbors[1] = hexGrid[hexPosX - 1, hexPosY];
		}
		catch (IndexOutOfRangeException) {
		}
		try {
			neighbors[2] = hexGrid[hexPosX - 1, hexPosY + 1];
		}
		catch (IndexOutOfRangeException) {
		}
		try {
			neighbors[3] = hexGrid[hexPosX, hexPosY + 1];
		}
		catch (IndexOutOfRangeException) {
		}
		try {
			neighbors[4] = hexGrid[hexPosX + 1, hexPosY];
		}
		catch (IndexOutOfRangeException) {
		}
		try {
			neighbors[5] = hexGrid[hexPosX + 1, hexPosY - 1];
		}
		catch (IndexOutOfRangeException) {
		}

		return neighbors;
	}

	public GameObject FindHexWithPosition(float x, float y) {
		//Zwraca hexa na odpowiedniej pozycji
		int q = Mathf.RoundToInt(((1 / 3 * sqrt3 * x) - (1 / 3 * y)) / size);
		int r = Mathf.RoundToInt(2 / 3 * y / size);

		GameObject temp = hexGrid[q, r];

		if (temp == null)
			Debug.LogError("Cannot find appropriate hex.");
		return temp;
	}

}
