using System;
using UnityEngine;
using System.Collections.Generic;

namespace Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private List<Joycon> joycons;
        private Joycon j;
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        public int jc_ind = 0;
        private void Start()
        {
            joycons = JoyconManager.Instance.j;
            if (joycons.Count > 0)
            {
                j = joycons[jc_ind];
            }
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                if (joycons.Count > 0)
                {
                    if (j.isLeft)
                        m_Jump = j.GetButton(Joycon.Button.DPAD_RIGHT);
                    else
                        m_Jump = j.GetButton(Joycon.Button.DPAD_LEFT);
                }
                else
                {
                    m_Jump = Input.GetButtonDown("Jump");
                }
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            float h;
            float v;
            bool crouch;
            bool ramasser, lacher;

            // read inputs
            if (joycons.Count > 0)
            {
                if (j.isLeft)
                {
                    h = -j.GetStick()[1];
                    v = j.GetStick()[0];
                    crouch = j.GetButton(Joycon.Button.DPAD_LEFT);
                    ramasser = j.GetButton(Joycon.Button.DPAD_DOWN);
                    lacher = j.GetButton(Joycon.Button.DPAD_LEFT);
                }
                else
                {
                    h = j.GetStick()[1];
                    v = -j.GetStick()[0];
                    crouch = j.GetButton(Joycon.Button.DPAD_RIGHT);
                    ramasser = j.GetButton(Joycon.Button.DPAD_UP);
                    lacher = j.GetButton(Joycon.Button.DPAD_RIGHT);
                }
            }
            else
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
                crouch = Input.GetButton("Fire1");
                ramasser = Input.GetButton("Fire2");
                lacher = Input.GetButton("Fire1");
            }
            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
            if (ramasser)
            {
                GetComponent<Ramasser>().Prendre();
            }
            if (lacher)
            {
                GetComponent<Ramasser>().Lacher();
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
