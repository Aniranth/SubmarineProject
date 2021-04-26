using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int pointValue;

    public delegate void CollectableAcquired(int value);
    public static event CollectableAcquired Collect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check to make sure sub collided
        SubMovement go = collision.gameObject.GetComponent<SubMovement>();
        if (go != null)
        {
            // we have found a sub
            Collect?.Invoke(pointValue);
            Destroy(gameObject);
        }
    }

}
