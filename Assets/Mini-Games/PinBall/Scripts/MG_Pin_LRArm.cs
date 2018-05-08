using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_LRArm : MonoBehaviour {
    private float baseYRotation;

    public float getBaseRotation()
    {
        return baseYRotation;
    }

	// Use this for initialization
	void Start () {
        MG_Pin_Arms father = gameObject.transform.parent.GetComponent<MG_Pin_Arms>();
        baseYRotation = gameObject.transform.rotation.eulerAngles.y;
		if(gameObject.name.CompareTo("LeftArm") == 0)
        {
            father.setLeftArm(gameObject.transform);
        }
        else
        {
            if (gameObject.name.CompareTo("RightArm") == 0)
            {
                father.setRightArm(gameObject.transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
