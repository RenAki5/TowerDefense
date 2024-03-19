using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    //Header and Serialized Field are for the inspector menu in Unity. These values can modified from the Unity Editor without changing the code.
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;         //total hit points
    [SerializeField] private int currencyWorth = 50;    //money granted on death

    private bool isDestroyed = false;                   //determine if an enemy is to be destroyed (this is to fix a bug with enemies taking multiple instances of lethal damage)

    //Deal damage to target
    public void TakeDamage(int dmg)
    {
        //reduce HP by dmg
        hitPoints -= dmg;

        //if health is at or below 0, and it hasn't already been set to be destroyed, then destroy the GameObject and give the currency
        if (hitPoints <= 0 && !isDestroyed) {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

}
