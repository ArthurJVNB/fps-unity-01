using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    #region Public Getters
    public float MaxWalkSpeed { get { return walkSpeed; } }
    public float MaxRunSpeed { get { return runSpeed; } }
    public float MinWalkSpeed { get { return -walkSpeed; } }
    public float MinRunSpeed { get { return -runSpeed; } }
    
    public Vector3 Speed { get; private set; }
    #endregion

    #region Inspector Variables
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] private LayerMask jumpableLayers;
    //[SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    #endregion

    #region Private Variables
    private CharacterController m_controller;
    private PlayerAnimator m_animator;
    private PlayerInput m_input;
    private Vector3 m_lastMovementVector = Vector3.zero;
    #endregion
    #endregion

    #region Initializations
    private void Awake()
    {
        m_input = GetComponent<PlayerInput>();
        TryGetComponent(out m_controller);
        TryGetComponent(out m_animator);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion

    #region Function calls
    private void OnEnable()
    {
        m_input.OnJump += OnJump;
    }

    private void OnDisable()
    {
        m_input.OnJump -= OnJump;
    }

    private void Update()
    {
        Move();
        LookAt();
    }
    #endregion

    #region Controller functions
    private void Move()
    {
        //if (!IsGrounded) return;
        //if (!m_characterController.isGrounded) return; // Pelo visto não precisa disso pra funcionar o movimento corretamente com o CharacterController

        float finalSpeed;
        if (m_input.IsWalking)
            finalSpeed = walkSpeed;
        else
            finalSpeed = runSpeed;

        float verticalInput = m_input.Vertical;
        float horizontalInput = m_input.Horizontal;

        // MOVEMENT
        Vector3 forwardMovement = cam.transform.forward * verticalInput;
        Vector3 rightMovement = cam.transform.right * horizontalInput;
        Vector3 movement = Vector3.ClampMagnitude(forwardMovement + rightMovement, 1) * finalSpeed;
        movement = new Vector3(movement.x, 0, movement.z);
        movement = Vector3.Lerp(m_lastMovementVector, movement, .9f);

        //Debug.Log("MOVEMENT: " + movement);

        // APPLY MOVEMENT
        m_controller.SimpleMove(movement);

        if (m_animator)
        {
            m_animator.PlayMovement(-walkSpeed, walkSpeed, -runSpeed, runSpeed, verticalInput * finalSpeed, horizontalInput * finalSpeed);
        }

        Speed = new Vector3(horizontalInput, 0, verticalInput) * finalSpeed;
        m_lastMovementVector = movement;
    }

    private void LookAt()
    {
        // Tutorial Using Camera
        float targetAngle = Mathf.Atan2(0, 0) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void OnJump()
    {
        if (Input.GetKeyUp(KeyCode.Space) && IsGrounded)
        {
            //if (m_rigidbody) m_rigidbody.AddForce(Vector3.up * jumpForce * m_rigidbody.mass, ForceMode.Impulse);
            StartCoroutine(OnJumpRoutine());
            if (m_animator) m_animator.PlayJump();
        }

    }

    private IEnumerator OnJumpRoutine()
    {
        Debug.Log("pulando");
        do
        {
            m_controller.Move(Vector3.up * jumpHeight * Time.deltaTime);
            yield return null;
        }
        while (!IsGrounded);
    }

    private bool IsGrounded
    {
        get
        {
            Collider[] collider = Physics.OverlapBox(transform.position, new Vector3(.25f, .1f, .25f), Quaternion.identity, jumpableLayers);
            return collider.Length > 0;

            //return m_controller.collisionFlags == CollisionFlags.Below;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(.5f, .2f, .5f));
    }
}
