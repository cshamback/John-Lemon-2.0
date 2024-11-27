using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] Vector3 jumpVector = new Vector3(0, 5, 10); //The vector that will be used to apply a force to the rigidbody.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 force = transform.forward * jumpVector.z + transform.up * jumpVector.y;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("There is something inside");
    }

}
