using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectUnit : MonoBehaviour {
	public GameObject whichSquadIsSelect = null;
	public GameObject curSquad = null;
	public GameObject curHex = null;
	public GameObject terrain;

	public GameObject myArmy;

	public GameObject GUI;

	float selectionFadeSpeed = 0.01f;
	float fadeTolerance = 0.05f;

	public void ReSelectUnit(List<GameObject> oldHexes, List<GameObject> newHexes) {
		UnSelectHexes(oldHexes);
		SelectHexes(newHexes);
	}

	void SelectHexes(List<GameObject> hexes) {
		while ((1 - hexes[0].GetComponent<hexProperties>().GetSelectionAlpha(1)) > fadeTolerance) {
			foreach (GameObject hex in hexes) {
				hex.GetComponent<hexProperties>().SetSelectionAlpha(Mathf.Lerp(hex.GetComponent<hexProperties>().GetSelectionAlpha(1), 1, Time.deltaTime * selectionFadeSpeed), 1);
			}
		}
	}

	void UnSelectHexes(List<GameObject> hexes) {
		while (hexes[0].GetComponent<hexProperties>().GetSelectionAlpha(1) > fadeTolerance) {
			foreach (GameObject hex in hexes) {
				hex.GetComponent<hexProperties>().SetSelectionAlpha(Mathf.Lerp(hex.GetComponent<hexProperties>().GetSelectionAlpha(1), 0, Time.deltaTime * selectionFadeSpeed), 1);
			}
		}
	}

	public void Update() {
		if (GUI.GetComponent<GUIInput>().GetGameMode() >= 0) {
			curHex = terrain.GetComponent<MouseOnHex>().currentHex;
			if (curHex != null && !curHex.GetComponent<hexProperties>().IsFree() && curHex.GetComponent<hexProperties>().GetFromHex().GetComponent<UnitProperties>().inWhichSquad.GetComponent<SquadProprties>().isControlByHuman)
				curSquad = curHex.GetComponent<hexProperties>().GetFromHex().GetComponent<UnitProperties>().inWhichSquad;
			else {
				curSquad = null;
			}

			if (whichSquadIsSelect != null) {
				myArmy = whichSquadIsSelect.GetComponent<SquadProprties>().inWhichArmy;
				List<GameObject> squads = myArmy.GetComponent<ArmyProperties>().squads;
				foreach (GameObject squad in squads) {
					if (!squad.GetComponent<SquadProprties>().isSelected) {
						UnSelectHexes(squad.GetComponent<SquadProprties>().unitHexes);
					}
				}
			}

			if (curHex != null && curHex.GetComponent<hexProperties>().IsAvaliable() && Input.anyKeyDown) {
				if (Input.GetButton("Mouse 1")) {
					if (curSquad == null && whichSquadIsSelect != null) {
						UnSelectHexes(whichSquadIsSelect.GetComponent<SquadProprties>().unitHexes);
						whichSquadIsSelect.GetComponent<SquadProprties>().isSelected = false;
						whichSquadIsSelect = null;
					}
					else if (whichSquadIsSelect != curSquad && whichSquadIsSelect != null) {
						UnSelectHexes(whichSquadIsSelect.GetComponent<SquadProprties>().unitHexes);
						whichSquadIsSelect.GetComponent<SquadProprties>().isSelected = false;


						curSquad.GetComponent<SquadProprties>().isSelected = true;
						whichSquadIsSelect = curSquad;
						SelectHexes(whichSquadIsSelect.GetComponent<SquadProprties>().unitHexes);
					}
					else if (whichSquadIsSelect == curSquad && curSquad != null) {
						UnSelectHexes(whichSquadIsSelect.GetComponent<SquadProprties>().unitHexes);
						whichSquadIsSelect.GetComponent<SquadProprties>().isSelected = false;
						whichSquadIsSelect = null;
					}
					else if (whichSquadIsSelect != curSquad && whichSquadIsSelect == null) {
						curSquad.GetComponent<SquadProprties>().isSelected = true;
						whichSquadIsSelect = curSquad;
						SelectHexes(whichSquadIsSelect.GetComponent<SquadProprties>().unitHexes);
					}
				}
			}
		}
	}
}
