using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombinationWheel : MonoBehaviour
{
    public int wheelValue = 0;
    public float anglePerStep = 36f;
    public CombinationLock lockScript;
    public Controller controls;
    
    private Camera mainCamera;

    // Start is called before the first frame update
    private void Start()
    {
        UpdateRotation();
        Disable();
    }

    void Awake()
    {
        controls = new Controller();

        mainCamera = Camera.main;

        controls.LockControls.Click.performed += _ => OnClick();
    }

    public void Enable()
    {
        controls.LockControls.Enable();
        Debug.Log("Controls for lock enabled");
    }

    public void Disable()
    {
        controls.LockControls.Disable();
        Debug.Log("Controls for lock disabled");
    }

    private void OnClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Raycast Hit: " + hit.transform.name);

            if (hit.transform == transform)
            {
                RotateWheel();
                lockScript.CheckCombination();
            }
        }
    }

    private void RotateWheel()
    {
        wheelValue = (wheelValue + 1) % 10;

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        float targetAngle = wheelValue * anglePerStep;
        transform.localEulerAngles = new Vector3(targetAngle, 0, 0);
    }
}
