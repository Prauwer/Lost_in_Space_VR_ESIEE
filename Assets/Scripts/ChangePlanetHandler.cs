using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using System.Collections;

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
            // D�tection du bouton B/Y via le nouveau Input System
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
        SceneManager.LoadScene(sceneName);
        isInSpace = false;
        StartCoroutine(ResetSecondaryButton());

    }

    void GoToSpace()
    {
        //gameObject.transform.position = SpaceshipPosition.position;
        //gameObject.transform.rotation = SpaceshipPosition.rotation;
        isInSpace = true;
        SceneManager.LoadScene("main scene");

        StartCoroutine(ResetSecondaryButton());
    }

    IEnumerator ResetSecondaryButton()
    {
        // on attend la fin de frame pour être sûr d’être dans la nouvelle scène
        yield return null;

        // pour chaque device on remet la valeur du bouton à 0
        foreach (var dev in InputSystem.devices)
        {
            var btn = dev.TryGetChildControl<ButtonControl>("secondaryButton");
            if (btn != null)
                InputState.Change(btn, 0f);
        }
        // on force le système à traiter l’événement tout de suite
        InputSystem.Update();
    }
}
