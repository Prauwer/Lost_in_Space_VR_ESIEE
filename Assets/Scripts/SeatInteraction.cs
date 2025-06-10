using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]
public class SeatInteraction : MonoBehaviour
{
    [Header("Seat Config")]
    [Tooltip("Position des pieds quand on s'assoit")]
    public Transform seatAnchor;
    [Tooltip("Position des pieds quand on sort du siège")]
    public Transform exitAnchor;

    [Header("Seated Controls")]
    [Tooltip("Le script qui gère l'input quand on est assis")]
    public SeatedControls seatedControls;

    [Header("Timing")]
    [Tooltip("Temps minimal entre deux toggles")]
    public float cooldown = 1f;

    private XROrigin xrOriginComp;
    private CharacterController characterController;
    private LocomotionProvider[] providers;
    private bool isSeated = false;
    private float lastActionTime = 0f;

    void Start()
    {
        // Récupération du XROrigin et de son CharacterController
        xrOriginComp = FindObjectOfType<XROrigin>();
        if (xrOriginComp == null)
        {
            Debug.LogError("Pas de XROrigin trouvé !");
            enabled = false;
            return;
        }
        characterController = xrOriginComp.GetComponent<CharacterController>();

        // Récupère tous les LocomotionProvider pour pouvoir les couper
        providers = FindObjectsOfType<LocomotionProvider>();

        if (seatedControls == null)
        {
            Debug.LogWarning("⚠️ Le champ 'seatedControls' n'est pas assigné dans l'inspecteur !");
        }
        else
        {
            seatedControls.enabled = isSeated;
        }
    }

    void Update()
    {
        // Détection du bouton A/X via le nouveau Input System
        foreach (var dev in InputSystem.devices)
        {
            var btn = dev.TryGetChildControl<ButtonControl>("primaryButton");
            if (btn != null && btn.wasPressedThisFrame)
            {
                if (Time.time - lastActionTime >= cooldown)
                {
                    lastActionTime = Time.time;
                    ToggleSeat();
                }
                break;
            }
        }
    }

    void ToggleSeat()
    {
        isSeated = !isSeated;

        // 1) (Dé)activation des locomotion providers
        foreach (var p in providers)
            p.enabled = !isSeated;

        // 2) (Dé)activation du CharacterController
        if (characterController != null)
            characterController.enabled = !isSeated;

        // 3) Téléportation du rig :
        //    - si on s'assoit → seatAnchor
        //    - si on se lève  → exitAnchor
        if (xrOriginComp != null)
        {
            var target = isSeated ? seatAnchor : exitAnchor;
            if (target != null)
            {
                xrOriginComp.transform.SetPositionAndRotation(
                    target.position,
                    target.rotation
                );
            }
            else
            {
                Debug.LogWarning("Missing " + (isSeated ? "seatAnchor" : "exitAnchor"));
            }
        }

        // 4) À chaque toggle, reset X/Z de l'offset de la caméra (garde la hauteur)
        var floor = xrOriginComp.CameraFloorOffsetObject.transform;
        float currentY = floor.localPosition.y;
        floor.localPosition = new Vector3(0f, currentY, 0f);
        floor.localRotation = Quaternion.identity;

        // 5) Active/désactive le script de contrôle assis
        if (seatedControls != null)
            seatedControls.enabled = isSeated;

        Debug.Log(isSeated
            ? "🪑 Assis : locomotion OFF, rig → seatAnchor"
            : "🚶 Levé : locomotion ON, rig → exitAnchor");
    }

    void OnDrawGizmos()
    {
        if (seatAnchor != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(seatAnchor.position, 0.1f);
        }
        if (exitAnchor != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(exitAnchor.position, 0.1f);
        }
    }
}
