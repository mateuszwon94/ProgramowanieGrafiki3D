using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectUnit : MonoBehaviour {
	public GameObject whichSquadIsSelect = null;
	public List<GameObject> whichHexIsSelected = new List<GameObject>();
	public GameObject curSquad = null;
	public GameObject curHex = null;
	public GameObject terrain;

	float selectionFadeSpeed = 0.1f;
	float fadeTolerance = 0.05f;

	void SelectHexes(List<GameObject> hexes) {
		if ((1 - hexes[0].GetComponent<hexProperties>().GetSelectionAlpha(1)) > fadeTolerance) {
			foreach (GameObject hex in hexes) {
				hex.GetComponent<hexProperties>().SetSelectionAlpha(Mathf.Lerp(hex.GetComponent<hexProperties>().GetSelectionAlpha(1), 1, Time.deltaTime * selectionFadeSpeed), 1);
			}
		}
	}

	void UnSelectHexes(List<GameObject> hexes) {
		if (hexes[0].GetComponent<hexProperties>().GetSelectionAlpha(1) > fadeTolerance) {
			foreach (GameObject hex in hexes) {
				hex.GetComponent<hexProperties>().SetSelectionAlpha(Mathf.Lerp(hex.GetComponent<hexProperties>().GetSelectionAlpha(1), 0, Time.deltaTime * selectionFadeSpeed), 1);
			}
		}
	}

	void Update() {
		curHex = terrain.GetComponent<MouseOnHex>().currentHex;
		if (curHex != null && !curHex.GetComponent<hexProperties>().IsFree())
			curSquad = curHex.GetComponent<hexProperties>().GetFromHex().GetComponent<UnitProperties>().inWhichSquad;
		if (curHex && curHex.GetComponent<hexProperties>().IsAvaliable()) {
			if (curHex != null) {
				if (Input.anyKey) {
					if (Input.GetButton("Mouse 1") && curSquad != null) {
						if (whichSquadIsSelect != curSquad && whichSquadIsSelect != null) {
							UnSelectHexes(whichHexIsSelected);
							for (int i = 0 ; i < whichHexIsSelected.Count ; ++i) {
								whichHexIsSelected.RemoveAt(0);
							}
							whichSquadIsSelect.GetComponent<SquadProprties>().isSelected = false;
							curSquad.GetComponent<SquadProprties>().isSelected = true;
							whichSquadIsSelect = curSquad;
							for (int i = 0 ; i < whichSquadIsSelect.GetComponent<SquadProprties>().squad.Count ; ++i) {
								GameObject unit = whichSquadIsSelect.GetComponent<SquadProprties>().squad[i];
								whichHexIsSelected.Add(unit.GetComponent<UnitProperties>().unitHex);
							}
							SelectHexes(whichHexIsSelected);
						}
						else if (whichSquadIsSelect == curSquad) {
							UnSelectHexes(whichHexIsSelected);
							for (int i = 0 ; i < whichHexIsSelected.Count ; ++i) {
								whichHexIsSelected.RemoveAt(0);
							}
							whichSquadIsSelect.GetComponent<SquadProprties>().isSelected = false;
							whichSquadIsSelect = null;
						}
						else if (whichSquadIsSelect != curSquad && whichSquadIsSelect == null) {
							curSquad.GetComponent<SquadProprties>().isSelected = true;
							whichSquadIsSelect = curSquad;
							for (int i = 0 ; i < whichSquadIsSelect.GetComponent<SquadProprties>().squad.Count ; ++i) {
								GameObject unit = whichSquadIsSelect.GetComponent<SquadProprties>().squad[i];
								whichHexIsSelected.Add(unit.GetComponent<UnitProperties>().unitHex);
							}
							SelectHexes(whichHexIsSelected);
						}
					}
				}
			}
		}
	}
}
