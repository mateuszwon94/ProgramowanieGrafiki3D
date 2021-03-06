﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MouseOnHex : MonoBehaviour {
	/*
	 * obsluga poruszania mysza po ekranie
	 * tak zeby zaznaczal sie odpowiedni hex
	 * wykorzystanie matematyki opartej o uklad hexowy :D
	 */

	static float sqrt3 = (float)Math.Sqrt(3);

	int x = 38;
	int y = 38;
	int size;
	int howManyToCut;

	public GameObject currentHex;
	GameObject previousHex = null;

	void Start() {
		currentHex = gameObject.GetComponent<SpawnHexes>().hexGrid[x, y];
		size = gameObject.GetComponent<SpawnHexes>().size;

		if ((size % 2) == 1) {
			howManyToCut = size / 2;
		}
		else {
			howManyToCut = (size / 2) - 1;
		}
	}

	void Update() {
		previousHex = currentHex;

		if (Input.mousePosition.y > 100) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
				x = (int)Math.Floor(38 + (((1f / 3f) * sqrt3 * hit.point.x - (1f / 3f) * hit.point.z)));
				y = (int)Math.Floor(38 + ((2f / 3f) * hit.point.z));

				if ((x + y > howManyToCut) && (x + y < 2 * size - howManyToCut - 2)) {
					try {
						currentHex = gameObject.GetComponent<SpawnHexes>().hexGrid[x, y];
					}
					catch (IndexOutOfRangeException) {
					}
				}
				else {
					currentHex = null;
				}
			}
		}
		else {
			currentHex = null;
		}

		//Odpowiednie zaznaczanie i odznaczanie hexa
		if (currentHex != null && currentHex.GetComponent<hexProperties>().IsAvaliable())
			currentHex.GetComponent<hexProperties>().isMouseOn = true;
		if (previousHex != null && previousHex != currentHex) {
			previousHex.GetComponent<hexProperties>().isMouseOn = false;
		}
	}
}