using System;
using UnityEngine;

public class BankManager : MonoBehaviour
{

    public static int _bank;
    public event EventHandler OnBalanceChange;
    public static BankManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (PlayerPrefs.HasKey("Currency"))
        {
            _bank = PlayerPrefs.GetInt("Currency");
        }
        else
        {
            _bank = 0;
            PlayerPrefs.SetInt("Currency", _bank);
        }
    }

    public void AddMoneyToBank(int coins)
    {
        _bank += coins;
        PlayerPrefs.SetInt("Currency", _bank);
        OnBalanceChange?.Invoke(this, EventArgs.Empty);
    }

    public void TakeMoneyFromBank(int coins)
    {
        _bank -= coins;
        PlayerPrefs.SetInt("Currency", _bank);
        OnBalanceChange?.Invoke(this, EventArgs.Empty);
    }
}
