using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Shop Item Database", menuName = "Shop/Shop Item/Database")]
public class ShopItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public Item[] shopItems;
    public Dictionary<Item, int> GetId = new Dictionary<Item, int>();

    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<Item, int>();
        for (int i = 0; i < shopItems.Length; i++)
        {
            for (int j = 0; j < shopItems.Length; j++)
            {
                GetId.Add(shopItems[i], i);

            }
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
