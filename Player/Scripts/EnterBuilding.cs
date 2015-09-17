using UnityEngine;
using System.Collections;

public class EnterBuilding : MonoBehaviour {
	

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			CastRay();
		}   

	}
	
	void CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit) {
			if (hit.collider.gameObject.name == "BuildingBuilding"){
				for (int i = 0; i < InventoryUI.images.Length; i++){
					Object.DontDestroyOnLoad(InventoryUI.images[i]);
					Object.DontDestroyOnLoad(InventoryUI.imageNums[i]);
				}
				Application.LoadLevel(2);

			}
		}
	}    
}