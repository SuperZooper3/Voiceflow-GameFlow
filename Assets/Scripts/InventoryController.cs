using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public string[] inventory = new string[3];
    public Image[] slots = new Image[3];
    public InventoryItem[] items;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RefreshItemSprites();
    }

    public void RefreshItemSprites() {
        for (int i = 0; i < slots.Length; i++) {
            if (inventory[i] != "") {
                foreach (InventoryItem item in items) {
                    if (item.name == inventory[i]) {
                        slots[i].enabled = true;
                        slots[i].sprite = Sprite.Create(item.texture, new Rect(0, 0, item.texture.width, item.texture.height), new Vector2(0.5f, 0.5f));
                    }
                }
            } else {
                slots[i].enabled = false;
            }
        }
    }

    public void AddItem(string itemName) {
        if (inventory[0] == "") {
            inventory[0] = itemName;
        } else if (inventory[1] == "") {
            inventory[1] = itemName;
        } else if (inventory[2] == "") {
            inventory[2] = itemName;
        } else {
            inventory[0] = itemName;
            Debug.Log("Inventory full, replacing first item");
        }
    }

    public void RemoveItem(int slot) {
        inventory[slot] = "";
    }
}

[System.Serializable]
public class InventoryItem {
    public string name;
    public Texture2D texture;
}

