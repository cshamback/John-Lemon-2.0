using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationLock : MonoBehaviour
{
    public int correctCombo1 = 5;
    public int correctCombo2 = 4;
    public int correctCombo3 = 9;
    public GameObject lockPuzzle;
    public GameObject doorOpened;
    public Transform targetPos;

    public CombinationWheel wheel1;
    public CombinationWheel wheel2;
    public CombinationWheel wheel3;

    private Transform mainCameraTransform;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    public void CheckCombination()
    {
        if (wheel1.wheelValue == correctCombo1 &&
            wheel2.wheelValue == correctCombo2 &&
            wheel3.wheelValue == correctCombo3)
        {
            Debug.Log("Lock opened!");
            lockPuzzle.SetActive(false);
            doorOpened.SetActive(false);
            mainCameraTransform.position = targetPos.position;
            mainCameraTransform.rotation = targetPos.rotation;
            wheel1.controls.LockControls.Disable();
            wheel2.controls.LockControls.Disable();
            wheel3.controls.LockControls.Disable();
            Debug.Log("Lock controls enabled");
        }
    }
}
