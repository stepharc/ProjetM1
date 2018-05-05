using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityJoint : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().useGravity && other.gameObject.GetComponent<SpringJoint>() == null)
        {
            SpringJoint joint = other.gameObject.AddComponent<SpringJoint>();
            joint.connectedBody = GetComponent<Rigidbody>();
            joint.breakForce = 100;
        }
    }
}
