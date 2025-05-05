using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipInteraction : MonoBehaviour
{

    public MonoBehaviour SpaceshipController;
    public Transform Player;

    [Header("Cameras")]
    public GameObject PlayerCam;
    public GameObject ShipCam;

    public GameObject DriveUI;

    bool Candrive;
    bool driving;

    // Start is called before the first frame update
    void Start()
    {
        ShipCam.SetActive(false);
        DriveUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Candrive && !driving) // If the player can drive and is not currently driving
            {
                driving = true;
                Player.gameObject.SetActive(false); // Deactivate player
                Player.transform.SetParent(transform); // Link it to the ship
                ShipCam.SetActive(true); // Activate ship cam and controls
                SpaceshipController.enabled = true;
            }
            else if (driving) // If the player is currently driving
            {
                driving = false;
                Player.transform.SetParent(null); // Unlink player from ship
                Player.gameObject.SetActive(true); // Activate player
                ShipCam.SetActive(false); // Deactivate ship cam and controls
                SpaceshipController.enabled = false;

            }
        }


    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Candrive = true;
            DriveUI.gameObject.SetActive(true);
        }
        if (driving)
        {
            DriveUI.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Candrive = false;
            DriveUI.gameObject.SetActive(false);
        }
    }
}
