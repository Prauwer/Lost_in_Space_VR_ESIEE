using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonPushExitSpaceShip : MonoBehaviour
{
    SpaceshipInteriorInteractions interior;
    // Start is called before the first frame update
    void Start()
    {
        interior = FindObjectOfType<SpaceshipInteriorInteractions>();
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => interior.ExitSpaceShip());
    }
}
