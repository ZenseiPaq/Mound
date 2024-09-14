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

    public Image crosshair; // Reference to the crosshair UI Image

    private Vector3 initialPosition;
    private bool isExtending;
    private bool isRetracting;
    private GameObject grabbedObject;

    void Start()
    {
        initialPosition = armEnd.localPosition;
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
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
        armEnd.position = Vector3.MoveTowards(armEnd.position, targetPosition, extendSpeed * Time.deltaTime);
        CheckForGrabbable();
    }

    void RetractArm()
    {
        armEnd.localPosition = Vector3.MoveTowards(armEnd.localPosition, initialPosition, retractSpeed * Time.deltaTime);

        if (Vector3.Distance(armEnd.localPosition, initialPosition) < 0.01f)
        {
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
        obj.SetActive(false);
        grabbedObject = obj;
        Debug.Log("Object grabbed and removed from the scene: " + obj.name);
        isRetracting = true;
    }

    void UpdateCrosshair()
    {
        // Optional: Change crosshair appearance when extending or retracting
        if (crosshair != null)
        {
            crosshair.color = isRetracting ? Color.red : Color.white; // Change to red when retracting
        }
    }
}
