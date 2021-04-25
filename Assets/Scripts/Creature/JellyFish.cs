using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFish : Creature
{
    public Transform player;

    private float thrustRate = .5f;
    private float nextThrust = 0f;

    public float thrustSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        damagePenalty = 1;
        volume = 5f;
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 5f;
        FindTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FindTarget();
        GoToTarget();
        buoyantForce = fluidDensity * volume * -Physics2D.gravity;
        rb.AddForce(buoyantForce);
    }

    public override void FindTarget()
    {
        target = new Vector2(player.position.x, player.position.y);
        targetAcquired = true;
    }

    public override void GoToTarget()
    {
        Vector2 move_dir = new Vector2(transform.position.x, transform.position.y) - target;
        if(Time.fixedTime >= nextThrust)
        {
            rb.AddForce(move_dir.normalized * -thrustSpeed, ForceMode2D.Impulse);
            nextThrust = Time.fixedTime + 1f / thrustRate;
        }
    }

    public override void Idle()
    {
        throw new System.NotImplementedException();
    }

    public override void Collided(Collision2D col)
    {
        rb.AddForce(-col.relativeVelocity.normalized * 100f, ForceMode2D.Impulse);
    }
}
