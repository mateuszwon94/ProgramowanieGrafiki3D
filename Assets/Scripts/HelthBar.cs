using UnityEngine;
using System.Collections;

public class HelthBar : MonoBehaviour {
	/*
	 * Skrypt odpowiedzialny za pasek zdrowia
	 * Obraca nim oraz odpowiednio zmniejsza
	 */

	public GameObject camera;

	float curRotX;
	float curRotY;
	float curRotZ = 0f;

	float scale = 20f;

	public void ReScale(int howManyLeft, int howManyWasOnStart) {
		//Funkcja przeskalowujaca pasek zdrowia
		float how = ((float)howManyLeft) / ((float)howManyWasOnStart);

		gameObject.transform.localScale = new Vector3(scale * how, 5, 1);

		if (how > 0.5)
			gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5f, 0f, 0.7f);
		else if (how > 0.2)
			gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.7f);
		else
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.7f);

		//curRotZ = -90f;
	}

	public void Awake() {
		camera = GameObject.FindGameObjectWithTag("Camera");
	}

	public void Update() {
		//Obracanie paska zdrowia
		curRotY = camera.GetComponent<CameraControl>().currentRotX;
		curRotX = camera.GetComponent<CameraControl>().currentRotY;

		gameObject.transform.rotation = Quaternion.identity;

		gameObject.transform.rotation = Quaternion.Euler(curRotX, curRotY, curRotZ);
	}
	
}
