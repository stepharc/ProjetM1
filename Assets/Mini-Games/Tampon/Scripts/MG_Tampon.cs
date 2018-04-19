using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Tampon : MonoBehaviour {
    private List<Joycon> joycons;
    private Joycon j;
    public Vector3 gyro;
    public int jc_ind = 0;

    IEnumerator jumpAnim(float o)
    {
        Debug.Log(o);
        float waitTime = .4F;
        //Tampon se lève
        gameObject.transform.localPosition += new Vector3(0, 0.5f, 0);
        yield return new WaitForSeconds(waitTime);
        //On fait avancer le tampon
        if (o >= 0)
        {
            gameObject.transform.localPosition += new Vector3(o, 0, 1f);
        }
        else
        {
            gameObject.transform.localPosition -= new Vector3(o, 0, 1f);
        }
        yield return new WaitForSeconds(waitTime);
        //On le repose
        gameObject.transform.localPosition -= new Vector3(0, 0.5f, 0);
    }

    // Use this for initialization
    void Start()
    {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            j = joycons[jc_ind];
            gyro = j.GetGyro();
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            Debug.Log("Pas de joycons détectés.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Modifie l'orientation du tampon (Axe Y) selon l'axe Z du gyroscope
        float orientation = j.GetGyro().z;
        gameObject.transform.Rotate(new Vector3(0, orientation, 0));
        //Si le bouton "Flèche vers le haut" est pressé, on fait bouger le tampon.
        if ((j.GetButtonDown(Joycon.Button.DPAD_UP)))
        {
            Debug.Log("JUMP !");
            //Pour que l'animation de mouvement ne se fasse pas instantanément, on utilise
            //une fonction de co-routine jumpAnim et on l'appelle avec StartCoroutine.
            orientation = gameObject.transform.localRotation.y;
            StartCoroutine(jumpAnim(orientation));
        }
    }
}
