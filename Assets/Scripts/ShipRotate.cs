using UnityEngine;

public class ShipRotate : MonoBehaviour
{
    [SerializeField, Tooltip("Drag factor to reduce angular velocity.")]
    private float angularDragFactor = 0.035f; // Drag factor to reduce angular velocity

    private Rigidbody rb; // Reference to the Rigidbody component



    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // Reduce angular velocity gradually using angular drag
        rb.angularVelocity *= (1f - angularDragFactor);

    }
}