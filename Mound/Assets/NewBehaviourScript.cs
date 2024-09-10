using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class NewBehaviourScript : MonoBehaviour
{
    //public float evaluatePos = 0.5f;
    public SplineContainer spline;

    SplineAnimate splineAnimate;

    float currentSplinePos = 0;

    float currentSpeed = 0;
    public float acceleration = 20;
    public float maxSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        splineAnimate = GetComponent<SplineAnimate>();

        splineAnimate.NormalizedTime = currentSplinePos;
    }

    // Update is called once per frame
    void Update()
    {
        float currentInput = Input.GetAxis("Vertical");

        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed * Mathf.Sign(currentInput), acceleration * Time.deltaTime * Mathf.Abs(currentInput));

        float splineLength = splineAnimate.Container.CalculateLength();

        float normalizedSpeed = currentSpeed / splineLength;
        float normalizedMaxSpeed = maxSpeed / splineLength;

        normalizedSpeed = Mathf.Clamp(normalizedSpeed, -normalizedMaxSpeed, normalizedMaxSpeed);

        currentSplinePos += normalizedSpeed;

        currentSplinePos = Mathf.Clamp01(currentSplinePos);

        splineAnimate.NormalizedTime = currentSplinePos;

        //float3 splinePos;
        //float3 splineTangent;
        //float3 splineUpVector;

        //spline.Evaluate(evaluatePos, out splinePos, out splineTangent, out splineUpVector);

        //transform.position =  splinePos - (float3)spline.Spline.transform.position;
    }
}
