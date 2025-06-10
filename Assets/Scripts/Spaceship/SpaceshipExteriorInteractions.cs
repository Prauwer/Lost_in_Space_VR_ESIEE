using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class SpaceshipExteriorInteractions : MonoBehaviour
{
    public Transform Player;
    private XROrigin xrOriginComp;

    bool canExit;

    public GameObject Interior;
    public GameObject DriveUI;

    public GameObject entrypoint;
    // Start is called before the first frame update
    void Start()
    {
        xrOriginComp = FindObjectOfType<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (canExit)
            {
                Interior.SetActive(true);
                Player.position = entrypoint.transform.position;
                gameObject.SetActive(false);

                // reset X/ Z de l'offset de la caméra (garde la hauteur)
                var floor = xrOriginComp.CameraFloorOffsetObject.transform;
                float currentY = floor.localPosition.y;
                floor.localPosition = new Vector3(0f, currentY, 0f);
                floor.localRotation = Quaternion.identity;
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        canExit = true;
        DriveUI.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.active)
        {
            canExit = false;
            DriveUI.gameObject.SetActive(false);
        }
    }
}
