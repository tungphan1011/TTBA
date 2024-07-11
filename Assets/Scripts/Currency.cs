using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField] int amount;
    [SerializeField] TMPro.TextMeshProUGUI text;

    private const string AmountKey = "CurrentAmount";

    private void Start()
    {
        amount = PlayerPrefs.GetInt(AmountKey, 1000);
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = $"{amount} Gold";
    }

    public void Add(int moneyGain)
    {
        amount += moneyGain;
        SaveAmount();
        UpdateText();
    }

    internal bool Check(int totalPrice)
    {
        return amount >= totalPrice;
    }

    internal void Decrease(int totalPrice)
    {
        amount -= totalPrice;
        if (amount < 0) { amount = 0; }
        SaveAmount();
        UpdateText();
    }

    private void SaveAmount()
    {
        PlayerPrefs.SetInt(AmountKey, amount);
        PlayerPrefs.Save();
    }
}
