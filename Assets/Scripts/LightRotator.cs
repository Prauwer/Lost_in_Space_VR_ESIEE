using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightRotator : MonoBehaviour
{
    public Transform Player;

    void Update()
    {
        SetRotation();
    }

    void SetRotation()
    {
        // Accès aux coordonnées du Player via Player.position
        Vector3 playerPosition = Player.position;

        // Calcul de l'angle en radians (autour de l'axe Y)
        double angleX_rad = Math.Atan2(playerPosition.x, playerPosition.z);

        // Conversion en degrés
        float angleX_deg = (float)(angleX_rad * (180.0 / Math.PI));

        // Appliquer la rotation de l'objet courant (fixer l'orientation)
        transform.rotation = Quaternion.Euler(0, angleX_deg, 0);
    }
}
