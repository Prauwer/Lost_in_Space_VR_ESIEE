using UnityEngine;

[ExecuteInEditMode]
public class SpeedTunnelEffect : MonoBehaviour
{
    public Material tunnelMaterial; // Matériel avec le shader
    public Rigidbody spaceshipRigidbody; // Référence au Rigidbody du vaisseau
    public float blurMultiplier = 0.1f; // Facteur de multiplication pour le flou
    public float maxBlur = 5.0f; // Valeur maximale du flou

    private float currentBlurStrength;

    void Update()
    {
        if (spaceshipRigidbody != null && tunnelMaterial != null)
        {
            // Calculer la vitesse du vaisseau
            float speed = spaceshipRigidbody.velocity.magnitude;

            // Calculer l'intensité du flou proportionnellement à la vitesse
            currentBlurStrength = Mathf.Clamp(speed * blurMultiplier, 0, maxBlur);

            // Appliquer l'intensité du flou au shader
            tunnelMaterial.SetFloat("_BlurStrength", currentBlurStrength);
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (tunnelMaterial != null)
        {
            Graphics.Blit(src, dest, tunnelMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}