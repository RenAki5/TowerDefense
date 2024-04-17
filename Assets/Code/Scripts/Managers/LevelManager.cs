using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform[] path;
    public Transform startpoint;

    public int currency;
    public int rallyCurrency;  // New variable for Rally currency

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 200;
        rallyCurrency = 0; // Initialize Rally currency
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough currency.");
            return false;
        }
    }

    // New method to increase Rally currency
    public void IncreaseRallyCurrency(int amount)
    {
        rallyCurrency += amount;
    }

    // Optionally, create a method to access the Rally currency, if needed
    public int GetRallyCurrency()
    {
        return rallyCurrency;
    }
}