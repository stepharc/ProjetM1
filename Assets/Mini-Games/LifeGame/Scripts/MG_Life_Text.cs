using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MG_Life_Text : MonoBehaviour {

    public void setText(string s)
    {
        if(GetComponent<Text>().text.CompareTo(s) != 0)
        {
            GetComponent<Text>().text = s;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
