using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Header and Serialized Field are for the inspector menu in Unity. These values can modified from the Unity Editor without changing the code.
    [Header("Referneces")]
    [SerializeField] private Rigidbody2D rb;            //rigidbody for collision detection


    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;    //bullet travel speed
    [SerializeField] private int bulletDamage = 1;      //damage dealt by each bullet
    [SerializeField] private string targetTag;          //Script is being reused for both enemies and friendly towers. String must match the Tag on the prefab ("Enemy" or "Tower").

    private Transform target;                           //target enemy of the bullet

    //Set the target for the bullet
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    //Set the damage of the bullet, useful for upgrading damage on enemies/towers
    public void SetDamage(int _bulletDamage)
    {
        bulletDamage = _bulletDamage;
    }

    //FixedUpdate is basically the same as Update, but called the same number of times per second, regardless of framerate
    private void FixedUpdate()
    {
        //if there is no target, destroy bullet (temp fix)
        if (!target)
        {
            Destroy(gameObject);
            return;
        }
        //get the direction towards current target enemy
        Vector2 direction = (target.position - transform.position).normalized;
        
        //rotates the bullet to point towards target enemy
        RotateTowardsTarget();

        //move towards target enemy
        rb.velocity = direction * bulletSpeed;
    }

    //Rotates the bullet towards target enemy (copied from the Tower script)
    private void RotateTowardsTarget()
    {
        //find the angle towards target
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        //rotates towards target based on determined angle
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1000);
    }

    //what to do on collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == targetTag)
        {
            //deal damage to the enemy collided with
            other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
            //destroy the bullet
            Destroy(gameObject);
        }
    }
}
