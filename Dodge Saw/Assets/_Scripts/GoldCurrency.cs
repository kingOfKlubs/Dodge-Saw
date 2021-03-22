using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldCurrency : MonoBehaviour
{
    public TextMeshProUGUI _currencyText;

    private void Start()
    {
        BankManager.instance.OnBalanceChange += UpdateCurrencyText;
        _currencyText = GetComponentInChildren<TextMeshProUGUI>();
        _currencyText.text = PlayerPrefs.GetInt("Currency", 0).ToString();
    }

    private void OnDisable()
    {
        BankManager.instance.OnBalanceChange -= UpdateCurrencyText;
    }

    private void UpdateCurrencyText(object sender, EventArgs e)
    {
        _currencyText.text = BankManager._bank.ToString(); 
    }
}
