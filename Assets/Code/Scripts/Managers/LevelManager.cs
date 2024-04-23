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
    public int lives;
    public GameObject gameOver;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 200;
        rallyCurrency = 0; // Initialize Rally currency
        lives = 3;
        gameOver.SetActive(false);
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
    public bool SpendRally(int amount)
    {
        if (amount <= rallyCurrency)
        {
            rallyCurrency -= amount;
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
