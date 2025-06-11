using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonPushEnterSpaceShip : MonoBehaviour
{
    SpaceshipExteriorInteractions exterior;
    // Start is called before the first frame update
    void Start()
    {
        exterior = FindObjectOfType<SpaceshipExteriorInteractions>();
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => exterior.EnterSpaceShip());
    }
}
