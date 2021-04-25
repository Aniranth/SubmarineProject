using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    ParticleSystem bubbleSystem;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        bubbleSystem = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 vel = rb.velocity;

        var emission = bubbleSystem.emission;
        emission.rateOverTimeMultiplier = Mathf.Ceil(vel.magnitude * 5);
    }
}
