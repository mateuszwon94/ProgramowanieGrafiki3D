using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingAStar : MonoBehaviour {

	class pathFinderNode : IComparable<pathFinderNode> {
		//Klasa przechowuje wszystkie potrzebne informacje
		public GameObject hex;
		public pathFinderNode father; //hex z ktorego jest najblizej do trzmanego przez ta instancje hexa

		public int costToNode; //Wpisany koszt przejscia od pozatku do obecnego hexa
		public int costFromNode; //Wyznaczony przyblizony koszt przejscia od tego hexa do ostatniego hexa

		public pathFinderNode(GameObject presentHex, GameObject endHex) { //konstruktor uzywany do zainicjalizowania pola startowego
			hex = presentHex;
			father = null;
			costToNode = 0;
			costFromNode = HeursticFunc(endHex);
		}

		public pathFinderNode(GameObject presentHex, GameObject endHex, int cost, pathFinderNode fat) { //Konstruktor uzywany do kazdego innego hexa
			hex = presentHex;
			father = fat;
			costToNode = fat.costToNode + cost;
			costFromNode = HeursticFunc(endHex);

		}

		public int TotalCost() { //zwraca calkowity koszt przejscia z hexa poczatkowego przez hex obecny do koncowego
			return costToNode + costFromNode;
		}

		public int CompareTo(pathFinderNode other) { //Uzywane do porownywania hexow przy sortowaniu
			if (TotalCost() == other.TotalCost()) {
				return costToNode - other.costToNode;
			}
			return TotalCost() - other.TotalCost();
		}

		/*
		 * Tutaj pojawia sie szereg funkcji heurystycznych na rozny sposob oszacowujacy odleglosc hexa obecnego od koncowego
		*/

		//NIEDOSZACOWUJE
		/*public int HeursticFunc(GameObject endHex) {
			double x = endHex.GetComponent<hexProperties>().hexPosX - hex.GetComponent<hexProperties>().hexPosX;
			double y = endHex.GetComponent<hexProperties>().hexPosY - hex.GetComponent<hexProperties>().hexPosY;
			return (int)(Math.Sqrt((double)(x * x + y * y)));
		}*/

		public int HeursticFunc(GameObject endHex) {
			double x = endHex.GetComponent<hexProperties>().hexPosX - hex.GetComponent<hexProperties>().hexPosX;
			double y = endHex.GetComponent<hexProperties>().hexPosY - hex.GetComponent<hexProperties>().hexPosY;
			double z = endHex.GetComponent<hexProperties>().hexPosZ - hex.GetComponent<hexProperties>().hexPosZ;
			return (int)(Math.Sqrt((double)(x * x + y * y + z * z)));
		}

		//PRZESZACOWUJE
		/*public int HeursticFunc(GameObject endHex) {
			return Math.Abs(endHex.GetComponent<hexProperties>().hexPosX - hex.GetComponent<hexProperties>().hexPosX) + Math.Abs(endHex.GetComponent<hexProperties>().hexPosY - hex.GetComponent<hexProperties>().hexPosY);
		}*/

		/*public int HeursticFunc(GameObject endHex) {
			return Math.Abs(endHex.GetComponent<hexProperties>().hexPosX - hex.GetComponent<hexProperties>().hexPosX) + Math.Abs(endHex.GetComponent<hexProperties>().hexPosY - hex.GetComponent<hexProperties>().hexPosY) + Math.Abs(endHex.GetComponent<hexProperties>().hexPosZ - hex.GetComponent<hexProperties>().hexPosZ);
		}*/
	}

	

	public List<GameObject> FindPathTo(GameObject startHex, GameObject endHex) { //Funkcja wyznacza sciezke od hexa startowego do koncowego
		if (endHex == null) { //Jesli hex nie istnieje zwroc NIC!
			return null;
		}

		List<pathFinderNode> listOpened = new List<pathFinderNode>(); //Lista hexow do sprzwdzenia
		List<pathFinderNode> listClosed = new List<pathFinderNode>(); //Lista hexow juz sprawdzonych
		pathFinderNode startHexNode = new pathFinderNode(startHex, endHex);
		int costFromNodeToNode = 1; //koszt przejscia z hexa do hexa (w przyszlosci jeszcze dojdzie tu hex poruszania sie po konkretnym hexie w zaleznosci od terenu)

		listClosed.Add(startHexNode);

		//Dodanie do listy DoSprawdzenia sasiadow startowego hexa odpowiednioe zainicjalizowanych
		foreach (GameObject presentHex in startHex.GetComponent<hexProperties>().hexNeighbors) {
			if (presentHex != null && (presentHex.GetComponent<hexProperties>().IsAvaliable())) {
				listOpened.Add(new pathFinderNode(presentHex, endHex, costFromNodeToNode, startHexNode));
			}
		}
		listOpened.Reverse(); //odwracamy bo lepiej jest wybierac ostatni z dodanych hexow o rownej ilosci calkowitego kosztu 
		listOpened.Sort(); //Sortujemy zeby odpowiedni hex byl na 0 miejscu w tablicy. Sortowanie jest stabilne (qsort albo heapsort w zaleznosci od ilosci elementow w liscie

		pathFinderNode presentHexNode;
		while (true) {
			if (listOpened.Count == 0) { //jesli nie ma juz hexow do przebadania znaczy to, ze przebadalismy wszystkie mozliwe a do wlasciwego nie dotarlismy, tzn nie da sie do niego dojsc. Sciezka nie istnieje wiec zwracamy NIIIIC! HUE HUE HUE :D
				return null;
			}

			presentHexNode = listOpened[0]; //Odczytanie wlasciwego hexa
			listClosed.Add(presentHexNode); //Dodanie go do przebadanych zeby go jeszcze raz nie badac
			listOpened.RemoveAt(0); //A jako ze sie wyroznial na tle innych hexow z listy do sprawdzenia juz go tam nie chca
			if (presentHexNode.hex == endHex) { //jesli jest to ostatni hex to je je je je i wychodzimy z petli
				break;
			}

			foreach (GameObject presentHex in presentHexNode.hex.GetComponent<hexProperties>().hexNeighbors) { //Sprawdzenie czy nie ma go w liscie sprawdzonej przypadkiem juz
				bool isInListClosed = false;
				foreach (pathFinderNode hexNode in listClosed) {
					if (hexNode.hex == presentHex) {
						isInListClosed = true;
						break;
					}
				}

				foreach (pathFinderNode hexNode in listOpened) { //Ustawienie odpowiednich wartosci dla kosztu do hexa i jego ojca jesli przez obecny hex jst do nieg oblizej (biedny hex... dopiero po takim czasie dowiaduje sie kto tak na prawde jest jego ojcem)
					if (hexNode.hex == presentHex && (presentHexNode.costToNode + costFromNodeToNode < hexNode.costToNode)) {
						hexNode.costToNode = presentHexNode.costToNode + costFromNodeToNode;
						hexNode.father = presentHexNode;
					}
				}

				if (presentHex != null && (presentHex.GetComponent<hexProperties>().IsAvaliable()) && !(isInListClosed)) { //Jesli hex jest dostepny i nie jest na liscie sprawdzonej dodajemy go do listy do sprawdzonia
					listOpened.Add(new pathFinderNode(presentHex, endHex, costFromNodeToNode, presentHexNode));
				}
			}
			listOpened.Reverse(); //odwracamy bo lepiej jest wybierac ostatni z dodanych hexow o rownej ilosci calkowitego kosztu 
			listOpened.Sort(); //Sortujemy zeby odpowiedni hex byl na 0 miejscu w tablicy. Sortowanie jest stabilne (qsort albo heapsort w zaleznosci od ilosci elementow w liscie
		}

		//Utworzenie sciezki
		List<GameObject> path = new List<GameObject>();
		listClosed.Reverse();
		for (pathFinderNode HexNode = listClosed[0]; HexNode != null; HexNode = HexNode.father) { //Idziemy od hexa koncowego przez wszystkich ojcow az ktos nie bedzie miec ojca (biedny ;(... smuteczek)
			path.Add(HexNode.hex);
		}

		listOpened.Clear(); //usuniecie niepotrzybnych juz list
		listClosed.Clear();

		return path; //I SUKCES!!! zwracamy sciezke :)
	}

}
