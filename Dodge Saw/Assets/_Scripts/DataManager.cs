using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
    [Header("Reference to StoreManager.cs")]
    public StoreManager storeManager;
    [Header("Meta")]
    public string persisterName;
    [Header("These Lists need to be Identical in Size")]
    public List<Item> objectsToPersist;
    public List<ShopItem> objectsToReference;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < objectsToPersist.Count; i++)
        {
            if(objectsToPersist[i].itemType == Item.ItemType.Player)
            {
                storeManager.shopPlayerItems.Add(objectsToPersist[i]);
            }
            else if(objectsToPersist[i].itemType == Item.ItemType.Death)
            {
                storeManager.shopDeathItems.Add(objectsToPersist[i]);
            }
            else if(objectsToPersist[i].itemType == Item.ItemType.Trail)
            {
                storeManager.shopTrailItems.Add(objectsToPersist[i]);
            }
            else if(objectsToPersist[i].itemType == Item.ItemType.Enemies)
            {
                storeManager.shopEnemyItems.Add(objectsToPersist[i]);
            }
            else if(objectsToPersist[i].itemType == Item.ItemType.Warp)
            {
                storeManager.shopWarpItems.Add(objectsToPersist[i]);
            }
            else if (objectsToPersist[i].itemType == Item.ItemType.IAP)
            {
                storeManager.shopIAPItems.Add(objectsToPersist[i]);
            }
        }
    }

    public bool SavedFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/Store_Data");
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
            FileStream file = File.Create(Application.persistentDataPath + "/Store_Data" + string.Format("/{0}_{1}.pso", persisterName, i));
            var json = JsonUtility.ToJson(objectsToPersist[i]);
            bf.Serialize(file, json);
            file.Close();
        }
    }

    public void OnEnable()
    {
        //TODO we need to find a way to make objectsToPersist a private list and then fill it with add instead of having to ensure both of the lists are equal in size
        for (int i = 0; i < objectsToPersist.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/Store_Data" + string.Format("/{0}_{1}.pso", persisterName, i)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/Store_Data" + string.Format("/{0}_{1}.pso", persisterName, i), FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), objectsToPersist[i]);
                file.Close();
                if(objectsToReference.Count > objectsToPersist.Count)
                {
                    for (int j = objectsToPersist.Count; j < objectsToReference.Count; j++)
                    {
                        objectsToPersist.Add(new Item(objectsToReference[j].backgroundImage, objectsToReference[j].texture, objectsToReference[j].color, objectsToReference[j].color2, objectsToReference[j].gradient, objectsToReference[j]._itemName, objectsToReference[j]._buttonText, objectsToReference[j].cost, objectsToReference[j].equipped, objectsToReference[j].purchased, objectsToReference[j].itemType));
                    }
                }
            }
            else
            {
                //these lists need to be identical in size
                objectsToPersist[i].backgroundImage = objectsToReference[i].backgroundImage;
                objectsToPersist[i].texture = objectsToReference[i].texture;
                objectsToPersist[i].color = objectsToReference[i].color;
                objectsToPersist[i].color2 = objectsToReference[i].color2;
                objectsToPersist[i].gradient = objectsToReference[i].gradient;
                objectsToPersist[i].itemName = objectsToReference[i]._itemName;
                objectsToPersist[i].buttonText = objectsToReference[i]._buttonText;
                objectsToPersist[i].cost = objectsToReference[i].cost;
                objectsToPersist[i].equipped = objectsToReference[i].equipped;
                objectsToPersist[i].purchased = objectsToReference[i].purchased;
                objectsToPersist[i].itemType = objectsToReference[i].itemType;
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
}
