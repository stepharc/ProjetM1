using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthControl : MonoBehaviour
{
    private enum ModeControles : int
    {
        DETECTION = 0, // Utilisation du gyroscope
        ANALOGIQUE = 1 // Utilisation du stick
    };
    private List<Joycon> joycons;


    private float[] stick;
    private Joycon j;

    [Range(0, 1)]
    private int mode;

    [Range(1f, 30f)]
    public float vitesse;
    public Vector3 accel;
    public int jc_ind = 0;

    /* App */
    void Start()
    {
        accel = new Vector3(0, 0, 0);
        joycons = JoyconManager.Instance.j;
        if (joycons.Count <= 0) // Si pas de joycons.
        {
            mode = (int)ModeControles.ANALOGIQUE;
        }
        else
        {
            mode = (int)ModeControles.DETECTION;
            j = joycons[jc_ind];
        }
    }

    // Update is called once per frame
    void Update()
    {
        // make sure the Joycon only gets checked if attached
        if (joycons.Count > 0)
        {
            mouvementJoycon();
        }
        else
        {
            mouvementAutre();
        }
    }

    private void mouvementJoycon()
    {
        // Pour changer de mode de controle.
        if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
        {
            mode = (int)ModeControles.DETECTION;
        }

        if (j.GetButtonDown(Joycon.Button.DPAD_UP))
        {
            mode = (int)ModeControles.ANALOGIQUE;
        }

        if (mode == (int)ModeControles.DETECTION)
        {
            accel = j.GetAccel();

            //Arrondissement à la décimale inférieure (pour limiter les tremblements)
            float x = (float)((int)(accel.x * 100)) / 100;
            float y = (float)((int)(accel.y * 100)) / 100;

            gameObject.transform.rotation = Quaternion.Euler(y * vitesse, 180, x * vitesse);
        }
        else if (mode == (int)ModeControles.ANALOGIQUE)
        {
            stick = j.GetStick();
            gameObject.transform.rotation = Quaternion.Euler(-stick[0] * vitesse, 180, -stick[1] * vitesse);
        }
    }

    private void mouvementAutre()
    {
        gameObject.transform.rotation = Quaternion.Euler(-Input.GetAxis("Vertical") * vitesse, 180, Input.GetAxis("Horizontal") * vitesse);
    }
}