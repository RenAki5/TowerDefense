using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    //Header and Serialized Field are for the inspector menu in Unity. These values can modified from the Unity Editor without changing the code.
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;     //rotation point for tower head
    [SerializeField] private LayerMask enemyMask;               //the enemy layer mask
    [SerializeField] private GameObject bulletPrefab;           //prefab for tower's bullet
    [SerializeField] private Transform firingPoint;             //firing point for bullets
    [SerializeField] private GameObject upgradeUI;              //upgrade UI menu
    [SerializeField] private Button upgradeButton;              //upgrade button
    
    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;         //tower range
    [SerializeField] private float rotationSpeed = 5f;          //tower rotation speed
    [SerializeField] private float bps = 1f;                    // Bullets Per Second
    [SerializeField] private int baseUpgradeCost = 100;         //base cost to upgrade

    private float bpsBase;                                      //base values for bps and targetingrange to calculate upgrades
    private float targetingRangeBase;

    private Transform target;                                   //target enemy
    private float timeUntilFire;                                //time until the next shot is taken

    private int level = 1;                                      //current tower level
    private bool isMelee = false;

    //Called by Unity at the start of the Scene
    private void Start()
    {
        //set the base values to turret starting stats
        bpsBase = bps;
        targetingRangeBase = targetingRange;

        //create a listener for the upgrade button
        upgradeButton.onClick.AddListener(Upgrade);
    }

    //Unity calls Update every frame
    private void Update()
    {
        //if there is currently no target, find one in range
        if (target == null)
        {
            FindTarget();
            return;
        }

        //rotate tower towards the target enemy
        RotateTowardsTarget();

        //if target is no longer in range, remove target
        if (!CheckTargetIsInRange())
        {
            target = null;
        } 
        //otherwise shoot at fire
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f/bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    //Shoot bullets
    private void Shoot()
    {
        //spawn a bullet, then set its target to the turret's target
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    //Find a target
    private void FindTarget()
    {
        //checks all GameObjects on the enemyMask layer within the range of the tower and adds them to a list
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        //if there is anything in the list, set the target to first enemy in the list
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    //Check that a target is range of the tower
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    //rotate tower towards target enemy
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    //Shows the range of the tower in the editor
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    //opens the upgrade UI
    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    //closes the upgrade UI
    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    //Upgrade the tower
    public void Upgrade()
    {
        //if player doesn't have enough money, do nothing
        if (CalculateCost() > LevelManager.main.currency) return;

        //if they do, spend the money
        LevelManager.main.SpendCurrency(CalculateCost());

        //level the tower up
        level++;

        //calculate the new Bullets/Second and tower Range, and set the stats to those.
        bps = CalculateBPS();
        targetingRange = CalculateRange();

        //close the upgrade UI
        CloseUpgradeUI();
        Debug.Log("New BPS: " + bps);
        Debug.Log("New Range: " + targetingRange);
        Debug.Log("New Cost: " + CalculateCost());
    }

    //calculates the cost of upgrades
    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    //calculates the bullets/second from upgrades
    private float CalculateBPS()
    {
        return (bpsBase * Mathf.Pow(level, 0.6f));
    }

    //calculates the range from upgrades
    private float CalculateRange()
    {
        return (targetingRangeBase * Mathf.Pow(level, 0.4f));
    }
}
