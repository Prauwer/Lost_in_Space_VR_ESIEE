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
    public float cooldown = 1f;
    private float lastActionTime = 0f;

    public bool isInSpace = true;

    private GameObject Space;
    private Vector3 SpacePosition;
    private Quaternion SpaceRotation;

    private bool shouldRestorePosition = false; // <- ajouté

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "main scene")
        {
            Space = GameObject.FindGameObjectWithTag("Space");

            if (shouldRestorePosition && Space != null)
            {
                Space.transform.position = SpacePosition;
                Space.transform.rotation = SpaceRotation;
                shouldRestorePosition = false;
            }
        }
    }

    void Update()
    {
        if (!isInSpace)
        {
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
        var spaceObj = GameObject.FindGameObjectWithTag("Space");
        if (spaceObj != null)
        {
            SpacePosition = spaceObj.transform.position;
            SpaceRotation = spaceObj.transform.rotation;
            shouldRestorePosition = true; // <- important
        }

        isInSpace = false;
        SceneManager.LoadScene(sceneName);
        StartCoroutine(ResetSecondaryButton());
    }

    void GoToSpace()
    {
        isInSpace = true;
        SceneManager.LoadScene("main scene");
        StartCoroutine(ResetSecondaryButton());
    }

    IEnumerator ResetSecondaryButton()
    {
        yield return null;

        foreach (var dev in InputSystem.devices)
        {
            var btn = dev.TryGetChildControl<ButtonControl>("secondaryButton");
            if (btn != null)
                InputState.Change(btn, 0f);
        }

        InputSystem.Update();
    }
}
