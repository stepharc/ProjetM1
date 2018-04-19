using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_MGSphere : MonoBehaviour
{
    private List<Joycon> joycons;
    private Joycon j;
    private float SphereRadius;
    private float BaseRadius;
    public Vector3 gyro;
    public int jc_ind = 0;
    public bool initpump;
    public float yinit;
    public float radius;

    // Use this for initialization
    void Start()
    {
        SphereRadius = -1;
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            j = joycons[jc_ind];
            gyro = j.GetGyro();
            yinit = gyro.y;
            initpump = false;
            BaseRadius = gameObject.transform.localScale.y;
        }
        else
        {
            Debug.Log("Pas de joycons détectés.");
            yinit = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gyro = j.GetGyro();
        if ((!initpump) && gyro.y >= yinit + 5)
        {
            Debug.Log("Action de pompage initialisé.");
            initpump = true;
        }
        if (initpump && gyro.y <= yinit - 5)
        {
            Debug.Log("De l'air est envoyé !");
            //initpump = false;
            gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            SphereRadius = gameObject.transform.localScale.y;
            radius = SphereRadius;
        }
        else if (SphereRadius > BaseRadius)
        {
            gameObject.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            SphereRadius = gameObject.transform.localScale.y;
            radius = SphereRadius;
        }
        if (SphereRadius != -1)
        {
            if (SphereRadius >= 5)
            {
                Debug.Log("La sphère éclate !");
                j.SetRumble(160f, 320f, 0.6f, 550);
                Destroy(gameObject);
                Application.Quit();
            }
        }
    }
}
