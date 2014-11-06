using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHexes : MonoBehaviour {

	static float sqrt3 = (float)Math.Sqrt(3);

	float startPosX = -95f;
	float startPosY = 500f;
	float startPosZ = -95f;

	int size = 75;

	public GameObject hex;
	public GameObject[,] hexGrid;

	GameObject currentHex;

	float spawnX;
	float spawnZ = 0f;

	void Awake() {
		hexGrid = new GameObject[size, size];
		GameObject grid = new GameObject ("Grid");

		for (int i = 0; i < size; i++) {
			spawnX = sqrt3*i;
			GameObject hexColumn = new GameObject ("Hex Column "+i.ToString());
			hexColumn.transform.parent = grid.transform;

			for (int j = 0; j < size; j++) {
				currentHex = (GameObject)Instantiate(hex, new Vector3(startPosX+spawnX,startPosY,startPosZ+spawnZ), Quaternion.identity);
				currentHex.transform.parent = hexColumn.transform;
				currentHex.transform.name = "hex (" + i.ToString()+","+j.ToString()+")";
				hexGrid[i,j] = currentHex;
				hexGrid[i,j].GetComponent<hexProperties>().hexPosX = i;
				hexGrid[i,j].GetComponent<hexProperties>().hexPosY = j;
				hexGrid[i, j].GetComponent<hexProperties>().hexPosZ = -i - j;
				hexGrid[i,j].GetComponent<hexProperties>().ChangeVisibility(size);
				if (! hexGrid[i,j].GetComponent<hexProperties>().isVisable){
					Destroy(hexGrid[i,j]);
					hexGrid[i,j] = null;
				}
				spawnZ += 1.5f;
				spawnX += sqrt3 / 2f;
			}
			spawnZ = 0;
		}

		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				if (hexGrid[i, j] != null) {
					hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors = GetNeighbors(hexGrid[i, j]);
				}
			}
		}
	}

	GameObject [] GetNeighbors (GameObject currentHex) {

		hexProperties currentProperties = currentHex.GetComponent<hexProperties>();
		int hexPosX = currentProperties.hexPosX;
		int hexPosY = currentProperties.hexPosY;

		GameObject [] neighbors = new GameObject[6];
		
		try {
			neighbors[0] = hexGrid[hexPosX, hexPosY - 1];
		}
		catch (IndexOutOfRangeException) {}
		try {
			neighbors[1] = hexGrid[hexPosX - 1, hexPosY];
		}
		catch (IndexOutOfRangeException) {}
		try {
			neighbors[2] = hexGrid[hexPosX - 1, hexPosY + 1];
		}
		catch (IndexOutOfRangeException) {}
		try {
			neighbors[3] = hexGrid[hexPosX, hexPosY + 1];
		}
		catch (IndexOutOfRangeException) {}
		try {
			neighbors[4] = hexGrid[hexPosX + 1, hexPosY];
		}
		catch (IndexOutOfRangeException) {}
		try {
			neighbors[5] = hexGrid[hexPosX + 1, hexPosY - 1];
		}
		catch (IndexOutOfRangeException) {}

		return neighbors;
	}

	public GameObject FindHexWithPosition (float x, float y) {
		int q = Mathf.RoundToInt( ((1/3 * sqrt3 * x) - (1/3 * y)) / size );
		int r = Mathf.RoundToInt( 2/3 * y / size );

		GameObject temp = hexGrid[q, r];

		if (temp == null) Debug.LogError ("Cannot find appropriate hex.");
		return temp;
	}
}
