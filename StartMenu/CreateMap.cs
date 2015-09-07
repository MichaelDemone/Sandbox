using UnityEngine;
using System.Collections;

public class CreateMap : MonoBehaviour {
	
	public static Hashtable map;//the map
	public static int seed = 1234;
	
	protected static GameObject player, ground;
	protected static float maxY = 0, minY = 0;
	protected static float previousYPosR = 0, previousYPosL = 0;
	protected static float widthOfGroundPiece = 1;
	protected const float MINIMUM_Y = -1000, MINIMUM_GROUND_Y = 0;
	protected static int distanceBetweenLoads = 1;
	protected static int lastXTransition = 0, lastYTransition = 0;
	protected static float widthDestroyBox = 20, heightDestroyBox = 60;
	protected static float distToDestroyBox = 30;
	
	// Use this for initialization
	void Start () {

		ground =  GameObject.Find("Ground");
		player = GameObject.Find ("Player");

		WalkingGeneration.setValues ();

		player.transform.position = new Vector3 (10, 26);

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

				// Set minimum and maximum y value
				if (previousYPosR > maxY) maxY = previousYPosR;
				if (previousYPosR < minY) minY = previousYPosR;
			}

		} else {
			Debug.Log("Loaded map");
			maxY = 100;
		}

		// Creating the objects for around you when you spawn
		int loadingRight = Mathf.RoundToInt(player.transform.position.x + distToDestroyBox + widthDestroyBox);
		int loadingLeft = Mathf.RoundToInt(player.transform.position.x - distanceBetweenLoads - widthDestroyBox);
		                             
		int bottomLoad = Mathf.RoundToInt(player.transform.position.y - distToDestroyBox - widthDestroyBox);
		int topLoad = Mathf.RoundToInt(player.transform.position.y + distToDestroyBox + widthDestroyBox);

		for (float i = loadingLeft; i < loadingRight; i += widthOfGroundPiece){
			for (float j = bottomLoad; j < topLoad; j += widthOfGroundPiece){
				WalkingGeneration.loadPeice(i,j,0);
			}
		}
	}

	//adds or subtracts 1 or 2 blocks to the height
	protected int selectDirection() {
		return (int) Random.Range (-1.95f, 1.95f);
	}

	// Update is called once per frame
	void Update () {
		WalkingGeneration.checkIfNeedsLoading ();
	}
}
