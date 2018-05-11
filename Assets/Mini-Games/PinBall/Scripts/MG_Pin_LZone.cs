using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_LZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.CompareTo("Bille") == 0)
        {
            other.gameObject.GetComponent<MG_Pin_Bille>().setBackToSpawn();
        }    
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
