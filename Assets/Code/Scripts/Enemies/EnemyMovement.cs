using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Header and Serialized Field are for the inspector menu in Unity. These values can modified from the Unity Editor without changing the code.
    //Ex. The "Tank" enemy has a lower movespeed than the normal "Enemy"
    [Header("References")]
    //Rigidbody for collision detection
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private LayerMask turretMask;
    [SerializeField] private int atkDamage = 1;
    [SerializeField] private float atkps = 1f;

    //Target destination (pulled from the path)
    private Transform target;
    private int pathIndex = 0;

    //used for attacking
    private float timeUntilAttack;
    private GameObject attackTarget;

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
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) {
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
                
                return;
            } 
            //Otherwise, set the target as the current path index
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
        if (isAttacking)
        {
            timeUntilAttack += Time.deltaTime;

            if (timeUntilAttack >= 1f / atkps)
            {
                Debug.Log("Attacking " + attackTarget);
                attackTarget.gameObject.GetComponent<Health>().TakeDamage(atkDamage);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UpdateSpeed(0f);
        isAttacking = true;
        attackTarget = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetSpeed();
        isAttacking = false;
        attackTarget = null;
    }
}
