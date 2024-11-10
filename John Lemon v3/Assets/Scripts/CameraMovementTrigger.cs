using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementTrigger : MonoBehaviour
{
    public Transform targetPos;
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
            //StopAllCoroutines();
            //StartCoroutine(MoveCameraToPosition());

            mainCameraTransform.position = targetPos.position;
            mainCameraTransform.rotation = targetPos.rotation;
        }
    }

    // This will make a smooth transition with the camera
    /*private System.Collections.IEnumerator MoveCameraToPosition()
    {
        while (Vector3.Distance(mainCameraTransform.position, targetPos.position) > 0.05f ||
            Quaternion.Angle(mainCameraTransform.rotation, targetPos.rotation) > 0.05f)
        {
            mainCameraTransform.position = Vector3.Lerp(mainCameraTransform.position, targetPos.position, Time.deltaTime * transitionSpeed);

            mainCameraTransform.rotation = Quaternion.Slerp(mainCameraTransform.rotation, targetPos.rotation, Time.deltaTime * transitionSpeed);

            yield return null;
        }

        mainCameraTransform.position = targetPos.position;
        mainCameraTransform.rotation = targetPos.rotation;
    }*/
}
