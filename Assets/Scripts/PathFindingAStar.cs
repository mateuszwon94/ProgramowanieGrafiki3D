using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingAStar : MonoBehaviour {

	class pathFinderNode : IComparable<pathFinderNode> {
		public GameObject hex;
		public pathFinderNode father;
		public int costToNode;
		public int costFromNode;

		public pathFinderNode(GameObject presentHex, GameObject endHex) {
			hex = presentHex;
			father = null;
			costToNode = 0;
			costFromNode = HeursticFunc(endHex);
		}

		public pathFinderNode(GameObject presentHex, GameObject endHex, int cost, pathFinderNode fat) {
			hex = presentHex;
			father = fat;
			costToNode = fat.costToNode + cost;
			costFromNode = HeursticFunc(endHex);

		}

		public int TotalCost() {
			return costToNode + costFromNode;
		}

		//NIEDOSZACOWUJE
		/*public int HeursticFunc(GameObject endHex) {
			double x = endHex.GetComponent<hexProperties>().hexPosX - hex.GetComponent<hexProperties>().hexPosX;
			double y = endHex.GetComponent<hexProperties>().hexPosY - hex.GetComponent<hexProperties>().hexPosY;
			return (int)(Math.Sqrt((double)(x * x + y * y)));
		}*/

		/*public int HeursticFunc(GameObject endHex) {
			double x = endHex.GetComponent<hexProperties>().hexPosX - hex.GetComponent<hexProperties>().hexPosX;
			double y = endHex.GetComponent<hexProperties>().hexPosY - hex.GetComponent<hexProperties>().hexPosY;
			double z = endHex.GetComponent<hexProperties>().hexPosZ - hex.GetComponent<hexProperties>().hexPosZ;
			return (int)(Math.Sqrt((double)(x * x + y * y + z * z)));
		}*/

		//PRZESZACOWUJE
		/*public int HeursticFunc(GameObject endHex) {
			return Math.Abs(endHex.GetComponent<hexProperties>().hexPosX - hex.GetComponent<hexProperties>().hexPosX) + Math.Abs(endHex.GetComponent<hexProperties>().hexPosY - hex.GetComponent<hexProperties>().hexPosY);
		}*/

		public int HeursticFunc(GameObject endHex) {
			return Math.Abs(endHex.GetComponent<hexProperties>().hexPosX - hex.GetComponent<hexProperties>().hexPosX) + Math.Abs(endHex.GetComponent<hexProperties>().hexPosY - hex.GetComponent<hexProperties>().hexPosY) + Math.Abs(endHex.GetComponent<hexProperties>().hexPosZ - hex.GetComponent<hexProperties>().hexPosZ);
		}

		//NIE DZIALA NAJGORZEJ ALE CHYBA TAMTE DWA SA LEPSZE
		/*public int HeursticFunc(GameObject endHex) {
			int xSteps = Math.Abs(hex.GetComponent<hexProperties>().hexPosX - endHex.GetComponent<hexProperties>().hexPosX);
			int ySteps = Math.Abs(hex.GetComponent<hexProperties>().hexPosY - endHex.GetComponent<hexProperties>().hexPosY);

			return Math.Max(xSteps, ySteps) + Math.Abs(xSteps - ySteps);
		}*/

		//BAAAAARDZO ZLY ZWRACA ZAPETLAJACE SIE SCIEZKI
		/*public int HeursticFunc(GameObject endHex) {
			if (hex.GetComponent<hexProperties>().hexPosX == endHex.GetComponent<hexProperties>().hexPosX)
				return hex.GetComponent<hexProperties>().hexPosX - endHex.GetComponent<hexProperties>().hexPosX;
			else if (hex.GetComponent<hexProperties>().hexPosY == endHex.GetComponent<hexProperties>().hexPosY)
				return hex.GetComponent<hexProperties>().hexPosY - endHex.GetComponent<hexProperties>().hexPosY;
			else {
				int dx = Math.Abs(hex.GetComponent<hexProperties>().hexPosX - endHex.GetComponent<hexProperties>().hexPosX);
				int dy = Math.Abs(hex.GetComponent<hexProperties>().hexPosY - endHex.GetComponent<hexProperties>().hexPosY);

				if (hex.GetComponent<hexProperties>().hexPosY < endHex.GetComponent<hexProperties>().hexPosY)
					return dx + dy - (int)Math.Ceiling(dx / 2f);
				else
					return dx + dy - (int)Math.Floor(dx / 2f);
			}*/
		
		//PODOBNIE DO PRZESZACOWANIA CHODZI TROSZKU PO PROSTYCH BARDZO
		/*public int HeursticFunc(GameObject endHex) {
			int yDistance = Math.Abs((hex.GetComponent<hexProperties>().hexPosY) - (endHex.GetComponent<hexProperties>().hexPosY));
			int xDistance = Math.Abs(hex.GetComponent<hexProperties>().hexPosX - endHex.GetComponent<hexProperties>().hexPosX);
			int maxDistance = Math.Max(xDistance,yDistance);

			int something = Math.Abs(-endHex.GetComponent<hexProperties>().hexPosY + endHex.GetComponent<hexProperties>().hexPosX + hex.GetComponent<hexProperties>().hexPosY - hex.GetComponent<hexProperties>().hexPosX);
			return Math.Max(something, maxDistance);
		}*/

		public int CompareTo(pathFinderNode other) {
			if (TotalCost() == other.TotalCost()) {
				return costToNode - other.costToNode;
			}
			return TotalCost() - other.TotalCost();
		}
	}

	

	public List<GameObject> FindPathTo(GameObject startHex, GameObject endHex) {
		if (endHex == null) {
			return null;
		}

		List<pathFinderNode> listOpened = new List<pathFinderNode>();
		List<pathFinderNode> listClosed = new List<pathFinderNode>();
		pathFinderNode startHexNode = new pathFinderNode(startHex, endHex);
		int costFromNodeToNode = 1;

		listClosed.Add(startHexNode);

		foreach (GameObject presentHex in startHex.GetComponent<hexProperties>().hexNeighbors) {
			if (presentHex != null && (presentHex.GetComponent<hexProperties>().IsAvaliable())) {
				listOpened.Add(new pathFinderNode(presentHex, endHex, costFromNodeToNode, startHexNode));
			}
		}
		listOpened.Reverse();
		listOpened.Sort();

		pathFinderNode presentHexNode;
		while (true) {
			if (listOpened.Count == 0) {
				return null;
			}

			presentHexNode = listOpened[0];
			listClosed.Add(presentHexNode);
			listOpened.RemoveAt(0);
			if (presentHexNode.hex == endHex) {
				break;
			}

			foreach (GameObject presentHex in presentHexNode.hex.GetComponent<hexProperties>().hexNeighbors) {
				bool isInListClosed = false;
				foreach (pathFinderNode hexNode in listClosed) {
					if (hexNode.hex == presentHex) {
						isInListClosed = true;
						break;
					}
				}

				foreach (pathFinderNode hexNode in listOpened) {
					if (hexNode.hex == presentHex && (presentHexNode.costToNode + costFromNodeToNode < hexNode.costToNode)) {
						hexNode.costToNode = presentHexNode.costToNode + costFromNodeToNode;
						hexNode.father = presentHexNode;
					}
				}

				if (presentHex != null && (presentHex.GetComponent<hexProperties>().IsAvaliable()) && !(isInListClosed)) {
					listOpened.Add(new pathFinderNode(presentHex, endHex, costFromNodeToNode, startHexNode));
				}
			}
			listOpened.Reverse();
			listOpened.Sort();
		}

		List<GameObject> path = new List<GameObject>();
		foreach (pathFinderNode HexNode in listClosed) {
			path.Add(HexNode.hex);
		}

		listOpened.Clear();
		listClosed.Clear();
		return path;
	}

}
