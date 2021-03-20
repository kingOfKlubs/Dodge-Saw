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
    public List<Item> shopPlayerItems;
    public List<Item> shopTrailItems;
    public List<Item> shopEnemyItems;
    //public List<Item> shopWarpItems;
    public List<Item> shopDeathItems;
    public List<Item> shopIAPItems;

    [Header("References")]
    [SerializeField] private Transform PlayerPage;
    [SerializeField] private Transform TrailPage;
    [SerializeField] private Transform EnemyPage;
    [SerializeField] private Transform WarpPage;
    [SerializeField] private Transform DeathPage;
    [SerializeField] private Transform IAPPage;
    [SerializeField] private GameObject shopItemPrefab;

    [Header("Background Images")]
    [SerializeField] private Sprite EquippedItemBackground;
    [SerializeField] private Sprite UnequippedItemBackground;

    [Header("Textures(This in the order for 'List of Items Sold')")]
    [SerializeField] private Texture[] shopTextures;

    [Header("Store's Purchase Effect")]
    [SerializeField] private ParticleSystem CoinFallEffect;

    public GameObject player;
    public GameObject NotEnoughFundsText;
    public float shakeDistance;
    public float shakeTime;

    private GameObject us;
    private CanvasGroup[] CanvasGroup;
    private GameObject[] items;
    private AudioManager audioManager;
    private DataManager dataManager = new DataManager();

    // Start is called before the first frame update
    void Start()
    {
        PopulateShop();
    }

    public void Buy(Item item, GameObject itemObject)
    {
        if (GoldCurrency._bank >= item.cost)
        {
            Debug.Log("you just purchased this item");
            GoldCurrency gc = FindObjectOfType<GoldCurrency>();
            gc.TakeMoneyFromBank(item.cost);
            item.purchased = true;
            item.buttonText = "Equip";
            audioManager.Play("ChaChing");
            CoinFallEffect.Play();
            dataManager.SaveItem(item);
            PopulateShop();
        }
        else
            UnableToPurchase(itemObject);
    }

    public void Equip(Item item)
    {

        if(item.itemType == ItemType.Player)
        {
            // player.GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor",item.color);
            
            for (int i = 0; i < shopPlayerItems.Count; i++)
            {
                if(item != shopPlayerItems[i])
                {
                    Unequip(shopPlayerItems[i]);
                }
            }
            
            PlayerPrefs.SetFloat("_playerColor.r", item.color.r);
            PlayerPrefs.SetFloat("_playerColor.g", item.color.g);
            PlayerPrefs.SetFloat("_playerColor.b", item.color.b);

            Color _playerColor = Color.clear;
            _playerColor.r = PlayerPrefs.GetFloat("_playerColor.r");
            _playerColor.g = PlayerPrefs.GetFloat("_playerColor.g");
            _playerColor.b = PlayerPrefs.GetFloat("_playerColor.b");

            player.GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);

        }
        if(item.itemType == ItemType.Trail)
        {
            Debug.Log("Equipped new trial");

            for (int i = 0; i < shopTrailItems.Count; i++)
            {
                if (item != shopTrailItems[i])
                {
                    Unequip(shopTrailItems[i]);
                }
            }
            PlayerPrefsX.SetColor("_trailGradient1", item.gradient.colorKeys[0].color);
            PlayerPrefsX.SetColor("_trailGradient2", item.gradient.colorKeys[1].color); 
            PlayerPrefsX.SetColor("_trailGradient3", item.gradient.colorKeys[2].color);
        }
        //if (item.itemType == Item.ItemType.Warp)
        //{
        //    for (int i = 0; i < shopPlayerItems.Count; i++)
        //    {
        //        if (item != shopWarpItems[i])
        //        {
        //            Unequip(shopWarpItems[i]);
        //        }
        //    }

        //    Debug.Log("Equipped new Warp Color");
        //    PlayerPrefsX.SetColor("_warpColor1", item.color);
        //    PlayerPrefsX.SetColor("_warpColor2", item.color2);
        //}
        if (item.itemType == ItemType.AltWarp)
        {
            Debug.Log("Equipped new AltWarp Color");
            PlayerPrefsX.SetColor("_altWarpColor1", item.color);
            PlayerPrefsX.SetColor("_altWarpColor2", item.color2);
        }
        if (item.itemType == ItemType.Enemies)
        {
            for (int i = 0; i < shopEnemyItems.Count; i++)
            {
                if (item != shopEnemyItems[i])
                {
                    Unequip(shopEnemyItems[i]);
                }
            }
            Debug.Log("Equipped new AltWarp Color");
            PlayerPrefsX.SetColor("EnemyColor", item.color);
            //PlayerPrefsX.SetColor("_altWarpColor2", item.color2);
        }
        if (item.itemType == ItemType.Death)
        {
            for (int i = 0; i < shopDeathItems.Count; i++)
            {
                if (item != shopDeathItems[i])
                {
                    Unequip(shopDeathItems[i]);
                }
            }
            Debug.Log("Equipped new DeathEffect Color");
            PlayerPrefsX.SetColor("_deathGradient1", item.gradient.colorKeys[0].color);
            PlayerPrefsX.SetColor("_deathGradient2", item.gradient.colorKeys[1].color);
            PlayerPrefsX.SetColor("_deathGradient3", item.gradient.colorKeys[2].color);
        }
        item.backgroundImage = EquippedItemBackground;
        item.equipped = true;
        item.buttonText = "Equipped";
        audioManager.Play("Equip");
        PopulateShop();
    }

    public void Unequip(Item item)
    {
        item.backgroundImage = UnequippedItemBackground;
        item.equipped = false;
        item.buttonText = "Equip";
        PopulateShop();
    }

    public void UnableToPurchase(GameObject item)
    {
        audioManager.Play("Denied");
        LeanTween.rotateAround(item,new Vector3(0,0,1),shakeDistance, shakeTime).setLoopPingPong(3);
        NotEnoughFundsText.SetActive(true);
        Debug.Log("You don't have sufficient funds to purchase this item");
    }

    void PopulateShop()
    {
        //this is to update the page when changes are made
        items = GameObject.FindGameObjectsWithTag("ShopItem");
        for (int i = 0; i < items.Length; i++)
        {
            Destroy(items[i]);
        }

        //this is for setting up the shop items
        for (int i = 0; i < shopPlayerItems.Count; i++)
        {
            Item si = shopPlayerItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, PlayerPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Panel
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)

            //assign image from the prefab to the instantiated object
            if(si.backgroundImage != null) {
                shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            }
            else {
                if (si.equipped) { shopItemObject.GetComponent<Image>().sprite = EquippedItemBackground; }
                else { shopItemObject.GetComponent<Image>().sprite = UnequippedItemBackground; }
            }

            //assign texture from the prefab to the instantiated object
            if (si.texture != null) {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = si.texture;
            }
            else {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture  = shopTextures[0];
            }
                        
            shopItemObject.transform.GetChild(1).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = si.buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
            {
                Debug.Log(si.itemName + " is Equipped");
            }
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Buy(si, shopItemObject));
                si.buttonText = "Buy";
            }


        }
        for (int i = 0; i < shopTrailItems.Count; i++)
        {
            Item si = shopTrailItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, TrailPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Panel
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)


            //assign image from the prefab to the instantiated object
            if (si.backgroundImage != null) {
                shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            }
            else {
                if (si.equipped) { shopItemObject.GetComponent<Image>().sprite = EquippedItemBackground; }
                else { shopItemObject.GetComponent<Image>().sprite = UnequippedItemBackground; }
            }

            //assign texture from the prefab to the instantiated object
            if (si.texture != null) {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = si.texture;
            }
            else {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = shopTextures[1];
            }


            //shopItemObject.transform.GetChild(0).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = si.buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
            {
                Debug.Log(si.itemName + " is Equipped");
            }
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Buy(si, shopItemObject));
                si.buttonText = "Buy";
            }

        }
        for (int i = 0; i < shopEnemyItems.Count; i++)
        {
            Item si = shopEnemyItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, EnemyPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Panel
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)


            //assign image from the prefab to the instantiated object
            if (si.backgroundImage != null) {
                shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            }
            else {
                if (si.equipped) { shopItemObject.GetComponent<Image>().sprite = EquippedItemBackground; }
                else { shopItemObject.GetComponent<Image>().sprite = UnequippedItemBackground; }
            }

            if (si.texture != null) {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = si.texture;
            }
            else {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = shopTextures[2];
            }

            shopItemObject.transform.GetChild(1).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = si.buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
            {
                Debug.Log(si.itemName + " is Equipped");
            }
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Buy(si, shopItemObject));
                si.buttonText = "Buy";
            }

        }
        //for (int i = 0; i < shopWarpItems.Count; i++)
        //{
        //    Item si = shopWarpItems[i];
        //    GameObject shopItemObject = Instantiate(shopItemPrefab, WarpPage);

        //    //this access' the prefabs's component, and change it based off your ShopItem struct
        //    //ShopItem(image)
        //    // - Panel
        //    // - Texture (Raw Image)
        //    // - name (Text)
        //    // - Cost (Text)
        //    //  - Coin (image)
        //    // - Unequip (button)
        //    // - Equip (button)
        //    // - Buy (button)

        //    //assign image from the prefab to the instantiated object
        //    if (si.backgroundImage != null) {
        //        shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
        //    }
        //    else {
        //        if (si.equipped) { shopItemObject.GetComponent<Image>().sprite = EquippedItemBackground; }
        //        else { shopItemObject.GetComponent<Image>().sprite = UnequippedItemBackground; }
        //    }

        //    //assign texture from the prefab to the instantiated object
        //    if (si.texture != null) {
        //        shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = si.texture;
        //    }
        //    else {
        //        shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = shopTextures[3];
        //    }

        //    //shopItemObject.transform.GetChild(0).GetComponent<RawImage>().color = si.color;
        //    //assign the name from the prefab to the instantiated object
        //    shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.itemName;
        //    //assign the cost from the pregab to the instantiated object
        //    shopItemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
        //    //assign the buttons from the prefab to the instantiated object
        //    shopItemObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = si.buttonText;

        //    //Grab button, assign a function to it's onClick event
        //    if (si.purchased && si.equipped)
        //    {
        //        Debug.Log(si.itemName + " is Equipped");
        //    }
        //    else if (si.purchased && !si.equipped)
        //        shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Equip(si));
        //    else
        //    {
        //        shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Buy(si, shopItemObject));
        //        si.buttonText = "Buy";
        //    }

        ////}
        for (int i = 0; i < shopDeathItems.Count; i++)
        {
            Item si = shopDeathItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, DeathPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Panel
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)

            //assign image from the prefab to the instantiated object
            if (si.backgroundImage != null) {
                shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            }
            else {
                if (si.equipped) { shopItemObject.GetComponent<Image>().sprite = EquippedItemBackground; }
                else { shopItemObject.GetComponent<Image>().sprite = UnequippedItemBackground; }
            }

            //assign texture from the prefab to the instantiated object
            if (si.texture != null) {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = si.texture;
            }
            else {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = shopTextures[4];
            }

            shopItemObject.transform.GetChild(1).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = si.buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
            {
                Debug.Log(si.itemName + " is Equipped");
            }
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Buy(si, shopItemObject));
                si.buttonText = "Buy";
            }

        }
        for (int i = 0; i < shopIAPItems.Count; i++)
        {
            Item si = shopIAPItems[i];
            GameObject shopItemObject = Instantiate(shopItemPrefab, IAPPage);

            //this access' the prefabs's component, and change it based off your ShopItem struct
            //ShopItem(image)
            // - Panel
            // - Texture (Raw Image)
            // - name (Text)
            // - Cost (Text)
            //  - Coin (image)
            // - Unequip (button)
            // - Equip (button)
            // - Buy (button)

            //assign image from the prefab to the instantiated object
            if (si.backgroundImage != null) {
                shopItemObject.GetComponent<Image>().sprite = si.backgroundImage;
            }
            else {
                if (si.equipped) { shopItemObject.GetComponent<Image>().sprite = EquippedItemBackground; }
                else { shopItemObject.GetComponent<Image>().sprite = UnequippedItemBackground; }
            }

            //assign texture from the prefab to the instantiated object
            if (si.texture != null) {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = si.texture;
            }
            else {
                shopItemObject.transform.GetChild(1).GetComponent<RawImage>().texture = shopTextures[6];
            }

            shopItemObject.transform.GetChild(1).GetComponent<RawImage>().color = si.color;
            //assign the name from the prefab to the instantiated object
            shopItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = si.itemName;
            //assign the cost from the pregab to the instantiated object
            shopItemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = si.cost.ToString();
            //assign the buttons from the prefab to the instantiated object
            shopItemObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = si.buttonText;

            //Grab button, assign a function to it's onClick event
            if (si.purchased && si.equipped)
            {
                Debug.Log(si.itemName + " is Equipped");
            }
            else if (si.purchased && !si.equipped)
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Equip(si));
            else
            {
                shopItemObject.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Buy(si, shopItemObject));
                si.buttonText = "Buy";
            }

        }
    }
}
