using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class SpaceshipExteriorInteractions : MonoBehaviour
{
    public Transform Player;
    private XROrigin xrOriginComp;


    public GameObject Interior;
    public GameObject DriveUI;

    public GameObject entrypoint;
    // Start is called before the first frame update
    void Start()
    {
        xrOriginComp = FindObjectOfType<XROrigin>();
    }

    public void EnterSpaceShip()
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
