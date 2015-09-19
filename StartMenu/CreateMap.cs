using UnityEngine;
using System.Collections;

public class CreateMap : MonoBehaviour {
	
	public static Hashtable map;//the map
	public static int seed = 1234;
	
	protected static GameObject player, ground;
	protected static float maxY = 0, minY = 0;
	protected static float minX = -1000, maxX = 1000;
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

		if (map == null) {
			
			// Put the correct data in the map

			map = new Hashtable ();
			Random.seed = seed;
			
			Vector3 position = new Vector3();

			// This is the starting position
			previousYPosR = 10;
			previousYPosL = 10;
			// Create map from -1000 to 1000

			for (float i = minX; i < maxX; i += widthOfGroundPiece) {

				// Pick whether the block goes up or down
				previousYPosR += selectDirection();
				
				for (float j = -20; j < previousYPosR; j += widthOfGroundPiece) {
					position = new Vector3 (i, j, 0);
					map.Add (position, "Rock");
					
				}

				Vector3 position1 = position;
				Vector3 position2 = position;
				Vector3 position3 = position;
				Vector3 position4 = position;

				position1.y = position.y - 1;
				position2.y = position.y - 2;
				position3.y = position.y - 3;
				position4.y = position.y - 4;

				// Put grassy block on top
				map.Remove (position);
				map.Remove (position1);
				map.Remove (position2);
				map.Remove (position3);
				map.Remove (position4);
				map.Add (position1, "Dirt");
				map.Add (position2, "Dirt");
				map.Add (position3, "Dirt");
				map.Add (position4, "Dirt");
				map.Add (position, "DirtWGrass");

				// Set minimum and maximum y value
				if (previousYPosR > maxY) maxY = previousYPosR;
				if (previousYPosR < minY) minY = previousYPosR;
			}

		} else {
			Debug.Log("Loaded map");
			maxY = 100;
		}

		// Generate Caves
		for (int i = 0; i < 10; i++) {
			GenerateCaves.generateCave();
		}

		GenerateTrees.treeLocation((int)maxY, 1.95f);

		player.transform.position = new Vector3 (10, 26);

		// Creating the objects for around you when you spawn
		int loadingRight = Mathf.RoundToInt(player.transform.position.x + distToDestroyBox);
		int loadingLeft = Mathf.RoundToInt(player.transform.position.x - distanceBetweenLoads);
		                             
		int bottomLoad = Mathf.RoundToInt(player.transform.position.y - distToDestroyBox);
		int topLoad = Mathf.RoundToInt(player.transform.position.y + distToDestroyBox);

		for (float i = loadingLeft; i < loadingRight; i += widthOfGroundPiece){
			for (float j = bottomLoad; j < topLoad; j += widthOfGroundPiece){
				WalkingGeneration.loadPeice(i,j,0);
				WalkingGeneration.loadPeice(i,j,-1);
				WalkingGeneration.loadPeice(i,j,1);
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
