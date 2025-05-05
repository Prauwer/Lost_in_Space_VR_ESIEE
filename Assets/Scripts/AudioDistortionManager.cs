using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDistortionManager : MonoBehaviour
{
    public AudioSource musicSource; // La source audio jouant la musique
    public SolitudeBar solitudePanel; // Référence au script contenant l'int

    void Update()
    {
        // Assurez-vous que SolitudePanel a un champ public ou une propriété pour accéder à l'int
        float solitudeLevel = solitudePanel.solitude; // L'int qui varie entre 0 et 100

        // Si la santé mentale est au dessus de 60, on laisse par défaut
        if (solitudeLevel > 60)
        {
            musicSource.pitch = 1f;
            musicSource.spatialBlend = 0f;
            musicSource.reverbZoneMix = 0f;
        }
        // Si entre 20 et 60, on varie
        else if (solitudeLevel <= 60 && solitudeLevel > 20)
        {
            // Normalisation pour la plage 60 à 20
            float blendFactor = Mathf.Clamp01((60f - solitudeLevel) / 60f);

            // Ajustement des paramètres
            musicSource.pitch = Mathf.Lerp(1f, 0.5f, blendFactor); // Diminue le pitch
            musicSource.spatialBlend = Mathf.Lerp(0f, 1f, blendFactor); // Ajoute un effet 3D progressif
            musicSource.reverbZoneMix = Mathf.Lerp(0f, 1f, blendFactor); // Augmente le mix de reverb
        }
        // On met les paramètres à 20 si en dessous
        else
        {
            float blendFactor = Mathf.Clamp01((60f - 20) / 60f);
            musicSource.pitch = Mathf.Lerp(1f, 0.5f, blendFactor);
            musicSource.spatialBlend = Mathf.Lerp(0f, 1f, blendFactor);
            musicSource.reverbZoneMix = Mathf.Lerp(0f, 1f, blendFactor);
        }
    }
}
