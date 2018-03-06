using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 100), ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
