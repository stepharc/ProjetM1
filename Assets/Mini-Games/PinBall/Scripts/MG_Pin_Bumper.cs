using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_Bumper : MonoBehaviour {
    private Color baseColor;
    private float pushStrength;

    IEnumerator changeColor()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<Renderer>().material.color = baseColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.CompareTo("Bille") == 0)
        {
            Rigidbody r = collision.gameObject.GetComponent<Rigidbody>();
            StartCoroutine("changeColor");
            //Repousse la bille en arrière avec une certaine force.
            Vector3 forceVec = -r.velocity.normalized * pushStrength;
            //Ajouter ce mode permet d'ignorer les masses des deux objets en collision.
            r.AddForce(forceVec, ForceMode.Acceleration);
        }
    }

    // Use this for initialization
    void Start () {
        baseColor = gameObject.GetComponent<Renderer>().material.color;
        pushStrength = 500f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
