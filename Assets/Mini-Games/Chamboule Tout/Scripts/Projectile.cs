using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private Vector3 direction = new Vector3(0, 0.2f, 1);
    public float force = 500; // La force de mouvement.
    // Use this for initialization
    void Start ()
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(direction*force*rigidbody.mass);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Sol" || other.gameObject.tag == "Support")
        {
            Destroy(gameObject);
        }
    }
}
