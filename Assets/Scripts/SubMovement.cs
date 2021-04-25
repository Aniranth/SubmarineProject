using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMovement : MonoBehaviour
{

    Rigidbody2D rb;

    // input vectors
    Vector2 debugMovement;

    // physics things
    const float fluidDensity = 1f; // g/cm3

    // submarine stats
    public float volume = 10f; // this will probably change with testing
    public float minWeight = 10f; // weight of sub without any crewmates or ballast

    public float throttleChangeRate = 5f; // increase to change throttle faster
    public float ballastChangeRate = 1f; // increase to change ballast faster
    public float rotationChangeRate = 3f; // increase to change rotation faster

    public float thrustLocation = -1.5f; // relative location of thruster

    // movement constraints
    public float debugMoveSpeed = 5f;
    public float minThrottle = 0f, maxThrottle = 1000f;
    public float minBallast = 0f, maxBallast = 50f; // sub starts positively buoyant.
    public float minRotation = -30f, maxRotation = 30f;

    // sub targets
    float targetThrottle = 0f;
    float targetBallast = 0f;
    float targetRotation = 0f;

    // sub actuals
    float currentThrottle = 0f;
    float currentBallast = 0f;
    float currentRotation = 0f;

    // forces upon the sub
    Vector2 buoyantForce;
    Vector2 thrustForce;
    Vector2 thrustPos;
    // plus gravity

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Vector2 rotateVector2(Vector2 v, float deg){
	float delta = Mathf.Deg2Rad * deg;
	return new Vector2(
	    v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
	);
    }

    // Update is called once per frame
    void Update()
    {
	// debug inputs for if the submarine is stupid
        debugMovement.x = Input.GetAxis("Horizontal");
        debugMovement.y = Input.GetAxis("Vertical");
        rb.MovePosition(rb.position + debugMovement.normalized * debugMoveSpeed * Time.fixedDeltaTime);

	// update our targets using the inputs (will replace with onscreen tools for mouse dragging)
	targetThrottle += Input.GetAxis("Throttle");
	targetBallast += Input.GetAxis("Ballast");
	targetRotation += Input.GetAxis("Rotation");

	// make sure all the values are valid
	targetThrottle = Mathf.Clamp(targetThrottle, minThrottle, maxThrottle);
	targetBallast = Mathf.Clamp(targetBallast, minBallast, maxBallast);
	targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);

	// Debug.Log("Throttle: " + currentThrottle + " -> " + targetThrottle);
	// Debug.Log("Ballast:  " + currentBallast + " -> " + targetBallast);
	// Debug.Log("Rotation: " + currentRotation + " -> " + targetRotation);

	// now we update our currents
	float throttleDelta = targetThrottle - currentThrottle;
	currentThrottle += Mathf.Clamp(throttleDelta, -throttleChangeRate, throttleChangeRate) * Time.fixedDeltaTime;
	float ballastDelta = targetBallast - currentBallast;
	currentBallast += Mathf.Clamp(ballastDelta, -ballastChangeRate, ballastChangeRate) * Time.fixedDeltaTime;
	float rotationDelta = targetRotation - currentRotation;
	currentRotation += Mathf.Clamp(rotationDelta, -rotationChangeRate, rotationChangeRate) * Time.fixedDeltaTime;

	// now we determine our force vectors using our current state
	buoyantForce = fluidDensity * volume * -Physics.gravity;
	thrustForce = rb.transform.right * currentThrottle;
	// Debug.Log("b4rot: " + thrustForce);
	thrustForce = rotateVector2(thrustForce, currentRotation);
	// Debug.Log("afrot: " + thrustForce);
	thrustPos = rb.transform.TransformPoint(thrustLocation, 0, 0);

	// and now we add them to the sub
	rb.mass = minWeight + currentBallast;
	rb.AddForce(buoyantForce);
	rb.AddForceAtPosition(thrustForce, thrustPos);
	
    }
}
