using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollowPlayer : MonoBehaviour
{
    // Vitesse de rotation
    public float rotationSpeed = 5.0f;

    // Référence à l'objet joueur
    private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

        parentTransform.Rotate(Vector3.up, mouseX, Space.Self);
    }
}
