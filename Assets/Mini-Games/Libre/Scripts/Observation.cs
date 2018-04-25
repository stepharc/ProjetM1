using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Observation : MonoBehaviour
{
    private List<Joycon> joycons;
    private Joycon j;
    public int jc_ind = 0;

    void Start()
    {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            j = joycons[jc_ind];
        }
    }

    void Update()
    {
        if (joycons.Count > 0)
        {
                // Rotation grâce au gyroscope.
                gameObject.transform.rotation = j.GetVector();
        }
    }
}
