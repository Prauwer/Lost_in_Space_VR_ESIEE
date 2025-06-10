using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class EnterPlanet : MonoBehaviour
{
    public ChangePlanetHandler ChangePlanetHandler;
    bool canEnter = false;
    public string SceneName;
    [Tooltip("Temps minimal entre deux toggles")]
    public float cooldown = 1f;
    private float lastActionTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        //ChangePlanetHandler = FindObjectOfType<ChangePlanetHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        // Détection du bouton B/Y via le nouveau Input System
        foreach (var dev in InputSystem.devices)
        {
            var btn = dev.TryGetChildControl<ButtonControl>("secondaryButton");
            if (canEnter && btn != null && btn.wasPressedThisFrame)
            {
                if (Time.time - lastActionTime >= cooldown)
                {
                    lastActionTime = Time.time;
                    ChangePlanetHandler.GoToPlanet(SceneName);
                }
                break;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.active)
        {
            canEnter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.active)
        {
            canEnter = false;
        }
    }
}
