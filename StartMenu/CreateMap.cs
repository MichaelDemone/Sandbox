using UnityEngine;
using System.Collections;

public class CreateMap : MonoBehaviour {
	
	public static Hashtable map;//the map
	public int seed = 1234;
	
	protected static GameObject player, ground;
	protected static float maxY = 0, minY = 0;
	protected static float previousYPosR = 0, previousYPosL = 0;
	protected static float widthOfGroundPiece = 1;
	protected const float MINIMUM_Y = -1000, MINIMUM_GROUND_Y = 0;
	protected static int distanceBetweenLoads = 1;
	private int lastXTransition = 0, lastYTransition = 0;
	private bool rightLastTransition = true, upLastTransition = false;
	private float widthDestroyBox = 20, heightDestroyBox = 60;
	private float distToDestroyBox = 30;
	
	// Use this for initialization
	void Start () {

		ground =  GameObject.Find("Ground");
		player = GameObject.Find ("Player");
		lastXTransition = Mathf.RoundToInt(player.transform.position.x);
		lastYTransition = Mathf.RoundToInt(player.transform.position.y);

		WalkingGeneration.cameraSizeX = 2*Camera.main.orthographicSize + 5;
		WalkingGeneration.cameraSizeY = Camera.main.orthographicSize + 10;

		Debug.Log (WalkingGeneration.cameraSizeX);

		distToDestroyBox = WalkingGeneration.cameraSizeX + 2;
		heightDestroyBox = distToDestroyBox * 2;
		widthDestroyBox = heightDestroyBox / 3f;


		player.transform.position = new Vector3 (0, player.transform.position.y);
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

		} else {
			Debug.Log("Loading map");
			maxY = 100;
		}

		player.transform.position = new Vector3 (10, 26);

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
	
	// Update is called once per frame
	void Update () {
		int xPos = Mathf.RoundToInt (player.transform.position.x);
		int yPos = Mathf.RoundToInt (player.transform.position.y);
		bool goingRight = player.GetComponent<Rigidbody2D>().velocity.x > 0;
		bool goingUp = player.GetComponent<Rigidbody2D>().velocity.y > 0;

		// Check if player should load in the X direction
		if (xPos > lastXTransition + distanceBetweenLoads || xPos < lastXTransition - distanceBetweenLoads) {
			
			for(int i = lastXTransition; i < xPos; i += distanceBetweenLoads) {
				WalkingGeneration.loadPeices(i, yPos, goingRight, goingUp, true, false);
				rightLastTransition = goingRight;
				lastXTransition = i;
			}

			for(int i = lastXTransition; i > xPos; i -= distanceBetweenLoads) {
				WalkingGeneration.loadPeices(i, yPos, goingRight, goingUp, true, false);
				rightLastTransition = goingRight;
				lastXTransition = i;
			}
			
			if (xPos != lastXTransition || rightLastTransition != goingRight) {
				WalkingGeneration.loadPeices(xPos, yPos, goingRight, goingUp, true, false);
				rightLastTransition = goingRight;
				lastXTransition = xPos;
			}
		}
		
		// Check if player should load in the X direction
		else if (yPos > lastYTransition + distanceBetweenLoads || yPos < lastYTransition - distanceBetweenLoads) {

			
			for(int i = lastYTransition; i < yPos; i += distanceBetweenLoads) {
				WalkingGeneration.loadPeices(xPos, i, goingRight, goingUp, false, true);
				upLastTransition = goingUp;
				lastYTransition = i;
			}
			for(int i = lastYTransition; i > yPos; i -= distanceBetweenLoads) {
				WalkingGeneration.loadPeices(xPos, i, goingRight, goingUp, false, true);
				upLastTransition = goingUp;
				lastYTransition = i;
			}
			
			if (yPos != lastYTransition || goingUp != upLastTransition) {
				WalkingGeneration.loadPeices(xPos, yPos, goingRight, goingUp, false, true);
				upLastTransition = goingUp;
				lastYTransition = yPos;
			}
		}


		Collider2D[] collidersY;
		Collider2D[] collidersX;

		/*
		Debug.DrawLine (new Vector2 (xPos + heightDestroyBox / 2, yPos + distToDestroyBox + widthDestroyBox), new Vector2(xPos - heightDestroyBox/2, yPos + distToDestroyBox));
		Debug.DrawLine (new Vector2(xPos + heightDestroyBox/2, yPos - distToDestroyBox - widthDestroyBox), new Vector2(xPos - heightDestroyBox/2, yPos - distToDestroyBox));
		Debug.DrawLine (new Vector2(xPos + distToDestroyBox, yPos - heightDestroyBox/2), new Vector2(xPos + distToDestroyBox + widthDestroyBox, yPos + heightDestroyBox/2));
		Debug.DrawLine (new Vector2(xPos - distToDestroyBox, yPos - heightDestroyBox/2), new Vector2(xPos - distToDestroyBox - widthDestroyBox, yPos + heightDestroyBox/2));
		*/

		if (!goingUp) {
			collidersY = Physics2D.OverlapAreaAll (new Vector2 (xPos + heightDestroyBox / 2, yPos + distToDestroyBox + widthDestroyBox), 
			                                      new Vector2 (xPos - heightDestroyBox / 2, yPos + distToDestroyBox));
		} else {
			collidersY = Physics2D.OverlapAreaAll (new Vector2 (xPos + heightDestroyBox / 2, yPos - distToDestroyBox - widthDestroyBox), 
			                                      new Vector2 (xPos - heightDestroyBox / 2, yPos - distToDestroyBox));
		}
		if (!goingRight) {
			collidersX = Physics2D.OverlapAreaAll (new Vector2 (xPos + distToDestroyBox, yPos - heightDestroyBox / 2), 
			                                      new Vector2 (xPos + distToDestroyBox + widthDestroyBox, yPos + heightDestroyBox / 2));
		} else {
			collidersX = Physics2D.OverlapAreaAll (new Vector2 (xPos - distToDestroyBox, yPos - heightDestroyBox / 2), 
			                                      new Vector2 (xPos - distToDestroyBox - widthDestroyBox, yPos + heightDestroyBox / 2));
		}

		foreach (Collider2D col in collidersX) {
			if(col.CompareTag("Tile"))
				Destroy(col.gameObject);
		}
		foreach (Collider2D col in collidersY) {
			if(col.CompareTag("Tile"))
				Destroy(col.gameObject);
		}
	}
	
	//adds or subtracts 1 or 2 blocks to the height
	protected int selectDirection() {
		return (int) Random.Range (-1.95f, 1.95f);
	}
}
