using UnityEngine;
using System;

[System.Serializable]
public class ItemData {

    //for the name
    public string itemName;
    //for the buttons text
    public string buttonText;

    //for the cost
    public int cost;

    //to manage if the item is equipped
    public bool equipped;
    //to manage if the item has been purchased
    public bool purchased;

    //to manage the type of item this is
    public ItemType itemType;


    public ItemData(Item item) {
        itemName = item.itemName;
        buttonText = item.buttonText;
        cost = item.cost;
        equipped = item.equipped;
        purchased = item.purchased;
        itemType = item.itemType;
    }
}
