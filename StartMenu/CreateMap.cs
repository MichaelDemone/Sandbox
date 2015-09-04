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
			previousYPosR = 10;
			previousYPosL = 10;


			// Create map from -1000 to 1000
			for (float i = -1000; i < 1000; i += widthOfGroundPiece) {

				// Pick whether the block goes up or down
				previousYPosR += selectDirection();

				for (float j = -20; j < previousYPosR; j += widthOfGroundPiece) {

					position = new Vector3 (i, j, 0);
					map.Add (position, "Dirt");
				}

				// Put grassy block on top
				map.Remove (position);
				map.Add (position, "DirtWGrass");

				if (previousYPosR > maxY) maxY = previousYPosR;
			}


			//GenerateTrees.placeTrees(maxY);

		} else {
			Debug.Log("Loading map");
			maxY = 100;
		}

		GenerateTrees.placeTrees((int) maxY, 1.95f);

		for (float i = -2*distanceBetweenLoads + widthOfGroundPiece; i < distanceBetweenLoads; i += widthOfGroundPiece){
			for (float j = minY; j < maxY; j += widthOfGroundPiece){
				WalkingGeneration.loadPeice(i,j,0);

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
			else if (xPos == lastTransition) {
				return;
			} else {
				WalkingGeneration.unloadPeices(xPos);
				WalkingGeneration.loadPeices(xPos);

			}
			rightLastTransition = goingRight;
			lastTransition = xPos;


		}
	}

	//adds or subtracts 1 or 2 blocks to the height
	protected int selectDirection() {
		return (int) Random.Range (-1.95f, 1.95f);
	}
}
