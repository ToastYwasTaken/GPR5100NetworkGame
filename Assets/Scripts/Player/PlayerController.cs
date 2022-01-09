using Photon.Pun;
using UnityEngine;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : PlayerController.cs
* Date   : 17.12.2021
* Author : René Kraus (RK)
* Version: 1.1
*
* These coded instructions, statements, and computer programs contain
* proprietary information of the author and are protected by Federal
* copyright law. They may not be disclosed to third parties or copied
* or duplicated in any form, in whole or in part, without the prior
* written consent of the author.
*
* History:
*	17.12.21	RK	Created
*	05.01.22    RK  Added Gravtiy Multiplier
******************************************************************************/
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private Transform m_PlayerCamera;
    [SerializeField] private Collider playerCollider;

    [Header("Move Properties")]
    [SerializeField] private bool m_UseAxisRaw = false;
    [SerializeField] private float m_Speed = 5f;
    [SerializeField] private bool m_CanRun = true;
    [SerializeField] private float m_RunSpeed = 10f;
    [SerializeField] private bool m_IsRunning = false;

    [Header("Run Endurance Properties")]
    [SerializeField] private bool m_UseEndurance = true;
    [SerializeField] private float m_Endurance = 3f;
    private float currentEndurance = 3f;
    [SerializeField] private float m_RefreshEnduranceOffset = 1f;
    [SerializeField] private float m_ReduceEnduranceOffset = 1f;

    [Header("Jump Properties")]
    [SerializeField] private bool m_CanJump = true;
    [SerializeField] private float m_JumpForce = 20f;
    [SerializeField] private ForceMode m_ForceMode = ForceMode.Force;
    [SerializeField] private float m_JumpingControlMultiplier = 1f;
    [SerializeField] private float m_GravityMultiplier = 1f;
    [SerializeField] private bool m_OnAir = false;

    [Header("Mouse Properties")]
    [SerializeField] private bool m_UseMouseAxisRaw = false;
    [SerializeField] private float m_RotationSpeed = 100f;
    [SerializeField, Range(0.0f, 2.0f)] private float m_Sensitivity = 1.0f;
    [SerializeField, Range(0f, 360f)] private float m_MaxVerticalAngle = 60f;
    private float currentAngle = 0f;

    [Header("Ground Properties")]
    [SerializeField] private bool m_IsGrounded = true;
    [SerializeField] private float m_GroundDistance = 1f;
    [SerializeField] private LayerMask m_GroundLayer = 0;
    [SerializeField] private QueryTriggerInteraction m_GroundInteraction = QueryTriggerInteraction.UseGlobal;
    [SerializeField] private float m_FallDownLimit = -5f;

    private Rigidbody playerRig;
    private Vector3 startPosition = Vector3.zero;
    private bool runKeyPressed = false;

    private void Awake()
    {
        playerRig = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        startPosition = playerRig.transform.position;
        currentEndurance = m_Endurance;

        m_PlayerCamera.GetComponentInChildren<Camera>().gameObject.SetActive(photonView.IsMine);
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (GameManager.instance.IsPause) return;

        // Player springt
        Jump(Controls.Jump());
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        if (GameManager.instance.IsPause) return;

        // Werte für das Gehen/Rennen
        float horizontal = Controls.Horizontal(m_UseAxisRaw);
        float vertical = Controls.Vertical(m_UseAxisRaw);

        // Mouse Steuerung
        float mouseX = Controls.MouseX(m_UseMouseAxisRaw);
        float mouseY = Controls.MouseY(m_UseMouseAxisRaw);

        // Taste für das Sprinten gedrückt
        runKeyPressed = Controls.Sprint();

       

        // Entscheidet zwischen Gehen und Rennen
        if (runKeyPressed && (currentEndurance > 0) && m_CanRun && m_IsGrounded)
        {
            // Rennen
            m_IsRunning = true;
            Move(horizontal, vertical, m_RunSpeed);
        }
        else
        {
            // Gehen
            m_IsRunning = false;
            Move(horizontal, vertical, m_Speed);
        }

        // Player zur Maus ausrichten
        Rotate(mouseX, mouseY);

        // Ausdauer Berücksichtigen
        if (m_UseEndurance)
        {
            // Ausdauer abziehen oder erneuern
            EnduranceBehaviour(!runKeyPressed);
        }

        // Befindet sich der Player auf den Boden
        GroundCheck();

        // Ist der Player aus dem Level gefallen 
        FallCheck();
    }

    /// <summary>
    /// Bewegt den Spieler
    /// </summary>
    /// <param name="_horizontal"></param>
    /// <param name="_vertical"></param>
    private void Move(float _horizontal, float _vertical, float _speed)
    {
        Vector3 playerMovement = new Vector3(_horizontal, 0f, _vertical);

        if (m_OnAir)
        {
            playerMovement = OnAir(playerMovement);
        }
        else
        {
            //playerRig.velocity = Vector3.zero; // <-- Blockiert springen, verbessert aber die Bewegungsabläufe Online
        }

        playerRig.MovePosition(playerRig.position + playerMovement.z *
            transform.forward * _speed * Time.fixedDeltaTime);

        playerRig.MovePosition(playerRig.position + playerMovement.x *
           transform.right * _speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Maus Steuerung
    /// </summary>
    void Rotate(float _mouseX, float _mouseY)
    {
        Vector3 mouseInput = new Vector3(_mouseX, _mouseY, 0f);

        
        // Verhindert ungewollte Bewegung
        playerRig.angularVelocity = Vector3.zero;

        // Spieler sieht nach oben oder unten
        currentAngle += -mouseInput.y * m_RotationSpeed * m_Sensitivity * Time.deltaTime;

        // Winkel der Kamera auf min und max begrenzen
        currentAngle = Mathf.Clamp(currentAngle,
            -m_MaxVerticalAngle,
            m_MaxVerticalAngle);

        // Rotationswinkel zuweisen
        m_PlayerCamera.localEulerAngles =
            new Vector3(currentAngle, 0, 0);

        // Spieler dreht sich nach links oder rechts
        Quaternion deltaRotationX = Quaternion.Euler(0, m_RotationSpeed * m_Sensitivity *
            Time.fixedDeltaTime * mouseInput.x, 0);

       playerRig.MoveRotation(playerRig.rotation * deltaRotationX);
    }

    /// <summary>
    /// Ausdauer Verhalten für das Rennen
    /// </summary>
    /// <param name="_refresh"></param>
    private void EnduranceBehaviour(bool _refresh = true)
    {
        if (m_IsRunning)
        {
            currentEndurance -= Time.deltaTime * m_ReduceEnduranceOffset;
        }
        else
        {
            if (_refresh)
            {
                if (currentEndurance > m_Endurance) return;

                currentEndurance += Time.deltaTime * m_RefreshEnduranceOffset;
            }
        }
    }

    /// <summary>
    /// Spieler springt
    /// </summary>
    /// <param name="_inputKey"></param>
    private void Jump(bool _inputKey)
    {
        if (_inputKey && m_CanJump && !m_OnAir && m_IsGrounded)
        {
            playerRig.AddForce(transform.up * m_JumpForce, m_ForceMode);
        }
    }

    /// <summary>
    /// Verhalten im Sprung
    /// </summary>
    /// <param name="_movement"></param>
    /// <returns></returns>
    private Vector3 OnAir(Vector3 _movement)
    {
        Vector3 airMovement = _movement;

        airMovement += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        airMovement.x = (_movement.x * m_Speed) * m_JumpingControlMultiplier;
        airMovement.y = (_movement.y * m_Speed) * m_JumpingControlMultiplier;

        return airMovement;
    }

    /// <summary>
    /// Prüft ob, der Player sich am Boden befindet
    /// </summary>
    private void GroundCheck()
    {
        float distance = m_GroundDistance + playerCollider.transform.localScale.y;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),
           distance, m_GroundLayer, m_GroundInteraction))
        {
            m_IsGrounded = true;
            m_OnAir = false;

        }
        else
        {
            m_IsGrounded = false;
            m_OnAir = true;
        }
    }

    /// <summary>
    /// Prüft ob, der Player sich unter angebenen Limit befindet und setzt diesen zurück
    /// </summary>
    private void FallCheck()
    {
        if (transform.position.y <= m_FallDownLimit)
        {
            transform.position = startPosition;
        }
    }
}
