using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryImage : MonoBehaviour {

    public bool equipped = false;

    void Start() {
        EventTrigger t = gameObject.AddComponent<EventTrigger>();
        
        EventTrigger.Entry e = new EventTrigger.Entry();
        e.eventID = EventTriggerType.PointerEnter;
        e.callback.AddListener((eventData) => { OnPointerEnter(); });

        EventTrigger.Entry f = new EventTrigger.Entry();
        f.eventID = EventTriggerType.PointerExit;
        f.callback.AddListener((eventData) => { OnPointerExit(); });

        t.triggers.Add(e);
        t.triggers.Add(f);
    }

    void OnPointerEnter() {
        if (GetComponent<Image>().color == Color.green) {
            equipped = true;
            return;
        } else
            equipped = false;
        GetComponent<Image>().color = Color.green;
    }

    void OnPointerExit() {
        if (equipped) return;
        GetComponent<Image>().color = Color.white;
        
    }
}
