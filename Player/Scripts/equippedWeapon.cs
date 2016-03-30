using UnityEngine;
using System.Collections;

public class equippedWeapon : usableItem {



	// Use this for initialization
	new void Start () { 
        base.Start();
	    foreach(Transform t in GetComponentsInChildren<Transform>()) {
            t.gameObject.AddComponent<parentReport>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void unequip() {
        foreach (SpriteRenderer sr in sprites) {
            sr.sprite = null;
            sr.GetComponent<Collider2D>().enabled = false;
        }
        transform.localPosition = new Vector2(0, 0);
        equipped = false;
    }

    public void equip(usableItem u) {
        for (int i = 0 ; i < sprites.GetLength(0) ; i++) {
            for (int j = 0 ; j < sprites.GetLength(1) ; j++) {

                // Set these sprites to the original ones.
                sprites[i, j].sprite = u.sprites[i, j].sprite;

                // If the sprite was not set, delete it
                if (sprites[i, j].sprite == null) {
                    sprites[i, j].GetComponent<BoxCollider2D>().enabled = false;
                    continue;
                } else {
                    sprites[i, j].GetComponent<BoxCollider2D>().enabled = true;
                }
            }
            equipped =true;
        }

        // Set the values
        strength = u.strength;
        sharpness = u.sharpness;
        speed = u.speed;
        durability = u.durability;

        // Shift to where the player wants it.
        // [1,0] is normally where the weapon is held from
        float newy = (u.xholdPosition - 1) * transform.localScale.y;
        float newx = -1*u.yholdPosition * transform.localScale.x;
        Vector2 newPos = new Vector2(newx, newy);
        transform.localPosition = newPos;
    }

    bool attacking = false;

    public void attack() {
        if (equipped && !attacking) {
            StartCoroutine(swing());
            attacking = true;
        }
    }

    public int rot;

    IEnumerator swing() {
        Vector3 v = transform.localPosition;
        for (float f = 0 ; f <= 10 ; f+=1) {

            Vector2 point = transform.parent.position;
            point.y -= 0.5f;

            transform.RotateAround(point, new Vector3(0, 0, 1), rot);

            if (transform.parent.localScale.x < 0) {
                transform.RotateAround(point, new Vector3(0, 0, 1), -2*rot);
                transform.Rotate(new Vector3(0, 0, 1), 2*rot);
            }

            yield return null;
        }
        transform.localPosition = v;
        transform.rotation = Quaternion.identity;
        attacking = false;
    }

    public void childHit(Collider2D other) {
        
        if (attacking && other.CompareTag("Enemy")) {
            int mod = transform.parent.localScale.x > 0 ? 1 : -1;
            other.GetComponent<BlobMovement>().hit(mod*10, 10, 1);
        }
    }
}
