using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrivedOnPlanetHandler : MonoBehaviour
{
    GameObject DonTDestroy;
    TeleportationArea TeleportationArea;
    // Start is called before the first frame update
    void Start()
    {
        
        DonTDestroy = GameObject.FindGameObjectWithTag("DonTDestroy");
        TeleportationArea = FindObjectOfType<TeleportationArea>();
        TerrainCollider terrainCollider = GetComponent<TerrainCollider>();

        TeleportationArea.colliders.Add(terrainCollider);

    }

    // Update is called once per frame
    void Update()
    {
    }
}