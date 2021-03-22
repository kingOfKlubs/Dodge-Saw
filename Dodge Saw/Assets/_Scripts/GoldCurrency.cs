using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldCurrency : MonoBehaviour
{
	
	public static int _bank;
	public TextMeshProUGUI[] _currencyText;
    GameObject[] _currencyTexts;

	// Start is called before the first frame update
	void Start()
	{
        //PlayerPrefs.SetInt("Currency", 505);
        _currencyText = FindObjectsOfType<TextMeshProUGUI>();
        if(PlayerPrefs.HasKey("Currency"))
        {
		    _bank = PlayerPrefs.GetInt("Currency");
        }
        else
        {
            _bank = 0;
            PlayerPrefs.SetInt("Currency", _bank);
        }
        foreach(TextMeshProUGUI text in _currencyText)
        {
            if(text.tag == "Currency")
            {
                text.text = "" + PlayerPrefs.GetInt("Currency", 0).ToString();
            }
        }
	}

    //private void LateUpdate()
    //{
    //    _currencyText.text = "" + PlayerPrefs.GetInt("Currency", 0).ToString();
    //}

    public void AddMoneyToBank(int coins)
    {
        _bank += coins;
        PlayerPrefs.SetInt("Currency", _bank);
        //_currencyText.text = "" + _bank.ToString();

    }
    public void TakeMoneyFromBank(int coins)
    {
        _bank -= coins;
        PlayerPrefs.SetInt("Currency", _bank);
        foreach (TextMeshProUGUI text in _currencyText)
        {
            if (text.tag == "Currency")
            {
                text.text = "" + _bank.ToString();
            }
        }
    }
    //public int GetCurrentBalance()
    //{
    //    return PlayerPrefs.GetInt("Currency",0);
    //}
}
