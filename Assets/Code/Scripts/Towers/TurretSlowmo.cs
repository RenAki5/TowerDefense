using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TurretSlowmo : MonoBehaviour
{
    //Header and Serialized Field are for the inspector menu in Unity. These values can modified from the Unity Editor without changing the code. (This is pretty similar to the "Turret" script)
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;           //enemy layer mask
    [SerializeField] private GameObject upgradeUI;          //upgrade UI
    [SerializeField] private Button upgradeButton;          //upgrade button

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;     //range of the tower
    [SerializeField] private float aps = 4f;                // Attacks Per Second
    [SerializeField] private float freezeTime = 1f;         //how long enemies have their speed reduced
    [SerializeField] private int baseUpgradeCost = 200;     //upgrade cost baseline

    private float timeUntilFire;                            //time until next freeze

    private int level = 1;                                  //turret level

    private float apsBase;                                   //base values for aps and targetingrange to calculate upgrades
    private float targetingRangeBase;

    //Called by Unity at the start of the Scene
    private void Start()
    {
        //set the base values to turret starting stats
        apsBase = aps;
        targetingRangeBase = targetingRange;

        //create a listener for the upgrade button
        upgradeButton.onClick.AddListener(Upgrade);
    }

    //Unity calls Update every frame
    private void Update()
    {
        //add time to timeUntilFire    
        timeUntilFire += Time.deltaTime;
            
            //if timeUntilFire is equal or greater than aps, fire the Freeze on enemies, then reset timeUntilFire
            if (timeUntilFire >= 1f / aps)
            {
                FreezeEnemies();
                timeUntilFire = 0f;
            }
    }

    //the Freeze attack
    private void FreezeEnemies()
    {
        //grab all enemies within the tower's range, and place them in a list
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        //if there are enemies in the list
        if (hits.Length > 0 )
        {
            //go through the list, and reduce the enemies speed.
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    //Reset enemy speed after the freeze is over
    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        //wait until freezeTime seconds have passed
        yield return new WaitForSeconds(freezeTime);

        //then reset the speed of the enemy
        em.ResetSpeed();
    }

    //Shows the range of the tower in the editor
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    //Opens upgrade UI
    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    //Closes upgrade UI
    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    //Upgrades tower
    public void Upgrade()
    {
        //if the player doesn't have enough money to upgrade, do nothing
        if (CalculateCost() > LevelManager.main.currency) return;

        //if they do, spend the money
        LevelManager.main.SpendCurrency(CalculateCost());

        //level up the tower
        level++;

        //then calculate the new Attacks/Second and range, and set the stats
        aps = CalculateAPS();
        targetingRange = CalculateRange();

        //lastly, close the upgrade UI
        CloseUpgradeUI();
        Debug.Log("New APS: " + aps);
        Debug.Log("New Range: " + targetingRange);
        Debug.Log("New Cost: " + CalculateCost());
    }

    //calculate new upgrade cost
    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    //calculate new Attacks/Second
    private float CalculateAPS()
    {
        return (apsBase * Mathf.Pow(level, 0.3f));
    }

    //calculate new tower Range
    private float CalculateRange()
    {
        return (targetingRangeBase * Mathf.Pow(level, 0.2f));
    }
}
