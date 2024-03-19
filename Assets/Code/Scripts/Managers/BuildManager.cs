using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    //Header and Serialized Field are for the inspector menu in Unity. These values can modified from the Unity Editor without changing the code.
    [Header("References")]
    [SerializeField] private Tower[] towers;    //total different towers in the game

    private int selectedTower = 0;              //which tower in the list is currently selected

    //Called when the script instance is loaded
    private void Awake()
    {
        main = this;
    }

    //get the currently selected tower
    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    //set the currently selected tower
    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }

}
