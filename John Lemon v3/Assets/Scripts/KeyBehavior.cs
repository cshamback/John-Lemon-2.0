using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    public Transform targetPos;
    public Transform prevPos;
    public GameObject Light;

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
            Light.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCameraTransform.position = prevPos.position;
            mainCameraTransform.rotation = prevPos.rotation;
            Light.gameObject.SetActive(false);
        }
    }
}
