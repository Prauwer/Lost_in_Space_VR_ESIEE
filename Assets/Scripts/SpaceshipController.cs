using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField, Tooltip("Force applied to each thruster of the ship.")]
    private float jetpackForce = 200f; // Adjust the force as needed
    [SerializeField, Tooltip("Velocity reduce on stabilize button.")]
    private float stabilizeSpeed = 0.5f;
    [SerializeField, Tooltip("Lateral rotation speed from the ship.")]
    private float rotationSpeed = 200f;

    [SerializeField, Tooltip("Audio asset of the thruster sound.")]
    private AudioSource thrusterSound; // Reference to the AudioSource component

    [SerializeField, Tooltip("Under reactor particles gameobject.")]
    private GameObject underReactorParticles;

    public Rigidbody rb;

    private Dictionary<KeyCode, Vector3> keyMappings;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Initialize the keyMappings dictionary with transform-based directions
        keyMappings = new Dictionary<KeyCode, Vector3>()
        {
            { KeyCode.Space, -rb.transform.up },
            { KeyCode.LeftControl, rb.transform.up },
            { KeyCode.Z, -rb.transform.forward },
            { KeyCode.S, rb.transform.forward },
            { KeyCode.Q, -rb.transform.right },
            { KeyCode.D, rb.transform.right },
            { KeyCode.X, Vector3.zero } // This key stops all movement
        };

        // Deactivate reactors particles
        underReactorParticles.SetActive(false);

        // Lock the cursor to the center of the game window
        Cursor.lockState = CursorLockMode.Locked;

        // Hide the cursor
        Cursor.visible = false;

        enabled = false;
    }

    void Update()
    {

        bool isKeyPressed = false;
        List<KeyCode> keysPressed = new();

        foreach (var kvp in keyMappings)
        {
            if (Input.GetKey(kvp.Key))
            {
                isKeyPressed = true;

                keysPressed.Add(kvp.Key);

                if (kvp.Key == KeyCode.X) // Special case for stopping movement
                {
                    StopMovement();
                }
                else
                {
                    Move(kvp.Value);
                }
            }
            PlaySound(keysPressed);
            PlayParticles();
        }

        // D�placement du vaisseau en fonction de la souris

        // Get mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");


        Vector3 torque;
        if (Input.GetKey(KeyCode.R)) // Different rotation if R is pressed
        {
            torque = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed;
        }
        else
        {
            torque = new Vector3(-mouseY, 0f, mouseX) * rotationSpeed;
        }

        rb.AddRelativeTorque(torque, ForceMode.Force);
        // Fin d�placement souris

        // Arreter les sons si aucun input clavier
        if (!isKeyPressed)
        {
            StopSound();
            StopParticles();
        }

        //DEBUG
        if (rb != null)
        {
            // Get the rotational forces acting on the object
            Vector3 velocity = rb.velocity;

            // Extend the length of the line for visualization

            Vector3 endPosition = transform.position + velocity * 10f; // Adjust 10f to your desired length


            // Draw lines to visualize the rotational forces
            Debug.DrawRay(transform.position, endPosition - transform.position, Color.red);
        }
        // END DEBUG

    }

    void Move(Vector3 direction)
    {
        Vector3 localDirection = transform.TransformDirection(direction);

        rb.AddForce(localDirection * jetpackForce, ForceMode.Force);
    }

    void StopMovement()
    {
        Vector3 newVelocity = Vector3.Lerp(rb.velocity, Vector3.zero, stabilizeSpeed * Time.deltaTime);
        rb.velocity = newVelocity;
    }

    void PlaySound(List<KeyCode> keys)
    {

        if ((!keys.Contains(KeyCode.Q) && !keys.Contains(KeyCode.D)) || (keys.Contains(KeyCode.Q) && keys.Contains(KeyCode.D)))
        {
            thrusterSound.panStereo = 0;
        }

        else if (keys.Contains(KeyCode.Q))
        {
            thrusterSound.panStereo = 0.6f;
        }

        else if (keys.Contains(KeyCode.D))
        {
            thrusterSound.panStereo = -0.6f;
        }

        if (!thrusterSound.isPlaying)
        {
            thrusterSound.Play();
        }
    }

    void StopSound()
    {
        if (thrusterSound.isPlaying)
        {
            thrusterSound.Stop();
        }
    }

    void PlayParticles()
    {
        underReactorParticles.SetActive(true); // Activate the particle system
    }

    void StopParticles()
    {
        underReactorParticles.SetActive(false);
    }
}