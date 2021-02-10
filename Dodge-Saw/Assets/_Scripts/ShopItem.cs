using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Shop/Shop Item")]
public class ShopItem : ScriptableObject
{
    //ShopItem(image)
    // - Texture (Raw Image)
    // - name (Text)
    // - Cost (Text)
    //  - Coin (image)
    // - Unequip (button)
    // - Equip (button)
    // - Buy (button)

    //for the shopitem background image
    public Sprite backgroundImage;
    //for the Raw Image
    public Texture texture;
    public Color color;
    public Color color2;
    public Gradient gradient;

    //for the name
    public string _itemName;
    //for the buttons text
    public string _buttonText;

    public LayerMask _layer;

    //for the cost
    public int cost;

    //to manage if the item is equipped
    public bool equipped;
    //to manage if the item has been purchased
    public bool purchased;

    //to manage the type of item this is
    public ItemType itemType;

    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
    public enum ItemType { Player, Trail, Enemies, Warp, AltWarp, Death}
}
