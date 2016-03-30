using UnityEngine;
using System.Collections;

public class usableItem : MonoBehaviour {

    public int xholdPosition, yholdPosition;
    public float sharpness, strength, speed, durability;
    public SpriteRenderer[,] sprites;

    public bool equipped = false;

    // Use this for initialization
    protected void Start() {
        sprites = new SpriteRenderer[7, 7];
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sr) {
            // Find column and row name.
            string name = s.name;
            name = name.Replace("Pos (", "").Replace(")", "");
            int columnNum = int.Parse(name);

            name = s.transform.parent.name;
            name = name.Replace("Row (", "").Replace(")", "");
            int rowNum = int.Parse(name);
            sprites[rowNum, columnNum] = s;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Setting the values for the weapon
    /// </summary>
    public void setValues(Item[,] items, string name, int strength, int sharpness, int speed, int durability, int x, int y) {

        Start();

        Item it = GetComponent<Item>();
        it.invName = name;
        it.name = name;

        this.strength = strength;
        this.sharpness = sharpness;
        this.speed = speed;
        this.durability = durability;
        this.xholdPosition = x;
        this.yholdPosition = y;

        for (int i = 0 ; i < items.GetLength(0) ; i++) {
            for (int j = 0 ; j < items.GetLength(1) ; j++) {
                if (items[i, j] == null) {
                    sprites[i, j].GetComponent<BoxCollider2D>().enabled = false;
                    continue;
                }
                sprites[i, j].sprite = items[i, j].inventorySprite;

            }
        }

        Inventory.addItem(this.gameObject, 1);
    }
}
