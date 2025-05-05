using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SolitudeBar : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider slider; // Assign the Slider from the Inspector
    [SerializeField] private TextMeshProUGUI solitudeText; // Assign the TextMeshPro text component
    [SerializeField] private Canvas solitudeCanvas; // Assign the Canvas from the Inspector

    [Header("Solitude Settings")]
    [Range(0f, 100f)] [SerializeField] public float solitude = 100f;
    [SerializeField] public bool isCanvasVisible = false; // Canvas is invisible by default

    private Coroutine flickerCoroutine;

    public float Solitude
    {
        get => solitude;
        set
        {
            solitude = Mathf.Clamp(value, 0f, 100f); // Clamp value to stay within 0 and 100
            UpdateBar();
        }
    }

    public bool IsCanvasVisible
    {
        get => isCanvasVisible;
        set
        {
            if (isCanvasVisible == value)
            {
                return; // Avoid unnecessary updates
            }
            else
            {
                isCanvasVisible = true;
                ToggleCanvasVisibility(isCanvasVisible);
            }
        }
    }

    void Start()
    {
        // Initialize Canvas visibility and UI
        ToggleCanvasVisibility(isCanvasVisible);
        UpdateBar();
    }

    void Update()
    {
        UpdateBar();
    }

    void UpdateBar()
    {
        if (!solitudeCanvas.gameObject.activeSelf)
            return; // Skip updates if Canvas is hidden

        // Update the slider and text
        slider.value = solitude; // Set the slider value to the solitude (float)
        solitudeText.text = "Solitude: " + Mathf.RoundToInt(solitude) + "%"; // Display rounded value for clarity
    }

    public void ToggleCanvasVisibility(bool isVisible)
    {
        solitudeCanvas.gameObject.SetActive(isVisible);

        if (isVisible)
        {
            UpdateBar();
        }
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            // Ensure real-time updates for solitude and canvas visibility
            UpdateBar();
            ToggleCanvasVisibility(isCanvasVisible);
        }
    }
}
