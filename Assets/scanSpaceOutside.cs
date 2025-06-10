using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scanSpaceOutside : MonoBehaviour
{
    [Header("Paramètres de recherche")]
    [Tooltip("Nom exact du GameObject à trouver (ou laissez vide pour utiliser le tag)")]
    public string objectName = "Space Outside";

    // Référence à votre script SeatedControls
    private SeatedControls seatedControls;

    void Awake()
    {
        // On récupère le premier SeatedControls trouvé dans la scène
        seatedControls = FindObjectOfType<SeatedControls>();
        if (seatedControls == null)
            Debug.LogWarning("[SpaceOutsideScanner] Aucun SeatedControls trouvé dans la scène !");
    }

    void Update()
    {
        seatedControls.spaceOutside = GameObject.Find(objectName);
    }
}
