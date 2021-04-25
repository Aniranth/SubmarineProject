using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float fluidDensity = 1f;
    protected float volume;

    public int damagePenalty; // How much does it hurt to run into

    protected Vector2 target;
    protected bool targetAcquired;

    protected Vector2 buoyantForce;

    public abstract void FindTarget(); // sets the target parameter via raycasts or however the creature behaves
    public abstract void GoToTarget(); // Moves towards target ... or swims away from target be creative!
    public abstract void Idle(); // what does the creature do when it has nothing else to do
    public abstract void Collided(Collision2D col); // When the creature runs into the sub how does it behave 
}
