using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessBlender : MonoBehaviour
{
    public Volume happyVolume;  // Volume heureux
    public Volume sadVolume;    // Volume triste
    public SolitudeBar solitudePanel;    // Référence au script contenant l'int

    void Start()
    {
        happyVolume.gameObject.SetActive(true);
        sadVolume.gameObject.SetActive(true);
    }

    void Update()
    {
        // Assurez-vous que SolitudePanel a un champ public ou une propriété pour accéder à l'int
        float solitudeLevel = solitudePanel.solitude;  // L'int qui varie entre 0 et 100

        // Normalisez la valeur entre 0 et 1
        float blendFactor = Mathf.Clamp01(solitudeLevel / 100f);

        // Ajuste les weights des volumes
        happyVolume.weight = blendFactor; // Volume heureux augmente avec le facteur
        sadVolume.weight = 1 - blendFactor; // Volume triste diminue
    }
}
