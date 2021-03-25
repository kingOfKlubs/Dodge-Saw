using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugStore : MonoBehaviour
{
    public int amount;
    public TextMeshProUGUI _currencyText;

    public void Add()
    {
        AddMoneyToBank(amount);
    }

    public void Take()
    {
        TakeMoneyFromBank(amount);
    }

    private void AddMoneyToBank(int coins)
    {
        BankManager.instance.AddMoneyToBank(coins);
    }
    private void TakeMoneyFromBank(int coins)
    {
        BankManager.instance.TakeMoneyFromBank(coins);
    }
}
