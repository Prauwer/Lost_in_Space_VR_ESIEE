using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class ChangePlanetHandler : MonoBehaviour
{
    [Tooltip("Temps minimal entre deux toggles")]
    public float cooldown = 1f;
    private float lastActionTime = 0f;

    private Transform SpaceshipPosition;
    public bool isInSpace = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!isInSpace)
        {
            // Détection du bouton B/Y via le nouveau Input System
            foreach (var dev in InputSystem.devices)
            {
                var btn = dev.TryGetChildControl<ButtonControl>("secondaryButton");
                if (btn != null && btn.wasPressedThisFrame)
                {
                    if (Time.time - lastActionTime >= cooldown)
                    {
                        lastActionTime = Time.time;
                        GoToSpace();
                    }
                    break;
                }
            }
        }
    }

    public void GoToPlanet(string sceneName)
    {
        SpaceshipPosition = gameObject.transform;
        isInSpace = false;
        SceneManager.LoadScene(sceneName);
    }

    void GoToSpace()
    {
        //gameObject.transform.position = SpaceshipPosition.position;
        //gameObject.transform.rotation = SpaceshipPosition.rotation;
        isInSpace = true;
        SceneManager.LoadScene("main scene");
        var sim = FindObjectOfType<XRDeviceSimulator>();
        if (sim != null)
        {
            sim.enabled = false;
            sim.enabled = true;
        }
    }
}
