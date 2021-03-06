﻿using UnityEngine;
using UnityEngine.UI;

namespace La.Cantina.UI
{
    public class StartPlayerController : MonoBehaviour
    {

        public Color m_PlayerColor;
        public Color m_DefaultColor;

        private int m_PlayerID;
        private int m_JoystickNumber;
        private bool m_IsInitialized = false;
        private bool m_IsReady = false;

        public AudioClip m_ConfirmFeedbackSFX;
        public AudioClip m_CancelFeedbackSFX;

        private AudioSource _audioSource;

        private StartController m_StartController;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (m_IsInitialized == true)
            {
                if (IsPressedAction(1) && m_IsReady == false)
                {
                    m_IsReady = true;
                    GetComponentInChildren<Image>().color = m_PlayerColor;

                    m_StartController.CallBackPlayerReady(m_PlayerID);

                    // Play confirm feedback SFX
                    if(_audioSource.isPlaying == false)
                    {
                        _audioSource.clip = m_ConfirmFeedbackSFX;
                        _audioSource.Play();
                    }
                }

                if (IsPressedAction(2) && m_IsReady == true)
                {
                    m_IsReady = false;
                    GetComponentInChildren<Image>().color = m_DefaultColor;

                    m_StartController.CallBackPlayerUnready(m_PlayerID);

                    // Play cancel feedback SFX
                    if (_audioSource.isPlaying == false)
                    {
                        _audioSource.clip = m_CancelFeedbackSFX;
                        _audioSource.Play();
                    }
                }
            }
        }

        public void InitPlayer(int id, StartController controller)
        {
            m_PlayerID          = id;
            m_JoystickNumber    = m_PlayerID + 1;
            m_IsInitialized     = true;
            m_StartController   = controller;
        }

        // Check les boutons appuyés (1=A, 2=B, 3=Y, 4=X)
        private bool IsPressedAction(int button)
        {
            bool isPressed = Input.GetButtonDown("Action" + button.ToString() + "_P" + m_JoystickNumber.ToString());

            return isPressed;
        }
    }
}