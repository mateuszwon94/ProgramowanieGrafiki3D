using UnityEngine;
using System.Collections;

public class HelthBar : MonoBehaviour {

	public GameObject camera;

	float curRotX;
	float curRotY;
	float curRotZ = 0f;

	float scale = 5f;

	public void ReScale(int howManyLeft, int howManyWasOnStart) {
		float how = ((float)howManyLeft) / ((float)howManyWasOnStart);

		gameObject.transform.localScale = new Vector3(1, scale * how, 1);

		//Debug.Log(how);

		if (how > 0.5)
			gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5f, 0f, 0.7f);
		else if (how > 0.2)
			gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.7f);
		else
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.7f);

		curRotZ = -90f;
	}

	public void Awake() {
		camera = GameObject.FindGameObjectWithTag("Camera");
	}

	public void Update() {
		curRotY = camera.GetComponent<CameraControl>().currentRotX;
		curRotX = camera.GetComponent<CameraControl>().currentRotY;

		gameObject.transform.rotation = Quaternion.identity;

		gameObject.transform.rotation = Quaternion.Euler(curRotX, curRotY, curRotZ);
	}
	
}
