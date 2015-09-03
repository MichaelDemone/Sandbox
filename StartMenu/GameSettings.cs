using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

	private GameObject menuButton, settings;
	// Use this for initialization
	void Start () {
		menuButton = GameObject.Find ("Menu Button");
		settings = GameObject.Find ("Settings");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void pauseGame() {
		settings.transform.localScale = new Vector3 (1, 1, 0);
		menuButton.transform.localScale = new Vector3 (0, 0, 0);
		Time.timeScale = 0;
	}

	void resumeGame() {
		Time.timeScale = 1;
		settings.transform.localScale = new Vector3 (0, 0, 0);
		menuButton.transform.localScale = new Vector3(1,1,0);
	}

	void exitGame() {
		Application.Quit ();
	}

	void openMoreSettings() {
		
	}
}
