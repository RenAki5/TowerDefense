using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    //Header and Serialized Field are for the inspector menu in Unity. These values can modified from the Unity Editor without changing the code.
    //Ex. The "Tank" enemy has a lower movespeed than the normal "Enemy"
    [Header("References")]
    //Rigidbody for collision detection
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private LayerMask turretMask;
    [SerializeField] private int atkDamage = 1;
    [SerializeField] private float atkps = 1f;
    [SerializeField] private float targetingRange = 1f;

    //Target destination (pulled from the path)
    private Transform target;
    private int pathIndex = 0;

    //used for attacking
    private float timeUntilAttack;
    private Transform attackTarget;

    //used to remember the base movement speed to reset if speed is altered (like by the slowmo tower)
    private float baseSpeed;

    private bool isAttacking = false;

    //Called by Unity at the start of the Scene
    private void Start()
    {
        //setting the baseline movespeed to reset to it when needed
        baseSpeed = moveSpeed;
        //finding the first target of the path
        target = LevelManager.main.path[pathIndex];
    }

    //Update is called every frame
    private void Update()
    {
        //if the enemy has reached it's current path destination, incriment to the next destination index
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            //if this is the end of the path, destroy the enemy (and later reduce health of the player)
            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                LevelManager.main.lives--;
                Destroy(gameObject);
                if (LevelManager.main.lives == 0)
                {
                    LevelManager.main.gameOver.SetActive(true);
                    Time.timeScale = 0;
                }
            }
            //Otherwise, set the target as the current path index
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
        //If there is not an attack target, reset speed, then find an attack target if able
        if (attackTarget == null)
        {
            ResetSpeed();
            FindTarget();            
        }
        //If the enemy is attacking, attack
        if (isAttacking)
        {
            timeUntilAttack += Time.deltaTime;

            if (timeUntilAttack >= 1f / atkps)
            {
                Shoot();
                timeUntilAttack = 0f;
            }
        }
    }

    //FixedUpdate is basically the same as Update, but called the same number of times per second, regardless of framerate
    private void FixedUpdate()
    {
        //Find the direction to the current target in the path
        Vector2 direction = (target.position - transform.position).normalized;

        //move towards it
        rb.velocity = direction * moveSpeed;
    }

    private void FindTarget()
    {
        //checks all GameObjects on the turretMask layer within the range of the tower and adds them to a list (Ranged and Melee towers are on different layers)
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, turretMask);

        //if there is anything in the list, set the target to first enemy in the list
        if (hits.Length > 0)
        {
            //set target to first enemy in the list, stop moving, and begin attacking
            attackTarget = hits[0].transform;
            UpdateSpeed(0);
            isAttacking = true;
        }
    }

    //Updates the speed (currently used for the slowmo turret to slow down enemies)
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    //Reset back to the base speed
    public void ResetSpeed()
    {
        moveSpeed = baseSpeed; ;
    }

    //Shoots at targeted Turret
    private void Shoot()
    {
        //spawn a bullet, then set its target to the turret's target
        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(attackTarget);
        bulletScript.SetDamage(atkDamage);
    }

    //Draws targeting range in the inspector
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
