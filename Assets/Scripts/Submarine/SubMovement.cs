using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMovement : MonoBehaviour
{
    public delegate void UIForce(float yForce);
    public static event UIForce OnBallastChange;

    Rigidbody2D rb;
    PlayerHealth ph;

    // input vectors
    Vector2 debugMovement;

    // control sliders
    public Slider throttleSlider;
    public Slider ballastSlider;
    public Slider rotationSlider;

    // physics things
    const float fluidDensity = 1f; // g/cm3

    // submarine stats
    public float volume = 15f; // this will probably change with testing
    public float minWeight = 10f; // weight of sub without any crewmates or ballast

    public float throttleChangeRate = 30f; // increase to change throttle faster
    public float ballastChangeRate = 10f; // increase to change ballast faster
    public float rotationChangeRate = 10f; // increase to change rotation faster

    public float thrustLocation = -1.5f; // relative location of thruster

    // movement constraints
    public float debugMoveSpeed = 5f;
    public float minThrottle = 0f, maxThrottle = 300f;
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
        PlayerHealth.StartSink += SinkBallast;

        rb = GetComponent<Rigidbody2D>();
        ph = GetComponent<PlayerHealth>();
	
	// set throttle values
	throttleSlider.minValue = minThrottle;
	throttleSlider.maxValue = maxThrottle;
	throttleSlider.value = 0f;
	ballastSlider.minValue = minBallast;
	ballastSlider.maxValue = maxBallast;
	ballastSlider.value = 0f;
	rotationSlider.minValue = minRotation;
	rotationSlider.maxValue = maxRotation;
	rotationSlider.value = 0f;

	// become a subscriber to the sliders
	throttleSlider.onValueChanged.AddListener(delegate{setTargetThrottle();});
	ballastSlider.onValueChanged.AddListener(delegate{setTargetBallast();});
	rotationSlider.onValueChanged.AddListener(delegate{setTargetRotation();});

    }

    Vector2 rotateVector2(Vector2 v, float deg){
	float delta = Mathf.Deg2Rad * -deg;
	return new Vector2(
	    v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
	);
    }

    void setTargetThrottle(){
        if(!ph.getDead())
        {
            targetThrottle = throttleSlider.value;
        }
    }

    void setTargetBallast(){
        if(!ph.getDead())
        {
            targetBallast = ballastSlider.value;
        }
    }

    void setTargetRotation(){
	targetRotation = rotationSlider.value;
    }

    void SinkBallast()
    {
        targetBallast = 100;
        currentBallast = 100;
        targetThrottle = 0;
        currentThrottle = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
	buoyantForce = fluidDensity * volume * -Physics2D.gravity;
	thrustForce = rb.transform.right * currentThrottle;
	// Debug.Log("b4rot: " + thrustForce);
	thrustForce = rotateVector2(thrustForce, currentRotation);
	// Debug.Log("afrot: " + thrustForce);
	thrustPos = rb.transform.TransformPoint(thrustLocation, 0, 0);

	// and now we add them to the sub
	rb.mass = minWeight + currentBallast;
	rb.AddForce(buoyantForce);
    rb.AddForceAtPosition(thrustForce, thrustPos);
    OnBallastChange?.Invoke((buoyantForce + (Physics2D.gravity * rb.mass)).y);
	
    }
}
