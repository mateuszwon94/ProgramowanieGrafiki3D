using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUIInput : MonoBehaviour {
	/*
	 * Skrypt odpowiedzialny za interfejs
	 */

	int howManyGameModes = 4;

	//przeciagniete w inspektorze odpowiednie przyciski i teksty
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

	public GameObject endTureGameMode;
	public GameObject endTureGameModeText;

	public GameObject obrazenia;
	public GameObject[] obrazeniaTexts = new GameObject[5];

	public GameObject terrain;

	int gameMode = -1;
	int prevGameMode = -1;

	GameObject whichSquadIsSelected;
	public GameObject whichSquadIsSelectedText;
	public GameObject whichSquasIsSelectedPlane;

	public void Exit() {
		//Zamykanie aplikacji
		Application.Quit();
	}

	public int GetGameMode() {
		//Zwraca obecyn tryb gry
		return gameMode;
	}



	public void ChangeMovementGameMode() {
		//zmienia tryb gry z/na faze ruchu
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
		//zmienia tryb gry z/na faze strzelania
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
		//zmienia tryb gry z/na faze walki wrecz
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
		//zmienia tryb gry z/na faze rozmieszczania
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
		endTureGameMode.SetActive(true);
		deployGameMode.SetActive(false);
		obrazenia.SetActive(false);
	}
	void SetDeployGameMode() {
		gameMode = -1;
		deployGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
		movmentGameMode.SetActive(false);
		fireGameMode.SetActive(false);
		attackGameMode.SetActive(false);
		endTureGameMode.SetActive(false);
		deployGameMode.SetActive(true);
		exitGameMode.SetActive(true);
		obrazenia.SetActive(false);
	}



	public void ChangeExitGameMode() {
		//zmienia tryb gry z/na faze wyjscia z aplikacji
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
			endTureGameMode.SetActive(true);
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
		endTureGameMode.SetActive(false);
	}


	public void ChangeEndTureGameMode() {
		//zmienia tryb gry z/na faze ruchu
		int curGM = gameMode;
		if (gameMode != 0)
			UnSetCurrentGameMode();
		if (curGM != 1) {
			SetEndTureGameMode();
		}

	}

	void UnSetEndTureGameMode() {
		prevGameMode = -10;
		endTureGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
	}
	void SetEndTureGameMode() {
		gameMode = -10;
		endTureGameModeText.GetComponent<ActiveText>().ChangeAvaliability();
	}


	void UnSetCurrentGameMode() {
		//ustawia tryb gry w wiekszosci przypadkow na zaden
		if (gameMode != 0)
			switch (gameMode) {
				case (-10):
					UnSetEndTureGameMode();
					gameMode = 0;
					break;
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
		/*
		 * Obsluga trybow gry
		 * Zarowno mysza jak i klawiatura
		 */
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
			if (Input.GetKeyDown(KeyCode.Return)) {
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
