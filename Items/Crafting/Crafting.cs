using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Crafting : MonoBehaviour {

	public Vector3[][] craftingGrid;
	private Vector3 topLeft;

	// Use this for initialization
	void Start () {

		craftingGrid[0][0] = new Vector3(0,0,0);
		craftingGrid[0][1] = new Vector3(0,1,0);
		craftingGrid[0][2] = new Vector3(0,2,0);
		craftingGrid[0][3] = new Vector3(0,3,0);
		craftingGrid[0][4] = new Vector3(0,4,0);
		craftingGrid[0][5] = new Vector3(0,5,0);
		craftingGrid[0][6] = new Vector3(0,6,0);

		craftingGrid[1][0] = new Vector3(1,0,0);
		craftingGrid[1][1] = new Vector3(1,1,0);
		craftingGrid[1][2] = new Vector3(1,2,0);
		craftingGrid[1][3] = new Vector3(1,3,0);
		craftingGrid[1][4] = new Vector3(1,4,0);
		craftingGrid[1][5] = new Vector3(1,5,0);
		craftingGrid[1][6] = new Vector3(1,6,0);

		craftingGrid[2][0] = new Vector3(2,0,0);
		craftingGrid[2][1] = new Vector3(2,1,0);
		craftingGrid[2][2] = new Vector3(2,2,0);
		craftingGrid[2][3] = new Vector3(2,3,0);
		craftingGrid[2][4] = new Vector3(2,4,0);
		craftingGrid[2][5] = new Vector3(2,5,0);
		craftingGrid[2][6] = new Vector3(2,6,0);

		craftingGrid[3][0] = new Vector3(3,0,0);
		craftingGrid[3][1] = new Vector3(3,1,0);
		craftingGrid[3][2] = new Vector3(3,2,0);
		craftingGrid[3][3] = new Vector3(3,3,0);
		craftingGrid[3][4] = new Vector3(3,4,0);
		craftingGrid[3][5] = new Vector3(3,5,0);
		craftingGrid[3][6] = new Vector3(3,6,0);

		craftingGrid[4][0] = new Vector3(4,0,0);
		craftingGrid[4][1] = new Vector3(4,1,0);
		craftingGrid[4][2] = new Vector3(4,2,0);
		craftingGrid[4][3] = new Vector3(4,3,0);
		craftingGrid[4][4] = new Vector3(4,4,0);
		craftingGrid[4][5] = new Vector3(4,5,0);
		craftingGrid[4][6] = new Vector3(4,6,0);

		craftingGrid[5][0] = new Vector3(5,0,0);
		craftingGrid[5][1] = new Vector3(5,1,0);
		craftingGrid[5][2] = new Vector3(5,2,0);
		craftingGrid[5][3] = new Vector3(5,3,0);
		craftingGrid[5][4] = new Vector3(5,4,0);
		craftingGrid[5][5] = new Vector3(5,5,0);
		craftingGrid[5][6] = new Vector3(5,6,0);

		craftingGrid[6][0] = new Vector3(6,0,0);
		craftingGrid[6][1] = new Vector3(6,1,0);
		craftingGrid[6][2] = new Vector3(6,2,0);
		craftingGrid[6][3] = new Vector3(6,3,0);
		craftingGrid[6][4] = new Vector3(6,4,0);
		craftingGrid[6][5] = new Vector3(6,5,0);
		craftingGrid[6][6] = new Vector3(6,6,0);


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
