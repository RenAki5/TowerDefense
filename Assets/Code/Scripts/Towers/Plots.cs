using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plots : MonoBehaviour
{
    //Header and Serialized Field are for the inspector menu in Unity. These values can modified from the Unity Editor without changing the code.
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;     //sprite renderer
    [SerializeField] private Color hoverColor;      //what color the plot becomes when hovered
    [SerializeField] private bool isPath;           //used to designate between normal plots and plots on the enemy path

    private GameObject towerObj;                    //used to check what tower is built on top
    public Turret turret;                           //used to check what tower is built on top
    public TurretSlowmo turretSlowmo;               //used to check what tower is built on top
    public TurretMelee turretMelee;                 //used to check what tower type is built
    private Color startColor;                       //starting plot color

    //Called by Unity at the start of the Scene
    private void Start()
    {
        startColor = sr.color;
    }

    //When mouse enters the plot
    private void OnMouseEnter()
    {
        sr.color = hoverColor;  //change the color to the hover color
    }

    //when mouse leaves the plot
    private void OnMouseExit()
    {
        sr.color = startColor;  //change back to start color
    }

    //When mouse is left-clicked
    private void OnMouseDown()
    {
        //if hovering over the upgrade UI, don't build a tower (to prevent player from accidently building towers on plots while upgrading towers)
        if (UIManager.main.IsHoveringUI()) return;

        //if there is a tower on the plot
        if (towerObj != null)
        {
            //check which tower type, then open the upgrade UI
            if (turret != null)
            {
                turret.OpenUpgradeUI();
                return;
            }
            if (turretSlowmo != null)
            {
                turretSlowmo.OpenUpgradeUI();
                return;
            }
            if (turretMelee != null)
            {
                turretMelee.OpenUpgradeUI();
                return;
            }
        }


        //grab the currently selected tower
        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        //make sure the tower is melee for a path plot
        if (isPath && !towerToBuild.isMelee)
        {
            Debug.Log("You can only build melee towers here");
            return;
        }

        if (!isPath && towerToBuild.isMelee)
        {
            Debug.Log("You cannot place Melee towers here");
            return;
        }

        //make sure the tower can be afforded, if not, do nothing
        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("You can't afford this tower");
            return;

        }
        if (towerToBuild.rally > LevelManager.main.rallyCurrency)
        {
            Debug.Log("You can't afford this tower");
            return;
        }

        //spend the currency for the tower
        LevelManager.main.SpendCurrency(towerToBuild.cost);
        LevelManager.main.SpendRally(towerToBuild.rally);


        //build the tower, then set either turret or turretSlowmo with the prefab of built tower
        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
        turretSlowmo = towerObj.GetComponent<TurretSlowmo>();
        turretMelee = towerObj.GetComponent<TurretMelee>();
    }
}
