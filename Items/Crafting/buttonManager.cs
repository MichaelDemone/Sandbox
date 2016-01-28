using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonManager : MonoBehaviour {

    public GameObject buildingBuilding;
    public GameObject background;
    public GameObject Message;
    private CraftingMike cm;

    void Start() {
        cm = GameObject.Find("Crafting").GetComponent<CraftingMike>();
    }

    public void exit() {

        restart();
        buildingBuilding.transform.localScale = new Vector2(0,0);
        
        Time.timeScale = 1;
    }

    public void create() {
        if (!cm.settingHold) {
            cm.settingHold = true;
            Message.GetComponent<Text>().text = "Now click where you want to hold the weapon";
        }
    }

    public void create(int y, int x) {
        string name = GetComponentInChildren<InputField>().text;
        cm.craftItem(name, y, x);
        Message.GetComponent<Text>().text = "Done!";

    }

    public void restart() {
        cm.returnItems();
        cm.settingHold = false;
        Message.GetComponent<Text>().text = "";
    }

    public void showOnPlayer() {

    }
}
