using UnityEngine;
using System.Collections;

public class ActiveText : MonoBehaviour {
	/* 
	 * Skrypt zmieniajacy widocznosc elementu
	 */
	public void ChangeAvaliability() {
		if (gameObject.active) {
			gameObject.SetActive(false);
		}
		else {
			gameObject.SetActive(true);
		}
	}
}
