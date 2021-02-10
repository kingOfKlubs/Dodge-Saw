using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldCurrency : MonoBehaviour
{
	
	public static int _bank;
	public Text _currencyText;
   
    

	// Start is called before the first frame update
	void Start()
	{
        //PlayerPrefs.SetInt("Currency", 505);
        _currencyText = GetComponentInChildren<Text>();
        if(PlayerPrefs.HasKey("Currency"))
        {
		    _bank = PlayerPrefs.GetInt("Currency");
        }
        else
        {
            _bank = 0;
            PlayerPrefs.SetInt("Currency", _bank);
        }

        _currencyText.text = "" + PlayerPrefs.GetInt("Currency", 0).ToString();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

    public void AddMoneyToBank(int coins)
    {
        _bank += coins;
        PlayerPrefs.SetInt("Currency", _bank);
        _currencyText.text = "" + _bank.ToString();

    }
    public void TakeMoneyFromBank(int coins)
    {
        _bank -= coins;
        PlayerPrefs.SetInt("Currency", _bank);
        _currencyText.text = "" + _bank.ToString();

    }
}
