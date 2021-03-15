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
        int _bank = PlayerPrefs.GetInt("Currency");
        _bank += coins;
        PlayerPrefs.SetInt("Currency", _bank);
        _currencyText.text = "" + _bank.ToString();

    }
    private void TakeMoneyFromBank(int coins)
    {
        int _bank = PlayerPrefs.GetInt("Currency");
        _bank -= coins;
        PlayerPrefs.SetInt("Currency", _bank);
        _currencyText.text = "" + _bank.ToString();

    }
}
