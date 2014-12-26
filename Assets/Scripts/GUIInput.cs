using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUIInput : MonoBehaviour {

	int howManyGameModes = 4;

	public GameObject movmentGameMode;
	public GameObject movmentGameModeText;

	public GameObject fireGameMode;
	public GameObject fireGameModeText;

	public GameObject attackGameMode;
	public GameObject attackGameModeText;

	public GameObject deployGameMode;
	public GameObject deployGameModeText;

	public GameObject exitGameMode;
	public GameObject exitGameModeText;

	public GameObject obrazenia;
	public GameObject[] obrazeniaTexts = new GameObject[5];

	public GameObject terrain;

	int gameMode = -1;
	int prevGameMode = -1;

	GameObject whichSquadIsSelected;
	public GameObject whichSquadIsSelectedText;
	public GameObject whichSquasIsSelectedPlane;

	public void Exit() {
		Application.Quit();
	}

	public int GetGameMode() {
		return gameMode;
	}



	public void ChangeMovementGameMode() {
		int curGM = gameMode;
		if (gameMode != 0)
			UnSetCurrentGameMode();
		 if (curGM != 1) {
			SetMovmentGameMode();
		}
		
	}

	void UnSetMovmentGameMode() {
		prevGameMode = 1;
		movmentGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
	}
	void SetMovmentGameMode() {
		gameMode = 1;
		movmentGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
	}



	public void ChangeFireGameMode() {
		int curGM = gameMode;
		if (gameMode != 0)
			UnSetCurrentGameMode();
		if (curGM != 2) {
			SetFireGameMode();
		}
		
	}

	void UnSetFireGameMode() {
		prevGameMode = 2;
		fireGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
		obrazenia.SetActive(false);
	}
	void SetFireGameMode() {
		gameMode = 2;
		fireGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
	}



	public void ChangeAttackGameMode() {
		int curGM = gameMode;
		if (gameMode != 0)
			UnSetCurrentGameMode();
		if (curGM != 3) {
			SetAttackGameMode();
		}
		
	}

	void UnSetAttackGameMode() {
		prevGameMode = 3;
		attackGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
	}
	void SetAttackGameMode() {
		gameMode = 3;
		attackGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
	}



	public void ChangeDeployGameMode() {
		int curGM = gameMode;
		if (gameMode != 0)
			UnSetCurrentGameMode();
		if (curGM != -1) {
			SetDeployGameMode();
		}
		
	}

	void UnSetDeployGameMode() {
		prevGameMode = -1;
		deployGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
		movmentGameMode.SetActive(true);
		fireGameMode.SetActive(true);
		attackGameMode.SetActive(true);
		deployGameMode.SetActive(false);
		obrazenia.SetActive(false);

	}
	void SetDeployGameMode() {
		gameMode = -1;
		deployGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
		movmentGameMode.SetActive(false);
		fireGameMode.SetActive(false);
		attackGameMode.SetActive(false);
		deployGameMode.SetActive(true);
		exitGameMode.SetActive(true);
		obrazenia.SetActive(false);

	}



	public void ChangeExitGameMode() {
		int curGM = gameMode;
		if (gameMode != 0)
			UnSetCurrentGameMode();
		if (curGM != -2) {
			SetExitGameMode();
		}
		
	}

	void UnSetExitGameMode() {
		bool wasDeply = false;
		if (prevGameMode == -1)
			wasDeply = true;
		exitGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
		exitGameMode.SetActive(true);
		obrazenia.SetActive(false);
		if (wasDeply) {
			gameMode = -1;
			SetDeployGameMode();
		}
		else {
			movmentGameMode.SetActive(true);
			fireGameMode.SetActive(true);
			attackGameMode.SetActive(true);
		}
	}
	void SetExitGameMode() {
		gameMode = -2;
		exitGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
		exitGameMode.SetActive(false);
		movmentGameMode.SetActive(false);
		fireGameMode.SetActive(false);
		attackGameMode.SetActive(false);
		deployGameMode.SetActive(false);
		obrazenia.SetActive(false);
	}


	void UnSetCurrentGameMode() {
		if (gameMode != 0)
			switch (gameMode) {
				case (-2):
					UnSetExitGameMode();
					if (prevGameMode == -1)
						gameMode = -1;
					else
						gameMode = 0;
					prevGameMode = -2;
					break;
				case (-1):
					UnSetDeployGameMode();
					gameMode = 0;
					break;
				case (1):
					UnSetMovmentGameMode();
					gameMode = 0;
					break;
				case (2):
					UnSetFireGameMode();
					gameMode = 0;
					break;
				case (3):
					UnSetAttackGameMode();
					gameMode = 0;
					break;
			}

	}

	public void Update() {

		try {
			whichSquadIsSelected = terrain.GetComponent<SelectUnit>().whichSquadIsSelect;
		}
		catch (NullReferenceException) {
			whichSquadIsSelected = null;
		}

		if (whichSquadIsSelected != null) {
			whichSquadIsSelectedText.GetComponent<Text>().text = whichSquadIsSelected.transform.name;
			whichSquadIsSelectedText.SetActive(true);
			whichSquasIsSelectedPlane.SetActive(true);
		}
		else {
			whichSquadIsSelectedText.GetComponent<Text>().text = "";
			whichSquadIsSelectedText.SetActive(false);
			whichSquasIsSelectedPlane.SetActive(false);
		}

		if (gameMode >= 0 && Input.anyKey) {
			if (Input.GetKeyDown(KeyCode.R)) {
				ChangeMovementGameMode();
			}
			if (Input.GetKeyDown(KeyCode.S)) {
				ChangeFireGameMode();
			}
			if (Input.GetKeyDown(KeyCode.A)) {
				ChangeAttackGameMode();
			}
		}
		if (gameMode == -1 && Input.anyKey && Input.GetKeyDown(KeyCode.D)) {
			ChangeDeployGameMode();
		}
		if (Input.anyKey && Input.GetKeyDown(KeyCode.Escape)) {
			ChangeExitGameMode();
		}
	}
}
