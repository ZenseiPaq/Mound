using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ExtendableArmController : MonoBehaviour
{
    public Transform armEnd;
    public Transform playerCamera;
    public float extendSpeed = 5f;
    public float retractSpeed = 5f;
    public float maxReach = 10f;
    public float stoppingDistance = 0.1f;

    public Image crosshair;

    private Vector3 initialPosition;
    private bool isExtending;
    private bool isRetracting;
    private GameObject grabbedObject;
    private Rigidbody armEndRigidbody;

    void Start()
    {
        initialPosition = armEnd.localPosition;

        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }

        armEndRigidbody = armEnd.GetComponent<Rigidbody>();
        if (armEndRigidbody == null)
        {
            Debug.LogError("The armEnd requires a Rigidbody component.");
        }
        else
        {
            armEndRigidbody.isKinematic = false;
            armEndRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            armEndRigidbody.useGravity = false;
            armEndRigidbody.drag = 5f; // Add drag to smooth out movement
            armEndRigidbody.angularDrag = 10f; // High angular drag to prevent rotation
            armEndRigidbody.constraints = RigidbodyConstraints.FreezeRotation; // Freeze rotation to prevent wobbling
        }
    }

    void Update()
    {
        HandleInput();

        if (isRetracting)
        {
            RetractArm();
        }

        UpdateCrosshair();
    }

    void HandleInput()
    {
        if (Input.GetButton("Fire1") && !isRetracting)
        {
            ExtendArm();
        }
        else if (Input.GetButtonUp("Fire1") && !isRetracting)
        {
            isRetracting = true;
        }
    }

    void ExtendArm()
    {
        isExtending = true;
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * maxReach;

        // Calculate the desired velocity towards the target position
        Vector3 direction = (targetPosition - armEnd.position).normalized;
        Vector3 desiredVelocity = direction * extendSpeed;

        // Apply velocity directly, this keeps the movement stable
        armEndRigidbody.velocity = Vector3.Lerp(armEndRigidbody.velocity, desiredVelocity, Time.deltaTime * 5f);

        // Check for stopping condition
        if (Vector3.Distance(armEnd.position, targetPosition) < stoppingDistance)
        {
            armEndRigidbody.velocity = Vector3.zero; // Stop the movement
            isExtending = false;
        }

        CheckForGrabbable();
    }

    void RetractArm()
    {
        Vector3 direction = (armEnd.parent.TransformPoint(initialPosition) - armEnd.position).normalized;
        Vector3 desiredVelocity = direction * retractSpeed;

        // Apply velocity directly to move back towards the initial position
        armEndRigidbody.velocity = Vector3.Lerp(armEndRigidbody.velocity, desiredVelocity, Time.deltaTime * 5f);

        // Check if the arm is near the initial position
        if (Vector3.Distance(armEnd.position, armEnd.parent.TransformPoint(initialPosition)) < stoppingDistance)
        {
            armEndRigidbody.velocity = Vector3.zero; // Stop the movement
            isRetracting = false;
            isExtending = false;
            grabbedObject = null;
        }
    }

    void CheckForGrabbable()
    {
        Collider[] hits = Physics.OverlapSphere(armEnd.position, 0.5f);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Grabbable") && grabbedObject == null)
            {
                GrabObject(hit.gameObject);
                break;
            }
        }
    }

    void GrabObject(GameObject obj)
    {
        Destroy(obj);
        Debug.Log("Object grabbed and deleted: " + obj.name);
        isRetracting = true;
    }

    void UpdateCrosshair()
    {
        if (crosshair != null)
        {
            crosshair.color = isRetracting ? Color.red : Color.white;
        }
    }
}
