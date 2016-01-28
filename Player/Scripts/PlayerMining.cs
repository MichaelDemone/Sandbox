using UnityEngine;
using System.Collections;

public class PlayerMining : MonoBehaviour {

    float lastMineTime;

    Stats s;

	// Use this for initialization
	void Start () {
        s = GetComponent<Stats>();
        lastMineTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButton(0) && Time.time - lastMineTime > s.timeBetweenHits) {
            hit();
        }
	}

    void hit() {
        Vector3 mosPos = Input.mousePosition;
        mosPos = Camera.main.ScreenToWorldPoint(mosPos);
        mosPos.z = 0;
        if ((mosPos - transform.position).magnitude > s.miningDistance) return;

        Collider2D[] hits = Physics2D.OverlapPointAll(mosPos);

        Mining m;

        foreach (Collider2D hit in hits) {
            
            if ((m = hit.GetComponent<Mining>()) != null && !m.wall) {
                m.strength -= s.strength;
                if (m.strength <= 0) {
                    CreateMap.map.Remove(m.transform.position);
                    m.GetComponent<Item>().drop();
                    GameObject.Destroy(m.gameObject);
                }
            }
        }
    }
}
