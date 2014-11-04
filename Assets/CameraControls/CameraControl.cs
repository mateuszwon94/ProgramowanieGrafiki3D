using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	float currentRotX = 0.0f;
	float currentRotY = 40.0f;

	float currentPosX;
	float currentPosY;

	float currentDistance;

	public float minVertical = 10f;
	public float maxVertical = 80f;

	public float movSensitivityVert = 5f;
	public float movSensitivityHoriz = 5f;

	public float rotSensitivityVert = 2f;
	public float rotSensitivityHoriz = 2f;

	public float minDistance = 5f;
	public float maxDistance = 50f;

	public float sensitivityZoom = 1f;

	Camera camera_;

	void Start() {
		camera_ = gameObject.transform.GetComponentInChildren<Camera>();

		gameObject.transform.rotation = Quaternion.Euler(currentRotY, currentRotX, 0.0f);

		currentPosX = gameObject.transform.position.x;
		currentPosY = gameObject.transform.position.z;

		currentDistance = -camera_.transform.position.z;


		Vector3 temp = new Vector3(currentPosX, 0.0f, currentPosY);
		gameObject.transform.position = Quaternion.AngleAxis(gameObject.transform.rotation.eulerAngles.y, Vector3.up) * temp;
	}

	void Update() {

		if (Input.anyKey) 
		{ //Moving by mouse movment
			if (Input.GetButton("Mouse 3")) {
				if (Input.GetButton("Camera Mod")) {
					currentPosX = Input.GetAxis("Mouse X") * movSensitivityHoriz * -0.01f * currentDistance;
					currentPosY = Input.GetAxis("Mouse Y") * movSensitivityVert * -0.01f * currentDistance;
					Vector3 temp = new Vector3(currentPosX, 0, currentPosY);

					gameObject.transform.position += Quaternion.AngleAxis(gameObject.transform.rotation.eulerAngles.y, Vector3.up) * temp;


					Ray ray = new Ray(transform.position + 100 * Vector3.up, Vector3.down); //chwyta również budynki i propsy, do modyfikacji
					RaycastHit hitInfo;

					if (Physics.Raycast(ray, out hitInfo, 1000f)) {
						temp = transform.position;
						temp.Set(temp.x, hitInfo.point.y, temp.z);
						transform.position = temp;
					}
				}
				else {
					currentRotX += Input.GetAxis("Mouse X") * rotSensitivityHoriz;
					currentRotY += Input.GetAxis("Mouse Y") * rotSensitivityVert * -1f;

					currentRotY = Mathf.Clamp(currentRotY, minVertical, maxVertical);

					gameObject.transform.rotation = Quaternion.Euler(currentRotY, currentRotX, 0.0f);
				}
			}
		}
		else { 
			//Moving near screen edges
			if ((Input.mousePosition.y >= 0) && (Input.mousePosition.y <= 10)) {
				currentPosY = movSensitivityVert * currentDistance * -0.01f;
				Vector3 temp = new Vector3(0, 0, currentPosY);

				gameObject.transform.position += Quaternion.AngleAxis(gameObject.transform.rotation.eulerAngles.y, Vector3.up) * temp;

			}
			else if ((Input.mousePosition.y >= Screen.height - 10) && (Input.mousePosition.y <= Screen.height)) {
				currentPosY = movSensitivityVert * currentDistance * 0.01f;
				Vector3 temp = new Vector3(0, 0, currentPosY);

				gameObject.transform.position += Quaternion.AngleAxis(gameObject.transform.rotation.eulerAngles.y, Vector3.up) * temp;

			}
			if ((Input.mousePosition.x >= 0) && (Input.mousePosition.x <= 10)) {
				currentPosX = movSensitivityHoriz * currentDistance * -0.01f;
				Vector3 temp = new Vector3(currentPosX, 0, 0);

				gameObject.transform.position += Quaternion.AngleAxis(gameObject.transform.rotation.eulerAngles.y, Vector3.up) * temp;

			}
			else if ((Input.mousePosition.x >= Screen.width - 10) && (Input.mousePosition.x <= Screen.width)) {
				currentPosX = movSensitivityHoriz * currentDistance * 0.01f;
				Vector3 temp = new Vector3(currentPosX, 0, 0);
				gameObject.transform.position += Quaternion.AngleAxis(gameObject.transform.rotation.eulerAngles.y, Vector3.up) * temp;
			}
			Ray ray = new Ray(transform.position + 100 * Vector3.up, Vector3.down); //chwyta również budynki i propsy, do modyfikacji
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo, 1000f)) {
				Vector3 temp = transform.position;
				temp.Set(temp.x, hitInfo.point.y, temp.z);
				transform.position = temp;
			}
		}

		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			currentDistance += Input.GetAxis("Mouse ScrollWheel") * -sensitivityZoom;

			currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

			camera_.transform.localPosition = new Vector3(0f, 0f, -currentDistance);
		}
	}
}

