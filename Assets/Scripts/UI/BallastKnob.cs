using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallastKnob : MonoBehaviour
{
    RectTransform ballastRect;
    private float resultantYForce;

    // Start is called before the first frame update
    void Start()
    {
        ballastRect = GetComponent<RectTransform>();
        SubMovement.OnBallastChange += this.UpdateResultantForce;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(resultantYForce > 0)
        {
            // convert from 0 to 50 -> 0 to 90
            ballastRect.rotation = Quaternion.Euler(new Vector3(0f, 0f, 9f/5f * resultantYForce));
        }
        else
        {
            // convert from (0, -440) to (0, -90)
            ballastRect.rotation = Quaternion.Euler(new Vector3(0f, 0f, 4.89f * resultantYForce));
        }
    }

    void UpdateResultantForce(float yForce)
    {
        resultantYForce = yForce;
        Debug.Log(yForce);
    }
}
