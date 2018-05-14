using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    private List<Joycon> joycons;
    private Joycon j;
    private float[] controls = { 0, 0 }; // Coordonnées des axes de controles.
    public Projectile projectile;
    public float beamRate = 1; // Le temps d'attente entre deux tirs.
    private float delay; // Le temps écoulé entre deux tirs.

    public float[] stick;
    [Range(0, 1)]
    public float vitesse;
    public float limitX, limitY;
    public int jc_ind = 0;

    Transform firePoint;
    // Use this for initialization
    void Start()
    {
        firePoint = transform.Find("FirePoint");
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            j = joycons[jc_ind];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (joycons.Count > 0)
        {
            stick = j.GetStick();

            if (j.isLeft)
            {
                if (transform.position.x < limitX && stick[1] < 0)
                {
                    controls[0] = -stick[1] * vitesse;
                }
                else if (transform.position.x > -limitX && stick[1] > 0)
                {
                    controls[0] = -stick[1] * vitesse;
                }
                else
                {
                    controls[0] = 0;
                }
                if (transform.position.y < limitY && stick[0] > 0)
                {
                    controls[1] = stick[0] * vitesse;
                }
                else if (transform.position.y > 0 && stick[0] < 0)
                {
                    controls[1] = stick[0] * vitesse;
                }
                else
                {
                    controls[1] = 0;
                }
                if (j.GetButton(Joycon.Button.DPAD_DOWN) && delay >= beamRate)
                {
                    Instantiate(projectile, firePoint.position, firePoint.rotation);
                    delay = 0;
                }
            }
            else
            {
                if (transform.position.x < limitX && stick[1] > 0)
                {
                    controls[0] = stick[1] * vitesse;
                }
                else if (transform.position.x > -limitX && stick[1] < 0)
                {
                    controls[0] = stick[1] * vitesse;
                }
                else
                {
                    controls[0] = 0;
                }
                if (transform.position.y < limitY && stick[0] < 0)
                {
                    controls[1] = -stick[0] * vitesse;
                }
                else if (transform.position.y > 0 && stick[0] > 0)
                {
                    controls[1] = -stick[0] * vitesse;
                }
                else
                {
                    controls[1] = 0;
                }

                if (j.GetButton(Joycon.Button.DPAD_UP) && delay >= beamRate)
                {
                    Instantiate(projectile, firePoint.position, firePoint.rotation);
                    delay = 0;
                }
            }


            transform.position += new Vector3(controls[0], controls[1], 0);
        }
        else
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            if (transform.position.x < limitX && x > 0)
            {
                controls[0] = x * vitesse;
            }
            else if (transform.position.x > -limitX && x < 0)
            {
                controls[0] = x * vitesse;
            }
            else
            {
                controls[0] = 0;
            }

            if (transform.position.y < limitY && y > 0)
            {
                controls[1] = y * vitesse;
            }
            else if (transform.position.y > 0 && y < 0)
            {
                controls[1] = y * vitesse;
            }
            else
            {
                controls[1] = 0;
            }

            if (Input.GetButton("Fire2") && delay >= beamRate)
            {
                Instantiate(projectile, firePoint.position, firePoint.rotation);
                delay = 0;
            }

            transform.position += new Vector3(controls[0], controls[1], 0);
        }
    }

    void FixedUpdate()
    {
        delay += Time.fixedDeltaTime;
    }
}
