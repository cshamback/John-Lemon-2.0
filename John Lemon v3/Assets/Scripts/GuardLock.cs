using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLock : MonoBehaviour
{
    public Transform targetPos;
    public Transform prevPos;
    public CombinationWheel Wheel1;
    public CombinationWheel Wheel2;
    public CombinationWheel Wheel3;
    public float transitionSpeed = 2.0f;

    private Transform mainCameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCameraTransform.position = targetPos.position;
            mainCameraTransform.rotation = targetPos.rotation;
            Wheel1.controls.LockControls.Enable();
            Wheel2.controls.LockControls.Enable();
            Wheel3.controls.LockControls.Enable();
            Debug.Log("Lock controls enabled");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCameraTransform.position = prevPos.position;
            mainCameraTransform.rotation = prevPos.rotation;
            Wheel1.controls.LockControls.Disable();
            Wheel2.controls.LockControls.Disable();
            Wheel3.controls.LockControls.Disable();
            Debug.Log("Lock controls enabled");
        }
    }
}
