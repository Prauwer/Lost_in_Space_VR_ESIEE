using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonPushExitSpaceShip : MonoBehaviour
{
    public SpaceshipInteriorInteractions interior;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => interior.ExitSpaceShip());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
