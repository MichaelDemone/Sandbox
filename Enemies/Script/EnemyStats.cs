using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {

    public float health; 

	// Use this for initialization
	void Start () {
        health *= Tweakables.scale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
