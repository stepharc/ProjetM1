using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_PBall_Camera : MonoBehaviour {
    //Distance entre la surface du Punching Ball et la caméra.
    private float offset;
    private MG_PBall_PBall bInstance;
    private MG_PBall_Player pInstance;
    private MG_PBall_Text cInstance;

    //Copie le mouvement circulaire du joueur
    private void rotateCameraObject()
    {
        float radius = (bInstance.transform.lossyScale.x / 2) + offset;
        gameObject.transform.position = bInstance.GetComponent<Collider>().bounds.center;
        float tc = pInstance.getTimeCounter();
        float newX = gameObject.transform.position.x + (Mathf.Cos(tc) * radius);
        float newZ = gameObject.transform.position.z + (Mathf.Sin(tc) * radius);
        gameObject.transform.position = new Vector3(newX, gameObject.transform.position.y, newZ);
        //On pointe la caméra vers l'objet joueur.
        gameObject.transform.LookAt(pInstance.transform);
    }

    // Use this for initialization
    void Start () {
        //On réalise la communication entre la caméra et le composant père, l'objet représentant le joueur.
        pInstance = gameObject.transform.parent.GetComponent<MG_PBall_Player>();
        bInstance = GameObject.Find("Punching Ball").GetComponent<MG_PBall_PBall>();
        cInstance = gameObject.transform.GetChild(0).GetComponent<MG_PBall_Text>();
        offset = bInstance.transform.lossyScale.y * 3;
        rotateCameraObject();
    }
	
	// Update is called once per frame
	void Update () {
        string cm = pInstance.getCurrentMode();
        string f = pInstance.getImpactForce();
        if(cm == "PUNCHING")
        {
            cInstance.setForceTextVisibility(true);
        }
        else
        {
            //Si on est toujours en mode orientation, on continue de faire tourner la caméra.
            if (cm == "ORIENTATION")
            {
                rotateCameraObject();
            }
            cInstance.setForceTextVisibility(false);
        }
        cInstance.setModeText("MODE : " + cm);
        cInstance.setForceText("FORCE : " + f);
    }
}
