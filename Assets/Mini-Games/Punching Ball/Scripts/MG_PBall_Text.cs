using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MG_PBall_Text : MonoBehaviour {
    private MG_PBall_Camera cameraToLookAt;
    private float distance;
    private Text tm, tf;

    public void setModeText(string s)
    {
        tm.text = s;
    }

    public void setForceText(string s)
    {
        tf.text = s;
    }

    public void setForceTextVisibility(bool b)
    {
        tf.enabled = b;
    }

    void Start()
    {
        distance = 10f;
        cameraToLookAt = gameObject.transform.parent.GetComponent<MG_PBall_Camera>();
        tm = gameObject.transform.GetChild(0).GetComponent<Text>();
        tf = gameObject.transform.GetChild(1).GetComponent<Text>();
    }

    void Update()
    {
        transform.position = cameraToLookAt.transform.position + (cameraToLookAt.transform.forward * distance);
    }
}
