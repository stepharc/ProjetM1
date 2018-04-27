using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Tampon : MonoBehaviour {
    private List<Joycon> joycons;
    private Joycon j;
    private int jc_ind = 0;
    private MG_Tampon_Surface sInstance;
    private bool jumpMode;
    private Color color;

    IEnumerator jumpAnim()
    {
        float waitTime = .3F, jumpHeight = 0.5f;
        //Tampon se lève
        gameObject.transform.localPosition += new Vector3(0, jumpHeight, 0);
        yield return new WaitForSeconds(waitTime);
        //On fait avancer le tampon
        gameObject.transform.Translate(Vector3.forward);
        sInstance.correctTamponPosition(gameObject.transform.position, gameObject.transform);
        yield return new WaitForSeconds(waitTime);
        //On le repose
        gameObject.transform.localPosition -= new Vector3(0, jumpHeight, 0);
        yield return new WaitForSeconds(.2F);
        //On n'oublie pas d'autoriser à nouveau le mouvement du tampon vu que celle en cours vient de se terminer.
        jumpMode = false;
    }

    //Choisit une couleur au hasard et l'applique au tampon.
    private void setRandomizedColor()
    {
        float r, g, b;
        r = Random.Range(0.0f, 1.0f);
        g = Random.Range(0.0f, 1.0f);
        b = Random.Range(0.0f, 1.0f);
        color = new Color(r, g, b, 1.0f);
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    // Use this for initialization
    void Start()
    {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            j = joycons[jc_ind];
            jumpMode = false;
            //Permet la communication entre le tampon et la surface de jeu.
            sInstance = GameObject.Find("Surface").GetComponent<MG_Tampon_Surface>();
            setRandomizedColor();
            //Envoi des données importantes à la surface de jeu (Orientation, dimensions, couleur, nom de l'objet)
            sInstance.setTamponRotation(gameObject.transform.rotation.eulerAngles.y);
            sInstance.setTamponScales(gameObject.transform.localScale.x, gameObject.transform.localScale.z);
            sInstance.setTamponName(gameObject.name);
            sInstance.setTrackColor(color);
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
        if (!jumpMode)
        {
            gameObject.transform.Rotate(new Vector3(0, orientation, 0));
        }
        //Si le bouton "Flèche vers le haut" est pressé et que le tampon n'est pas en plein animation, on fait bouger le tampon.
        if ((j.GetButtonDown(Joycon.Button.DPAD_UP)) && !jumpMode)
        {
            jumpMode = true;
            Debug.Log("JUMP !");
            sInstance.setTamponRotation(gameObject.transform.rotation.eulerAngles.y);
            //Pour que l'animation de mouvement ne se fasse pas instantanément, on utilise
            //une fonction de co-routine jumpAnim et on l'appelle avec StartCoroutine.
            StartCoroutine(jumpAnim());
        }
    }
}
