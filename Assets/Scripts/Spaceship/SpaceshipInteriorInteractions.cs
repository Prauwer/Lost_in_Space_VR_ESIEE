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

    public void ExitSpaceShip()
    {
        if (ParentObject.isInSpace)
        {
            return;
        }
        Exterior.SetActive(true);
        Player.position = exitpoint.transform.position;

        // reset X/ Z de l'offset de la cam�ra (garde la hauteur)
        var floor = xrOriginComp.CameraFloorOffsetObject.transform;
        float currentY = floor.localPosition.y;
        floor.localPosition = new Vector3(0f, currentY, 0f);
        floor.localRotation = Quaternion.identity;


        gameObject.SetActive(false);
    }
}
