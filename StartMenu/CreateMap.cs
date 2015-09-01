using UnityEngine;
using System.Collections;

public class CreateMap : MonoBehaviour {

	public GameObject dirt, copper;
	public int seed = 1234;
	bool thing = false;
	private int maxY = 0;
	private float previousYPosR, previousYPosL;
	private GameObject player, Ground;
	public Hashtable map;//the map
	private float widthOfGroundPiece = 1;

	// Use this for initialization
	void Start () {

		Ground =  GameObject.Find("Ground");
		player = GameObject.Find ("Player2");

		map = new Hashtable();
		Random.seed = seed;

		// This is the starting position
		Vector3 position = new Vector3 (-1000, -20, 0);

		previousYPosR = 0;

		// Continue creating peices until the x position reaches 20
		for (float i = -1000; i < 1000; i += widthOfGroundPiece) {
			
			int columnHeight = selectDirection();
			if (i == -1000) previousYPosL = columnHeight;
			
			for  (float j = -20; j < previousYPosR + columnHeight; j += widthOfGroundPiece) {
				position = new Vector3(i,j,0);
				//if(j % 5 == 0){
					map.Add(position,"Dirt");
				//}else{
				//	map.Add(position,"Copper");
				//}

				thing = !thing;
				
				if (columnHeight + previousYPosR > maxY) maxY += columnHeight;
				
			}
			 map.Remove(position);
			 map.Add(position,"DirtWGrass");
			previousYPosR += columnHeight;
		}
	

		for (float i = -100; i < 100; i += widthOfGroundPiece){
			for (float j = -20; j < maxY; j += widthOfGroundPiece){
				string tile = (string) map[new Vector3(i,j,0)];
				if(tile != null) {
					GameObject obj = (GameObject) GameObject.Instantiate(Dictionary.get (tile), new Vector3(i,j,0), Quaternion.identity);
					obj.layer = LayerMask.NameToLayer("Ground");
					obj.transform.SetParent(Ground.transform);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.RoundToInt (player.transform.position.x) % 100 == 0) {
			StartCoroutine("loadPeices");
			StartCoroutine("unloadPeices");
		}
	}

	//adds or subtracts 1 or 2 blocks to the height
	int selectDirection(){

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
		bool goingRight = player.GetComponent<PlayerControl>().goingRight;

		Vector3 position;

		if(goingRight) {

			position = new Vector3 (playerXPosition + 100,0,0);

			for(float i = -40; i < 20; i += widthOfGroundPiece) {
				Collider2D box = Physics2D.OverlapPoint(new Vector2(position.x, i));
				if(box != null) {
					yield break;
				}
			}

			for (float i = playerXPosition + 100; i < playerXPosition + 200; i += widthOfGroundPiece) {
				position.x = i;
				previousYPosR += selectDirection ();

				for  (float j = -20; j < maxY; j += widthOfGroundPiece) {
					position.y = j;

					string tile = (string) map[new Vector3(i,j,0)];
					if(tile != null) {
						GameObject obj = (GameObject) GameObject.Instantiate(Dictionary.get (tile), position, Quaternion.identity);
						obj.transform.SetParent(Ground.transform);
					}
				}
			}
		} else {
			
			position = new Vector3 (playerXPosition - 100,0,0);

			for(float i = -40; i < 20; i += widthOfGroundPiece) {
				Collider2D box = Physics2D.OverlapPoint(new Vector2(position.x, i));
				if(box != null) {
					yield break;
				}
			}

			for (float i = playerXPosition - 100; i > playerXPosition - 200; i -= widthOfGroundPiece) {
				position.x = i;

				previousYPosL += selectDirection ();
				
				for  (float j = -20; j < maxY; j += widthOfGroundPiece) {
					position.y = j;

					string tile = (string) map[position];
					if(tile != null) {
						GameObject obj = (GameObject) GameObject.Instantiate(Dictionary.get (tile), position, Quaternion.identity);
						obj.transform.SetParent(Ground.transform);
					}
				}
				
			}
		}
		yield return null;
	}



	public IEnumerator unloadPeices() {

		int playerXPosition = Mathf.RoundToInt(player.transform.position.x);
		bool goingRight = player.GetComponent<Rigidbody2D>().velocity.x > 0;

		if(goingRight) {
			for (float i = playerXPosition - 100; i > playerXPosition - 200; i -= widthOfGroundPiece) {
				for(float j = -40; j < 300; j += widthOfGroundPiece) {
					Vector2 pos = new Vector3(i,j,0);
					Collider2D box = Physics2D.OverlapPoint(pos);

					if(box != null) {
						if(map.Contains(pos)) {
							map.Remove(pos);
						}
						Item script;
						if((script = box.gameObject.GetComponent<Item>()) != null)
							map.Add(pos, script.name);
						else
							Debug.Log("null stuff");
						GameObject.Destroy(box.gameObject);
					} else {
						if(map.Contains(pos)) {
							map.Remove(pos);
						}
					}
				}

			}
		} else {
			for (float i = playerXPosition + 100; i < playerXPosition + 200; i += widthOfGroundPiece) {
				for(float j = -40; j < 300; j += widthOfGroundPiece) {
					Vector2 pos = new Vector3(i,j,0);
					Collider2D box = Physics2D.OverlapPoint(new Vector2(i, j));
					if(box != null) {
						if(map.Contains(pos)) {
							map.Remove(pos);
						}
						Item script;
						if((script = box.gameObject.GetComponent<Item>()) != null)
							map.Add(pos, script.name);
						else
							Debug.Log("null stuff");
						GameObject.Destroy(box.gameObject);
					} else {
						if(map.Contains(pos)) {
							map.Remove(pos);
						}
					}

				}

			}
		}
		yield return null;
	}
}
