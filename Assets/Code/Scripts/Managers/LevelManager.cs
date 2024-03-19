using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    //list of all the pathing points for the enemies
    public Transform[] path;
    //the starting point of the path
    public Transform startpoint;

    //currency for the game
    public int currency;


    //Called when the script instance is loaded
    private void Awake()
    {
        main = this;
    }

    //Called by Unity at the start of the Scene
    private void Start()
    {
        //set the starting currency
        currency = 200;
    }

    //increase the currency
    public void IncreaseCurrency(int ammount)
    {
        currency += ammount;
    }

    //spend the currency
    public bool SpendCurrency(int ammount)
    {
        //check that there is enough currency to buy the tower/upgrade
        if (ammount <= currency)
        {
            // Buy Item
            currency -= ammount;
            return true;
        } else
        {
            Debug.Log("You do not have enough to purchase this item.");
            return false;
        }
    }
}
