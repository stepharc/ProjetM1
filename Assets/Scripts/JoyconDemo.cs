using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconDemo : MonoBehaviour
{

    private List<Joycon> joycons;

    // Values made available via Unity
    public float[] stick;
    public float posX;
    public float posY;
    public float posZ;
    public GameObject cursor1;
    public GameObject cursor2;
    public GameObject cursor3;
    public GameObject cursor4;
    [Range(1f, 100f)]
    public float vitesse;
    public float angle_mort = 0.05f;
    public Vector3 gyro;
    public Vector3 accel;
    [Range(0, 3)]
    public Vector3 accel1;
    public Vector3 accel2;
    public Vector3 accel3;
    public int jc_ind = 0;
    public Quaternion orientation;

    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;

        posX = 0;
        posY = 0;
        posZ = 0;
        cursor1 = (GameObject)Instantiate(cursor1, Vector3.zero, orientation);
        cursor1.GetComponent<Renderer>().material.color = Color.red;
        cursor2 = (GameObject)Instantiate(cursor2, Vector3.zero, orientation);
        cursor2.GetComponent<Renderer>().material.color = Color.green;
        cursor3 = (GameObject)Instantiate(cursor3, Vector3.zero, orientation);
        cursor3.GetComponent<Renderer>().material.color = Color.blue;
        cursor4 = (GameObject)Instantiate(cursor4, Vector3.zero, orientation);
        cursor4.GetComponent<Renderer>().material.color = Color.grey;
        if (joycons.Count < jc_ind + 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // make sure the Joycon only gets checked if attached
        if (joycons.Count > 0)
        {
            Joycon j = joycons[jc_ind];
            // GetButtonDown checks if a button has been pressed (not held)
            if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 pressed");
                // GetStick returns a 2-element vector with x/y joystick components
                Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}", j.GetStick()[0], j.GetStick()[1]));

                // Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
                j.Recenter();
                gameObject.transform.position = Vector3.zero;
            }
            // GetButtonDown checks if a button has been released
            if (j.GetButtonUp(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 released");
            }
            // GetButtonDown checks if a button is currently down (pressed or held)
            if (j.GetButton(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 held");
            }

            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                Debug.Log("Rumble");

                // Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
                // https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

                j.SetRumble(160, 320, 0.6f, 200);

                // The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
                // (Useful for dynamically changing rumble values.)
                // Then call SetRumble(0,0,0) when you want to turn it off.
            }

            if (j.GetButton(Joycon.Button.DPAD_UP))
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            }

            stick = j.GetStick();

            // Gyro values: x, y, z axis values (in radians per second)
            gyro = j.GetGyro();

            // Accel values:  x, y, z axis values (in Gs)
            accel = j.GetAccel();

            accel1 = j.GetAccel1();
            accel2 = j.GetAccel2();
            accel3 = j.GetAccel3();

            bool isMoving = false;
            bool debug = false;

            if (gyro.x > angle_mort || gyro.x < -angle_mort || debug)
            {
                isMoving = true;
                orientation.x = j.GetVector().x;
            }
            if (gyro.y > angle_mort || gyro.y < -angle_mort || debug)
            {
                isMoving = true;
                orientation.y = j.GetVector().y;
            }
            if (gyro.z > angle_mort || gyro.z < -angle_mort || debug)
            {
                isMoving = true;
                orientation.z = j.GetVector().z;
            }



            if (isMoving)
            {
                orientation.w = j.GetVector().w;
                gameObject.transform.rotation = orientation;
            }
            
                gameObject.transform.position = new Vector3(accel3.x*vitesse, accel1.y * vitesse, accel3.z * vitesse);

            cursor1.transform.position = accel1;
            cursor2.transform.position = accel2;
            cursor3.transform.position = accel3;
            cursor4.transform.position = accel1 + accel2 + accel3;
        }
    }
}