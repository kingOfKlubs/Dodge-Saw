﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {

    [Header("Reference to StoreManager.cs")]
    public StoreManager storeManager;
    [Header("Meta")]
    public string persisterName;
    
    [Header("These Lists need to be Identical in Size")]
    [SerializeField] private List<Item> items;
    [SerializeField] private List<ItemData> objectsToPersist;
    [SerializeField] private List<ShopItem> objectsToReference;

    public bool SavedFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/Store_Data");
    }

    public void SaveItem(Item item) {
        ItemData _item = new ItemData(item);
        //TODO add saving to the objects to Persist list here
        for (int i = 0; i < objectsToPersist.Count; i++){
            if (_item.itemName == objectsToPersist[i].itemName && _item.itemType == objectsToPersist[i].itemType)
            {
                objectsToPersist[i].itemName = _item.itemName;
                objectsToPersist[i].buttonText = _item.buttonText;
                objectsToPersist[i].cost = _item.cost;
                objectsToPersist[i].equipped = _item.equipped;
                objectsToPersist[i].purchased = _item.purchased;
                objectsToPersist[i].itemType = _item.itemType;
            }
        }
    }

    public void SaveGame()
    {
        if (!SavedFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Store_Data");
        }
        for (int i = 0; i < objectsToPersist.Count; i++)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Create(Application.persistentDataPath + "/Store_Data" + string.Format("/{0}_{1}.pso", persisterName, i))) {
                try
                {
                    bf.Serialize(file, objectsToPersist[i]);
                }
                catch (IOException e)
                {
                    throw new Exception("You had an error saving the game data. The Error was: " + e);
                    Debug.LogError("You had an error saving the game data. The Error was: " + e);
                }
                finally
                {
                    if(file != null)
                    {
                        file.Close();
                    }
                }
            }
        }
    }

    public void OnEnable()
    {
        //TODO we need to find a way to make objectsToPersist a private list and then fill it with add instead of having to ensure both of the lists are equal in size
        for (int i = 0; i < objectsToReference.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/Store_Data" + string.Format("/{0}_{1}.pso", persisterName, i)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream file = File.Open(Application.persistentDataPath + "/Store_Data" + string.Format("/{0}_{1}.pso", persisterName, i), FileMode.Open))
                {
                    try
                    {
                        ItemData tempItem = (bf.Deserialize(file)) as ItemData;
                        items[i].backgroundImage = objectsToReference[i].backgroundImage;
                        items[i].texture = objectsToReference[i].texture;
                        items[i].color = objectsToReference[i].color;
                        items[i].color2 = objectsToReference[i].color2;
                        items[i].gradient = objectsToReference[i].gradient;
                        items[i].itemName = objectsToReference[i]._itemName;
                        items[i].buttonText = tempItem.buttonText;
                        items[i].cost = objectsToReference[i].cost;
                        items[i].equipped = tempItem.equipped;
                        items[i].purchased = tempItem.purchased;
                        items[i].itemType = tempItem.itemType;
                    }
                    catch (IOException e)
                    {
                        throw new Exception("You had an error loading your saved data. The error was: " + e);
                        Debug.LogError("You had an error loading your saved data. The error was: " + e);
                    }
                    finally
                    {
                        if(file != null)
                        {
                            file.Close();
                        }
                    }
                }
                objectsToPersist[i].itemName = items[i].itemName;
                objectsToPersist[i].buttonText = items[i].buttonText;
                objectsToPersist[i].cost = items[i].cost;
                objectsToPersist[i].equipped = items[i].equipped;
                objectsToPersist[i].purchased = items[i].purchased;
                objectsToPersist[i].itemType = items[i].itemType;
            }
            else
            {
                items[i].backgroundImage = objectsToReference[i].backgroundImage;
                items[i].texture = objectsToReference[i].texture;
                items[i].color = objectsToReference[i].color;
                items[i].color2 = objectsToReference[i].color2;
                items[i].gradient = objectsToReference[i].gradient;
                items[i].itemName = objectsToReference[i]._itemName;
                items[i].buttonText = objectsToReference[i]._buttonText;
                items[i].cost = objectsToReference[i].cost;
                items[i].equipped = objectsToReference[i].equipped;
                items[i].purchased = objectsToReference[i].purchased;
                items[i].itemType = objectsToReference[i].itemType;
                objectsToPersist[i].itemName = objectsToReference[i]._itemName;
                objectsToPersist[i].buttonText = objectsToReference[i]._buttonText;
                objectsToPersist[i].cost = objectsToReference[i].cost;
                objectsToPersist[i].equipped = objectsToReference[i].equipped;
                objectsToPersist[i].purchased = objectsToReference[i].purchased;
                objectsToPersist[i].itemType = objectsToReference[i].itemType;
            }
        }

        for (int i = 0; i < items.Count; i++) {
            if (items[i].itemType == ItemType.Player) {
                storeManager.shopPlayerItems.Add(items[i]);
            }
            else if (items[i].itemType == ItemType.Death) {
                storeManager.shopDeathItems.Add(items[i]);
            }
            else if (items[i].itemType == ItemType.Trail) {
                storeManager.shopTrailItems.Add(items[i]);
            }
            else if (items[i].itemType == ItemType.Skin) {
                storeManager.shopSkinItems.Add(items[i]);
            }
            else if (objectsToPersist[i].itemType == ItemType.Cosmetic)
            {
                storeManager.shopCosmeticItems.Add(items[i]);
            }
        }
    }

    public void OnDisable()
    {
        SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnDistroy() {
        SaveGame();
    }
}
