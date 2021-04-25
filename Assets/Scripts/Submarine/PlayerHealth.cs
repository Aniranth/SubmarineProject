using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public delegate void Sink();
    public static event Sink StartSink;

    public int maxPlayerHealth = 10; // Change this with heart containers
    public int currPlayerHealth = 10; // Value to damage

    private bool is_dead = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(is_dead)
        {
            // invoke delegate to play with ui game over you have crashed
            return;
        }
        Creature enemy = collision.gameObject.GetComponent<Creature>(); // Templatize with enemy when class is made
        if(enemy != null) // we have found an enemy in our collision look up damage
        {
            currPlayerHealth -= enemy.damagePenalty;
            enemy.Collided(collision); // ESCAPE!!!
            Debug.Log("Ouch avoid that next time!");
        }
        else // we ran into a wall damage based on speed
        {
            Vector2 collision_velocity = collision.relativeVelocity;
            if(collision_velocity.magnitude < .75f)
            {
                currPlayerHealth -= 1;
            } 
            else if (collision_velocity.magnitude < 1.25f && collision_velocity.magnitude >= .75f)
            {
                currPlayerHealth -= 2;
            }
            else
            {
                currPlayerHealth -= 3;
            }
            Debug.Log("WallCollision");
        }

        if(currPlayerHealth <= 0)
        {
            Die();
        }
        // add invuln frames health damage or free phasing movement?
    }

    private void Die()
    {
        is_dead = true;
        StartSink?.Invoke();
        Debug.Log("You have died");
    }

    public bool getDead()
    {
        return is_dead;
    }
}
