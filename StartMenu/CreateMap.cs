using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenerateTrees))]


public class CreateMap : MonoBehaviour {

	public static Hashtable map;//the map
	public int seed = 1234;

	protected static GameObject player, ground;
	protected static float maxY = 0, minY = 0;
	protected static float previousYPosR = 0, previousYPosL = 0;
	protected static float widthOfGroundPiece = 1;
	protected const float MINIMUM_Y = -1000, MINIMUM_GROUND_Y = 0;
	protected static int distanceBetweenLoads = 5;
	private int lastTransition = 0;
	private bool rightLastTransition = false;

	// Use this for initialization
	void Start () {

		ground =  GameObject.Find("Ground");
		player = GameObject.Find ("Player");
		player.transform.position = new Vector3 (-distanceBetweenLoads / 2, player.transform.position.y);
		if (map == null) {

			// Put the correct data in the map

			map = new Hashtable ();
			Random.seed = seed;

			Vector3 position = new Vector3();
			// This is the starting position
<<<<<<< HEAD
			previousYPosR = 10;
			previousYPosL = 10;
=======
			Vector3 position = new Vector3 (-1000, -40, 0);
>>>>>>> origin/master

			// Create map from -1000 to 1000
			for (float i = -1000; i < 1000; i += widthOfGroundPiece) {
<<<<<<< HEAD

				// Pick whether the block goes up or down
				previousYPosR += selectDirection();

				for (float j = -20; j < previousYPosR; j += widthOfGroundPiece) {
=======
			
				int columnHeight = selectDirection ();
				Debug.Log(columnHeight);
				if (i == -1000)
					previousYPosL = columnHeight;
			
				for (float j = -40; j < previousYPosR + columnHeight; j += widthOfGroundPiece) {
>>>>>>> origin/master
					position = new Vector3 (i, j, 0);
					map.Add (position, "Dirt");
				}

				// Put grassy block on top
				map.Remove (position);
				map.Add (position, "DirtWGrass");

				if (previousYPosR > maxY) maxY = previousYPosR;
			}

<<<<<<< HEAD
=======
			//GenerateTrees.placeTrees(maxY);

>>>>>>> origin/master
		} else {
			Debug.Log("Loading map");
			maxY = 100;
		}

<<<<<<< HEAD
		for (float i = -2*distanceBetweenLoads + widthOfGroundPiece; i < distanceBetweenLoads; i += widthOfGroundPiece){
			for (float j = minY; j < maxY; j += widthOfGroundPiece){
				WalkingGeneration.loadPeice(i,j,0);
=======
		for (float i = -199; i < 100; i += widthOfGroundPiece){
			for (float j = -40; j < maxY; j += widthOfGroundPiece){
				string tile = (string) map[new Vector3(i,j,0)];
				if(tile != null) {
					GameObject obj = (GameObject) GameObject.Instantiate(Dictionary.get (tile), new Vector3(i,j,0), Quaternion.identity);
					obj.layer = LayerMask.NameToLayer("Ground");
					obj.transform.SetParent(Ground.transform);
				}
>>>>>>> origin/master
			}
		}
	}

	// Update is called once per frame
	void Update () {
		int xPos = Mathf.RoundToInt (player.transform.position.x);
		if (xPos % distanceBetweenLoads == 0) {
			bool goingRight = player.GetComponent<Rigidbody2D>().velocity.x > 0;

			if(xPos == lastTransition && goingRight != rightLastTransition) {
				WalkingGeneration.unloadPeices(xPos);
				WalkingGeneration.loadPeices(xPos);
			}
<<<<<<< HEAD
			else if (xPos == lastTransition) {
				return;
			} else {
				WalkingGeneration.unloadPeices(xPos);
				WalkingGeneration.loadPeices(xPos);
=======

			for (float i = playerXPosition + 100; i < playerXPosition + 200; i += widthOfGroundPiece) {
				position.x = i;
				previousYPosR += selectDirection ();

				for  (float j = -40; j < maxY; j += widthOfGroundPiece) {
					position.y = j;

					string tile = (string) map[new Vector3(i,j,0)];
					if(tile != null) {
						GameObject obj = (GameObject) GameObject.Instantiate(Dictionary.get (tile), position, Quaternion.identity);
						obj.transform.SetParent(Ground.transform);
					}
				}
>>>>>>> origin/master
			}
			rightLastTransition = goingRight;
			lastTransition = xPos;

<<<<<<< HEAD
=======
			for(float i = -40; i < 20; i += widthOfGroundPiece) {
				Collider2D box = Physics2D.OverlapPoint(new Vector2(position.x, i));
				if(box != null) {
					yield break;
				}
			}

			for (float i = playerXPosition - 100; i > playerXPosition - 200; i -= widthOfGroundPiece) {
				position.x = i;

				previousYPosL += selectDirection ();
				
				for  (float j = -40; j < maxY; j += widthOfGroundPiece) {
					position.y = j;

					string tile = (string) map[position];
					if(tile != null) {
						GameObject obj = (GameObject) GameObject.Instantiate(Dictionary.get (tile), position, Quaternion.identity);
						obj.transform.SetParent(Ground.transform);
					}
				}
				
			}
>>>>>>> origin/master
		}
	}

	//adds or subtracts 1 or 2 blocks to the height
	protected int selectDirection() {
		return (int) Random.Range (-1.95f, 1.95f);
	}
}
