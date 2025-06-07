using UnityEngine;

[ExecuteInEditMode]
public class OrbitGizmoFixedDash : MonoBehaviour
{
    [Header("Apparence du dash")]
    [Tooltip("Longueur (en unités) de chaque tiret")]
    public float dashLength = 1f;
    [Tooltip("Longueur (en unités) de chaque espace entre les tirets")]
    public float gapLength = 0.5f;
    [Tooltip("Pour courber chaque tiret, nombre de petits segments par dash (1 = ligne droite)")]
    [Range(1, 16)] public int segmentsPerDash = 1;

    [Header("Parameters supplémentaires")]
    [Tooltip("Dessiner aussi la ligne entre l'objet et le centre")]
    public bool drawLineToCenter = false;
    [Tooltip("Couleur du gizmo")]
    public Color gizmoColor = Color.cyan;

    private void OnDrawGizmos()
    {
        Vector3 center = Vector3.zero;

        // Rayon sur le plan XZ
        Vector3 posXZ = new Vector3(transform.position.x, 0f, transform.position.z);
        float radius = posXZ.magnitude;
        if (radius <= 0f) return;

        // Périmètre et nombre de cycles dash+gap
        float circumference = 2f * Mathf.PI * radius;
        float cycleLen = dashLength + gapLength;
        int cycleCount = Mathf.FloorToInt(circumference / cycleLen);
        float anglePerUnit = 1f / radius; // θ = s / R

        Gizmos.color = gizmoColor;

        // Pour chaque dash
        for (int i = 0; i < cycleCount; i++)
        {
            // arc-length en début de dash
            float startArc = i * cycleLen;

            // tracer le dash en segments courbés
            for (int seg = 0; seg < segmentsPerDash; seg++)
            {
                // t0 et t1 vont de 0 à 1 sur la longueur du dash
                float t0 = seg / (float)segmentsPerDash;
                float t1 = (seg + 1) / (float)segmentsPerDash;

                // arc-lengths
                float arc0 = startArc + dashLength * t0;
                float arc1 = startArc + dashLength * t1;

                // angles correspondants
                float θ0 = arc0 * anglePerUnit;
                float θ1 = arc1 * anglePerUnit;

                // points sur le cercle
                Vector3 p0 = center + new Vector3(Mathf.Cos(θ0) * radius, 0f, Mathf.Sin(θ0) * radius);
                Vector3 p1 = center + new Vector3(Mathf.Cos(θ1) * radius, 0f, Mathf.Sin(θ1) * radius);

                Gizmos.DrawLine(p0, p1);
            }
        }

        // Optionnel : ligne vers le centre
        if (drawLineToCenter)
            Gizmos.DrawLine(transform.position, center);
    }
}
