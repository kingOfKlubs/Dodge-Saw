using UnityEngine;

[System.Serializable]
public class Item
{
    //for the shopitem background image
    public Sprite backgroundImage;
    //for the Raw Image
    public Texture texture;
    public Color color;
    public Color color2;
    public Gradient gradient;

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

    public Item(Sprite _backgroundImage, Texture _texture, Color _color, Color _color2, Gradient _gradient, string _itemName, string _buttonText, int _cost, bool _equipped, bool _purchased, ItemType _itemType)
    {
        backgroundImage = _backgroundImage;
        texture = _texture;
        color = _color;
        color2 = _color2;
        gradient = _gradient;
        itemName = _itemName;
        buttonText = _buttonText;
        cost = _cost;
        equipped = _equipped;
        purchased = _purchased;
        itemType = _itemType;
    }
}

public enum ItemType { Player, Trail, Enemies, Warp, AltWarp, Death, IAP }
