using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;
using Unity.XR.CoreUtils;

public class SeatedControls : MonoBehaviour
{
    [Header("Space Outside")]
    [Tooltip("Le monde autour du vaisseau à déplacer")]
    public GameObject spaceOutside;

    [Header("Vaisseau")]
    [Tooltip("Transform du vaisseau autour duquel on fera tourner le monde")]
    public Transform shipTransform;

    [Header("Vitesse de déplacement")]
    [Tooltip("Facteur de vitesse pour le déplacement du plan XZ")]
    public float moveSpeed = 1f;

    [Header("Vitesse de rotation")]
    [Tooltip("Vitesse angulaire (°/s) pour la rotation du monde en Y autour du vaisseau")]
    public float rotationSpeed = 45f;

    private XROrigin xrOriginComp;
    private CharacterController characterController;
    private Vector2 leftStickInput;
    private Vector2 rightStickInput;
    private Cloth[] childCloths;

    void Awake()
    {

    }

    void Start()
    {
        xrOriginComp = FindObjectOfType<XROrigin>();
        if (xrOriginComp == null)
        {
            Debug.LogError("Pas de XROrigin trouvé !");
            enabled = false;
            return;
        }
        characterController = xrOriginComp.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Réinitialiser chaque frame
        leftStickInput = Vector2.zero;
        rightStickInput = Vector2.zero;

        // Parcourir tous les XRController du nouveau Input System
        foreach (var device in InputSystem.devices.OfType<XRController>())
        {
            var stick = device.TryGetChildControl<Vector2Control>("primary2DAxis");
            if (stick == null)
                continue;

            var value = stick.ReadValue();
            if (device.usages.Contains(CommonUsages.LeftHand))
                leftStickInput = value;
            else if (device.usages.Contains(CommonUsages.RightHand))
                rightStickInput = value;
        }

        if (spaceOutside != null)
        {
            float deltaZ = leftStickInput.x * moveSpeed * Time.deltaTime;
            float deltaX = leftStickInput.y * moveSpeed * Time.deltaTime;

            spaceOutside.transform.Translate(-deltaZ, 0f, -deltaX, Space.World);

            if (shipTransform != null && spaceOutside != null)
            {
                float yawAngle = -rightStickInput.x * rotationSpeed * Time.deltaTime;
                spaceOutside.transform.RotateAround(
                    shipTransform.position,
                    Vector3.up,
                    yawAngle
                );

                float currentSkyboxRot = RenderSettings.skybox.GetFloat("_Rotation");
                RenderSettings.skybox.SetFloat("_Rotation", currentSkyboxRot - yawAngle);
            }

            childCloths = spaceOutside.GetComponentsInChildren<Cloth>();
            foreach (var c in childCloths)
            {
                c.useGravity = false;
                c.externalAcceleration = Vector3.zero;
            }

            foreach (var c in childCloths)
                c.ClearTransformMotion();

        }
    }
}
