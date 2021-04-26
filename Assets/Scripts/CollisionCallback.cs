using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCallback : MonoBehaviour
{
    [SerializeField] private string triggerString;

    public delegate void TriggerHit(string message);
    public static event TriggerHit TriggerCrossed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check to make sure sub collided
        SubMovement go = collision.gameObject.GetComponent<SubMovement>();
        if(go != null)
        {
            // we have found a sub
            Debug.Log(triggerString);
            TriggerCrossed?.Invoke(triggerString);
        }
    }
}
