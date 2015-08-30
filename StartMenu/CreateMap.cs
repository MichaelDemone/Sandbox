using UnityEngine;
using System.Collections;

public class CreateMap : MonoBehaviour {

	public GameObject dirt, copper;
	public int seed = 1234;
	bool thing = false;

	private GameObject player;


	// Use this for initialization
	void Start () {

		Random.seed = seed;

		// This is the starting position
		Vector3 position = new Vector3 (-60, -20, 0);
		float widthOfGroundPiece = 1;
		float previousYPos = 0;

		// Continue creating peices until the x position reaches 20
		for (; position.x < 40; position.Set(position.x + widthOfGroundPiece, -20, 0)) {

			float columnHeight = selectHeight();
			for  (; position.y < previousYPos + columnHeight; position.Set (position.x, position.y + widthOfGroundPiece,0)) {
				GameObject obj;
				if(thing)
					obj = (GameObject) GameObject.Instantiate(dirt, position, Quaternion.identity);
				else
					obj = (GameObject) GameObject.Instantiate(copper, position, Quaternion.identity);
				obj.transform.SetParent(this.transform);
				thing = !thing;
			}
			previousYPos += columnHeight;
		}
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.RoundToInt (player.transform.position.x) % 100 == 0) {
			StartCoroutine("loadPeices");
			StartCoroutine("unloadPeices");
		}
	}

	//adds or subtracts 1 or 2 blocks to the height
	int selectHeight(){

		int dir = (int) (Random.value * 100);

		if (dir % 2 == 0){

			int amnt = Random.Range(0,2);
			return amnt;

		} else {

			int amnt = Random.Range(0,2);
			return (-1 * amnt);

		}
	}

	public IEnumerator loadPeices() {
		int playerXPosition = Mathf.RoundToInt(player.transform.position.x);
		for (int i = playerXPosition + 100; i < playerXPosition + 200; i++) {
			for(int j = -20; j < 0; j++) {
				GameObject.Instantiate(dirt, new Vector3(i, j, 0), Quaternion.identity);
				yield return null;
			}
		}
	}

	public IEnumerator unloadPeices() {
		int playerXPosition = Mathf.RoundToInt(player.transform.position.x);
		for (int i = playerXPosition - 100; i > playerXPosition - 200; i--) {
			for(int j = -20; j < 0; j++) {
				Collider2D box = Physics2D.OverlapPoint(new Vector2(i, j));
				GameObject.Destroy(box.gameObject);
				yield return null;
			}
		}
	}

}
