using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{

    /// <summary>
    ///     to reset the store go to scriptableObjects folder and do it manually
    ///     make sure everything is unequipped first
    /// </summary>


    [Header("List of Items Sold")]
    [SerializeField] private ShopItem[] shopPlayerItems;
    [SerializeField] private ShopItem[] shopTrailItems;
    [SerializeField] private ShopItem[] shopEnemyItems;
    [SerializeField] private ShopItem[] shopWarpItems;
    [SerializeField] private ShopItem[] shopDeathItems;
    [SerializeField] private ShopItem[] shopIAPItems;

    [Header("References")]
    [SerializeField] private Transform PlayerPage;
    [SerializeField] private Transform TrailPage;
    [SerializeField] private Transform EnemyPage;
    [SerializeField] private Transform WarpPage;
    [SerializeField] private Transform DeathPage;
    [SerializeField] private Transform IAPPage;
    [SerializeField] private GameObject shopItemPrefab;

    [SerializeField] private Sprite EquippedItemBackground;
    [SerializeField] private Sprite UnequippedItemBackground;
    public GameObject player;
    GameObject us;

    private CanvasGroup[] CanvasGroup;
    private GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {

        PopulateShop();
    }

    public void Buy(ShopItem item)
    {
        if (GoldCurrency._bank >= item.cost)
        {
            Debug.Log("you just purchased this item");
            GoldCurrency gc = FindObjectOfType<GoldCurrency>();
            gc.TakeMoneyFromBank(item.cost);
            item.purchased = true;
            item._buttonText = "Equip";
            PopulateShop();
        }
        else
            UnableToPurchase();
    }

    public void Equip(ShopItem item)
    {
        if(item.itemType == ShopItem.ItemType.Player)
        {
            // player.GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor",item.color);
            
            PlayerPrefs.SetFloat("_playerColor.r", item.color.r);
            PlayerPrefs.SetFloat("_playerColor.g", item.color.g);
            PlayerPrefs.SetFloat("_playerColor.b", item.color.b);

        }
        if(item.itemType == ShopItem.ItemType.Trail)
        {
            Debug.Log("Equipped new trial");
            //PlayerPrefs.SetFloat("_trailColor.r", item.color.r);
            //PlayerPrefs.SetFloat("_trailColor.g", item.color.g);
            //PlayerPrefs.SetFloat("_trailColor.b", item.color.b);
            PlayerPrefsX.SetColor("_trailGradient1", item.gradient.colorKeys[0].color);
            PlayerPrefsX.SetColor("_trailGradient2", item.gradient.colorKeys[1].color); 
            PlayerPrefsX.SetColor("_trailGradient3", item.gradient.colorKeys[2].color);

        }
        if (item.itemType == ShopItem.ItemType.Warp)
        {
            Debug.Log("Equipped new Warp Color");
            PlayerPrefsX.SetColor("_warpColor1", item.color);
            PlayerPrefsX.SetColor("_warpColor2", item.color2);
        }
        if (item.itemType == ShopItem.ItemType.AltWarp)
        {
            Debug.Log("Equipped new AltWarp Color");
            PlayerPrefsX.SetColor("_altWarpColor1", item.color);
            PlayerPrefsX.SetColor("_altWarpColor2", item.color2);
        }
        if (item.itemType == ShopItem.ItemType.Enemies)
        {
            Debug.Log("Equipped new AltWarp Color");
            PlayerPrefsX.SetColor("EnemyColor", item.color);
            //PlayerPrefsX.SetColor("_altWarpColor2", item.color2);
        }
        if (item.itemType == ShopItem.ItemType.Death)
        {
            Debug.Log("Equipped new DeathEffect Color");
            PlayerPrefsX.SetColor("_deathGradient1", item.gradient.colorKeys[0].color);
            PlayerPrefsX.SetColor("_deathGradient2", item.gradient.colorKeys[1].color);
            PlayerPrefsX.SetColor("_deathGradient3", item.gradient.colorKeys[2].color);
        }
        item.backgroundImage = EquippedItemBackground;
        item.equipped = true;
        item._buttonText = "Unequip";
        PopulateShop();
    }

    public void Unequip(ShopItem item)
    {
        if(item.itemType == ShopItem.ItemType.Player)
        {
           player.GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", Color.red);
        }
        if (item.itemType == ShopItem.ItemType.Trail)
        {
            Debug.Log("Unequipped new trial");
        }
        item.backgroundImage = UnequippedItemBackground;
        item.equipped = false;
        item._buttonText = "Equip";
        PopulateShop();
    }

    public void UnableToPurchase()
    { Debug.Log("You don't have sufficient funds to purchase this item"); }

    void PopulateShop()
    {
        //this is to update the page when changes are made
        items = GameObject.FindGameObjectsWithTag("ShopItem");
        for (int i = 0; i < items.Length; i++)
        {
            Destroy(items[i]);
        }

        //this is for setting up the shop items
        for (int i = 0; i < shopPlayerItems.Length; i++)
        {
            ShopItem si = shopPlayerItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, PlayerPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)

            //assign image from the prefab to the instantiated object
            shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            //assign texture from the prefab to the instantiated object
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().texture = si.texture;
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = si._itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = si._buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Unequip(si));
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Buy(si));
                si._buttonText = "Buy";
            }


        }
        for (int i = 0; i < shopTrailItems.Length; i++)
        {
            ShopItem si = shopTrailItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, TrailPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)

            //assign image from the prefab to the instantiated object
            shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            //assign texture from the prefab to the instantiated object
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().texture = si.texture;
            //shopItemObject.transform.GetChild(0).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = si._itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = si._buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Unequip(si));
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Buy(si));
                si._buttonText = "Buy";
            }

        }
        for (int i = 0; i < shopEnemyItems.Length; i++)
        {
            ShopItem si = shopEnemyItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, EnemyPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)

            //assign image from the prefab to the instantiated object
            shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            //assign texture from the prefab to the instantiated object
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().texture = si.texture;
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = si._itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = si._buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Unequip(si));
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Buy(si));
                si._buttonText = "Buy";
            }

        }
        for (int i = 0; i < shopWarpItems.Length; i++)
        {
            ShopItem si = shopWarpItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, WarpPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)

            //assign image from the prefab to the instantiated object
            shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            //assign texture from the prefab to the instantiated object
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().texture = si.texture;
            //shopItemObject.transform.GetChild(0).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = si._itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = si._buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Unequip(si));
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Buy(si));
                si._buttonText = "Buy";
            }

        }
        for (int i = 0; i < shopDeathItems.Length; i++)
        {
            ShopItem si = shopDeathItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, DeathPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)

            //assign image from the prefab to the instantiated object
            shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            //assign texture from the prefab to the instantiated object
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().texture = si.texture;
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = si._itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = si._buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Unequip(si));
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Buy(si));
                si._buttonText = "Buy";
            }

        }
        for (int i = 0; i < shopIAPItems.Length; i++)
        {
            ShopItem si = shopIAPItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, IAPPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)

            //assign image from the prefab to the instantiated object
            shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            //assign texture from the prefab to the instantiated object
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().texture = si.texture;
            shopItemObject.transform.GetChild(0).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = si._itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = si._buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Unequip(si));
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => Buy(si));
                si._buttonText = "Buy";
            }

        }
    }
    public void UnequipOtherEquippedItems()
    {
       //the current idea is to check other items of the same type if they are equipped with the ShopItem bool equipped
       //if so change thier backgrounds and button text
    }

}
