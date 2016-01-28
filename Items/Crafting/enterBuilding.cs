using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class enterBuilding : MonoBehaviour {

    bool playerCanEnter = false;
    public GameObject buildingBuilding;
    public GameObject background;

	// Use this for initialization
	void Start () {
        buildingBuilding.transform.localScale = new Vector2(0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        
	    if(Input.GetKeyDown(KeyCode.Space) && playerCanEnter) {
            openBuilding();
        }
	}

    void openBuilding() {
        // Pause game and open menu
        buildingBuilding.transform.localScale = new Vector2(1, 1);
        Time.timeScale = 0;

        // Resize building to fit screen
        Vector2 r = GameObject.Find("UI").GetComponent<RectTransform>().rect.size;
        background.GetComponent<RectTransform>().sizeDelta = r;
        buildingBuilding.GetComponent<RectTransform>().sizeDelta = r;

    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            playerCanEnter = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerCanEnter = false;
        }
    }
}
