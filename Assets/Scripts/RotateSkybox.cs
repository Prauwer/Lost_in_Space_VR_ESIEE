using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float currentSkyboxRot = RenderSettings.skybox.GetFloat("_Rotation");
        float deltaRot = speed * Time.deltaTime;

        RenderSettings.skybox.SetFloat("_Rotation", currentSkyboxRot + deltaRot);
    }
}