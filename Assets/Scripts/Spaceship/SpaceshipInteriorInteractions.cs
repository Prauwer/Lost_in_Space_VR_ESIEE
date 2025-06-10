using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class SpaceshipInteriorInteractions : MonoBehaviour
{
    public Transform Player;

    bool canExit;

    private XROrigin xrOriginComp;
    public GameObject Exterior;
    public GameObject DriveUI;
    public ChangePlanetHandler ParentObject;

    public GameObject exitpoint;
    // Start is called before the first frame update
    void Start()
    {
        xrOriginComp = FindObjectOfType<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ParentObject.isInSpace)
        {
            canExit = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (canExit)
            {
                Exterior.SetActive(true);
                Player.position = exitpoint.transform.position;

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
