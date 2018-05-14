using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_LRArm : MonoBehaviour {
    private float baseYRotation, boundX, lastContactX, pushForce, basePushForce;
    private Vector3 lastVelocity;

    public float getBaseRotation()
    {
        return baseYRotation;
    }

    private void OnCollisionStay(Collision collision)
    {
        //On garde en mémoire le dernier point de contact et la dernière vélocité de la bille.
        if(collision.gameObject.name.CompareTo("Bille") == 0)
        {
            lastContactX = collision.contacts[0].point.x;
            lastVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.normalized;
        }
    }

    //La bille ne touche plus le bras, la bille est donc projetée !
    private void OnCollisionExit(Collision collision)
    {
        bool ok = true;
        if (collision.gameObject.name.CompareTo("Bille") == 0)
        {
            Rigidbody r = collision.gameObject.GetComponent<Rigidbody>();
            float distance;
            //On calcule la distance entre le dernier point de contact et l'extremité du bras
            if (boundX > lastContactX)
            {
                distance = boundX - lastContactX;
            }
            else
            {
                distance = lastContactX - boundX;
            }
            if(distance > 1)
            {
                //Plus la bille est loin de l'extremité du bras, plus la puissance de projection diminue.
                pushForce = pushForce / distance;
            }
            else
            {
                //Empêche la projection de la bille si cette dernière ne touche plus le bras non pas à cause du
                //joueur mais à cause de la gravité.
                if(distance < 0.05)
                {
                    ok = false;
                }
            }
            if (ok)
            {
                Vector3 forceVec = lastVelocity * pushForce;
                r.AddForce(forceVec, ForceMode.Acceleration);
                if (pushForce != basePushForce) pushForce = basePushForce;
            }
        }
    }

    // Use this for initialization
    void Start () {
        MG_Pin_Arms father = gameObject.transform.parent.GetComponent<MG_Pin_Arms>();
        baseYRotation = gameObject.transform.rotation.eulerAngles.y;
        pushForce = basePushForce = 1000f;
		if(gameObject.name.CompareTo("LeftArm") == 0)
        {
            father.setLeftArm(gameObject.transform);
            boundX = gameObject.GetComponent<Renderer>().bounds.max.x;
        }
        else
        {
            if (gameObject.name.CompareTo("RightArm") == 0)
            {
                father.setRightArm(gameObject.transform);
                boundX = gameObject.GetComponent<Renderer>().bounds.min.x;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
