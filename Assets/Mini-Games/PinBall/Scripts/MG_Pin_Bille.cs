using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_Bille : MonoBehaviour {
    private Vector3 spawnPos;

    public void setBackToSpawn()
    {
        gameObject.transform.position = spawnPos;
    }

	// Use this for initialization
	void Start () {
        spawnPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
