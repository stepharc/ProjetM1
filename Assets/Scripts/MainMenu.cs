using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    private List<Joycon> joycons;
    public float[] stick;
    public Vector3 gyro;
    public int jc_ind = 0;
    public Quaternion orientation;

    // Use this for initialization
    void Start () {
        gyro = new Vector3(0, 0, 0);
        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jc_ind + 1)
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (joycons.Count > 0)
        {
            Joycon j = joycons[jc_ind];
            gyro = j.GetGyro();
            gameObject.transform.position.Set(gyro.x, gyro.y, 0);
        }
	}
}
