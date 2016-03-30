using UnityEngine;
using System.Collections;

public class WalkingGeneration : CreateMap {
	
	private static float yMin, yMax, xMin, xMax;

	public static float cameraSizeX = 100, cameraSizeY = 80;
	public static int bufferSize = 2;
	

	// Should only be called once
	public static void setValues() {

		lastXTransition = Mathf.RoundToInt(player.transform.position.x);
		lastYTransition = Mathf.RoundToInt(player.transform.position.y);
		
		cameraSizeX = (Camera.main.orthographicSize*Camera.main.aspect);
		cameraSizeY = Camera.main.orthographicSize;
		
		distToDestroyBox = cameraSizeX + 2;
		heightDestroyBox = distToDestroyBox * 2;
		widthDestroyBox = heightDestroyBox / 3f;
	}

	// Should be called in an update
	public static void checkIfNeedsLoading() {

		int xPos = Mathf.RoundToInt (player.transform.position.x);
		int yPos = Mathf.RoundToInt (player.transform.position.y);
		bool goingRight = player.GetComponent<Rigidbody2D>().velocity.x > 0;
		bool goingUp = player.GetComponent<Rigidbody2D>().velocity.y > 0;
		
		// Check if player should load in the X direction
		if (xPos > lastXTransition + distanceBetweenLoads || xPos < lastXTransition - distanceBetweenLoads) {
			
			// Load peices up to your position on the right
			for(int i = lastXTransition; i < xPos; i += distanceBetweenLoads) {
				WalkingGeneration.loadPeices(i, yPos, goingRight, goingUp, true, false);
				lastXTransition = i;
			}
			
			// Load peices up to your position on the left
			for(int i = lastXTransition; i > xPos; i -= distanceBetweenLoads) {
				WalkingGeneration.loadPeices(i, yPos, goingRight, goingUp, true, false);
				lastXTransition = i;
			}
		}
		
		// Check if player should load in the Y direction
		if (yPos > lastYTransition + distanceBetweenLoads || yPos < lastYTransition - distanceBetweenLoads) {
			
			// Load peices above you 
			for(int i = lastYTransition; i < yPos; i += distanceBetweenLoads) {
				WalkingGeneration.loadPeices(xPos, i, goingRight, goingUp, false, true);
				lastYTransition = i;
			}
			
			// Load peices below you
			for(int i = lastYTransition; i > yPos; i -= distanceBetweenLoads) {
				WalkingGeneration.loadPeices(xPos, i, goingRight, goingUp, false, true);
				lastYTransition = i;
			}
		}
		
		
		Collider2D[] collidersY;
		Collider2D[] collidersX;
		
		/* DEBUGGING
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
				Dictionary.remove(col.gameObject);
		}
		foreach (Collider2D col in collidersY) {
			if(col.CompareTag("Tile"))
				Dictionary.remove(col.gameObject);
		}
	}


	// Loading
	public static void loadPeices(int xPos, int yPos, bool goingRight, bool goingUp, bool x, bool y) {
		int yMod;
		int xMod;
		
		if (goingUp) {
			yMod = 1;
		} else {
			yMod = -1;
		}
		
		if (goingRight) {
			xMod = 1;
		} else {
			xMod = -1;
		}
		
		int loadingXPosition;
		int loadXTo;
		int loadingYPosition;
		int loadYTo;
		
/*                  ↓↓ Loading this part
   		 _ _ _ _ _ _ _
		|	_ _ _ _	  |
		|  |	   |  |
		|  |	.  |  |
		|  |_ _ _ _|  |
		|_ _ _ _ _ _ _|
*/		
		
		if (x) {
			
			// X LOADING
			loadingXPosition = Mathf.RoundToInt (xPos + xMod * cameraSizeX);
			loadXTo = Mathf.RoundToInt (xPos + xMod * (cameraSizeX + bufferSize));
			
			loadingYPosition = Mathf.RoundToInt (yPos - cameraSizeY - bufferSize);
			loadYTo = Mathf.RoundToInt (yPos + (cameraSizeY + bufferSize));
			
			if (goingRight) {
				for (float i = loadingXPosition; i <= loadXTo; i += widthOfGroundPiece) {
					for (float j = loadingYPosition; j < loadYTo; j += widthOfGroundPiece) {
						loadPeice (i, j, STANDARD_LAYER);
						loadPeice (i, j, WALL_LAYER);
						loadPeice (i, j, GRASS_LAYER);
					}
				}
			} else {
				for (float i = loadingXPosition; i >= loadXTo; i -= widthOfGroundPiece) {
					for (float j = loadingYPosition; j < loadYTo; j += widthOfGroundPiece) {
						loadPeice (i, j, STANDARD_LAYER);
						loadPeice (i, j, WALL_LAYER);
						loadPeice (i, j, GRASS_LAYER);
					}
				}
			}
		}
/*		 _ _ _ _ _ _ _
		|	_ _ _ _	  | <- Loading this part
		|  |	   |  |
		|  |	.  |  |
		|  |_ _ _ _|  |
		|_ _ _ _ _ _ _|
*/
		if (y) {
			
			// Y LOADING
			loadingXPosition = Mathf.RoundToInt (xPos - cameraSizeX - bufferSize);
			loadXTo = Mathf.RoundToInt (xPos + cameraSizeX + bufferSize);
			
			loadingYPosition = Mathf.RoundToInt (yPos + yMod * cameraSizeY);
			loadYTo = Mathf.RoundToInt (yPos + yMod * (cameraSizeY + bufferSize));
			
			if (goingUp) {
				for (float j = loadingYPosition; j < loadYTo; j += widthOfGroundPiece) {
					for (float i = loadingXPosition; i <= loadXTo; i += widthOfGroundPiece) {
						loadPeice (i, j, STANDARD_LAYER);
						loadPeice (i, j, WALL_LAYER);
						loadPeice (i, j, GRASS_LAYER);
					}
				}
			} else {
				for (float j = loadingYPosition; j > loadYTo; j -= widthOfGroundPiece) {
					for (float i = loadingXPosition; i <= loadXTo; i += widthOfGroundPiece) {
						loadPeice (i, j, STANDARD_LAYER);
						loadPeice (i, j, WALL_LAYER);
						loadPeice (i, j, GRASS_LAYER);
					}
				}
			}
		}
	}
	
