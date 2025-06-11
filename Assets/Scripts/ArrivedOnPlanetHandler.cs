using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrivedOnPlanetHandler : MonoBehaviour
{
    GameObject DonTDestroy;
    TeleportationArea TeleportationArea;
    Terrain Terrain;
    // Start is called before the first frame update
    void Start()
    {
        /*
        DonTDestroy = GameObject.FindGameObjectWithTag("DonTDestroy");
        TeleportationArea = FindObjectOfType<TeleportationArea>();
        Terrain = FindObjectOfType<Terrain>();

        DonTDestroy.transform.position = gameObject.transform.position;
        DonTDestroy.transform.rotation = gameObject.transform.rotation;
        */
        //TeleportationArea.colliders.Add(Terrain.GetComponent<Collider>());

    }

    // Update is called once per frame
    void Update()
    {
    }
}