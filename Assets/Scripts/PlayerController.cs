using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;


public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;

    private int count;
    public TextMeshProUGUI countText;

    public GameObject winTextObject;

    public RawImage frontScreenImage;
    public GameObject dialogueBox;
    public GameObject instructionBox;
    public SolitudeBar solitudePanel;

    [Header("Mouvement")]
    public float speed = 6f;
    public float jumpForce = 5f;
    private bool isGrounded;
    public Vector3 groundNormal;

    private MyGravityCharacterController characterController;

    [Header("Souris")]
    public float mouseSensitivity = 250f; // Sensibilité de la souris
    public Transform playerCamera; // Référence à la caméra
    private float xRotation = 0f; // Rotation verticale accumulée
    private Vector3 velocity; // Vitesse actuelle (gravité incluse)
    private Vector3 direction;
    public float moveSpeed = 5f; // Vitesse de déplacement


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<MyGravityCharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Bloque le curseur au centre de l'écran
        Cursor.visible = false; // Masque le curseur
        
        winTextObject.gameObject.SetActive(false);

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // La gravité est gérée ailleurs
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        count = 0;
        SetCountText();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText(); 
        }
        else if (other.gameObject.CompareTag("familyPhoto"))
        {
            frontScreenImage.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(true);
            other.gameObject.SetActive(false);
            Renderer objectRenderer = other.gameObject.GetComponent<Renderer>();
            if (objectRenderer != null && objectRenderer.material.mainTexture != null)
            {
                frontScreenImage.texture = objectRenderer.material.mainTexture;
            }
            solitudePanel.solitude = Math.Min(solitudePanel.solitude + 35, 100);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        countText.color = Color.red;

        if (count >= 5)
        {
            winTextObject.SetActive(true);
        }
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.E)){
            frontScreenImage.gameObject.SetActive(false);
            dialogueBox.gameObject.SetActive(false);
            instructionBox.gameObject.SetActive(false);
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotation verticale de la caméra (limite l'angle)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Applique la rotation à la caméra et au joueur
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void CheckGroundStatus()
    {
        // Augmentez la distance de détection pour plus de robustesse
        float groundCheckDistance = 1.5f;

        RaycastHit hitInfo;
        bool raycastHit = Physics.Raycast(
            transform.position,
            -transform.up,
            out hitInfo,
            groundCheckDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        if (raycastHit)
        {
            // Vérifier l'angle entre la normale et l'up du personnage
            float angleToUp = Vector3.Angle(hitInfo.normal, transform.up);
            if (angleToUp < 45f)
            {
                characterController.m_IsGrounded = true;
                characterController.m_GroundNormal = hitInfo.normal;
            }
            else
            {
                // Utiliser une normale de repli si l'angle est trop important
                characterController.m_IsGrounded = true;
                characterController.m_GroundNormal = transform.up;
            }
        }
        else
        {
            characterController.m_IsGrounded = false;

            // Logique existante pour définir characterController.m_GroundNormal en l'air
        }
    }

    /// <summary>
    /// Gère les déplacements du joueur avec ZQSD (ou WASD).
    /// </summary>
    private void HandleMovement()
    {
        // Vérifier le statut du sol à chaque frame
        CheckGroundStatus();

        // Récupération des entrées utilisateur
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Créer le vecteur de mouvement basé sur l'orientation du joueur
        Vector3 moveDirection = transform.right * x + transform.forward * z;

        // Projection du mouvement sur la surface (si au sol)
        if (characterController.m_IsGrounded && characterController.m_GroundNormal != Vector3.zero)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, characterController.m_GroundNormal);
        }

        // Normaliser et multiplier par la vitesse
        Vector3 targetVelocity = moveDirection.normalized * speed;

        // Conserver la composante verticale de la vélocité existante
        targetVelocity.y = rb.velocity.y;

        // Appliquer la vélocité cible
        rb.velocity = targetVelocity;

        // Saut
        if (Input.GetButtonDown("Jump") && characterController.m_IsGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        // Logs de débogage
        //Debug.Log("Move Direction: " + moveDirection);
        //Debug.Log("Ground Normal: " + characterController.m_GroundNormal);
        //Debug.Log("Is Grounded: " + characterController.m_IsGrounded);
    }
}
