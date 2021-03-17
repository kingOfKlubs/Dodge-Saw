using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Percentage : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = PlayerPrefs.GetString("VolumePercent","75%");
    }

    public void ConvertToPertcentage(float value)
    {
        text.text = Mathf.Abs(value + 80) + "%";
        PlayerPrefs.SetString("VolumePercent", text.text);
    }
}
