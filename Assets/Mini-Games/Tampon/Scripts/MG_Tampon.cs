using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Tampon : MonoBehaviour {
    private List<Joycon> joycons;
    private Joycon j;
    private Vector3 gyro;
    private MG_Tampon_Surface sInstance;
    private bool jumpMode;

    private int jc_ind = 0;

    IEnumerator jumpAnim()
    {
        float waitTime = .4F;
        //Tampon se lève
        gameObject.transform.localPosition += new Vector3(0, 0.5f, 0);
        yield return new WaitForSeconds(waitTime);
        //On fait avancer le tampon
        gameObject.transform.Translate(Vector3.forward);
        yield return new WaitForSeconds(waitTime);
        //On le repose
        gameObject.transform.localPosition -= new Vector3(0, 0.5f, 0);
        yield return new WaitForSeconds(.2F);
        //On n'oublie pas d'autoriser à nouveau le mouvement du tampon vu que celle en cours est terminée.
        jumpMode = false;
    }

    // Use this for initialization
    void Start()
    {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            j = joycons[jc_ind];
            gyro = j.GetGyro();
            jumpMode = false;
            //Permet la communication entre le tampon et la surface de jeu.
            sInstance = GetComponent<MG_Tampon_Surface>();
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
        //Si le bouton "Flèche vers le haut" est pressé et que le tampon n'est pas en plein animation, on fait bouger le tampon.
        if ((j.GetButtonDown(Joycon.Button.DPAD_UP)) && !jumpMode)
        {
            jumpMode = true;
            Debug.Log("JUMP !");
            //Pour que l'animation de mouvement ne se fasse pas instantanément, on utilise
            //une fonction de co-routine jumpAnim et on l'appelle avec StartCoroutine.
            StartCoroutine(jumpAnim());
        }
    }
}
