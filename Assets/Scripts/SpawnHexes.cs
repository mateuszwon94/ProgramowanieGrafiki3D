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
					// odwrotnie do ruchu wskazowek zegara od elementu pod hexem
					try {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[0] = hexGrid[i, j - 1];
					}
					catch (IndexOutOfRangeException) {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[0] = null;
					}

					try {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[1] = hexGrid[i - 1, j];
					}
					catch (IndexOutOfRangeException) {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[1] = null;
					}

					try {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[2] = hexGrid[i - 1, j + 1];
					}
					catch (IndexOutOfRangeException) {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[2] = null;
					}

					try {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[3] = hexGrid[i, j + 1];
					}
					catch (IndexOutOfRangeException) {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[3] = null;
					}

					try {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[4] = hexGrid[i + 1, j];
					}
					catch (IndexOutOfRangeException) {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[4] = null;
					}

					try {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[5] = hexGrid[i + 1, j - 1];
					}
					catch (IndexOutOfRangeException) {
						hexGrid[i, j].GetComponent<hexProperties>().hexNeighbors[5] = null;
					}

				}

			}

		}

	}

}