	public static void loadPeice(float i, float j, float k) {

		Vector3 pos = new Vector3 (i, j, k);

		if (Physics2D.OverlapPoint (pos, -1, k-0.01f, k+0.01f) != null) {
			return;
		}

		string tile = (string) map[pos];
		if(tile != null) {
            GameObject obj = Dictionary.get(tile);

            if(!obj.name.Equals("Block(Clone)")) {
                obj.transform.position = pos;
            } else {
                obj = (GameObject) GameObject.Instantiate(obj, pos, Quaternion.identity);
                obj.transform.SetParent(ground.transform);
                obj.isStatic = false;
            }
		}
	}

	public static void unloadPeice(GameObject gm) {
		Vector3 pos = gm.transform.position;
		Item script = gm.GetComponent<Item>();
		if (map.Contains (pos)) {
			if(map[pos].Equals(script.name))
				map.Remove(pos);
			else
				return;
		}
		map.Add (pos, script.name);
		Dictionary.remove(gm);
	}

	// Saves point to the map
	public static void unloadPeice(float i, float j, float k) {
		Vector3 pos = new Vector3 (i, j, k);
		Collider2D box = Physics2D.OverlapPoint(pos);
		
		if(map.Contains(pos)) {
			map.Remove(pos);
		}
		
		Item script;
		if(box != null && (script = box.gameObject.GetComponent<Item>()) != null) {
			map.Add(pos, script.name);
			Dictionary.remove(box.gameObject);
		}
	}
}
