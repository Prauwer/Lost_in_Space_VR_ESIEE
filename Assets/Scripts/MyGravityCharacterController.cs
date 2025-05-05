using UnityEngine;

/// <summary>
/// The bulk of character-related movement and animation.
/// Heavily modified from the standard assets' ThirdPersonCharacter.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MyGravityCharacterController : MonoBehaviour
{
	[SerializeField] float m_GroundCheckDistance = 1.2f; // Higher value leads to more slope acceptance
    [SerializeField] float m_FallCheckDistance = 4f; // For standOnNormals planets, how far the ground is allowed to be under the player before ignoring standOnNormals

    [HideInInspector] public bool m_IsGrounded;
    [HideInInspector] public bool m_Crouching;
    [HideInInspector] public Vector3 m_GroundNormal;

    Rigidbody m_Rigidbody;
    public MyPlayerPhysics m_Phys;
    float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_TurnAmount;
	float m_ForwardAmount;
	float m_CapsuleHeight;
	Vector3 m_CapsuleCenter;
	CapsuleCollider m_Capsule;
    GameObject m_prevGravity;


	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
        m_Phys = GetComponent<MyPlayerPhysics>();
        m_Capsule = GetComponent<CapsuleCollider>();
		m_CapsuleHeight = m_Capsule.height;
		m_CapsuleCenter = m_Capsule.center;

		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		m_OrigGroundCheckDistance = m_GroundCheckDistance;
	}

	void ScaleCapsuleForCrouching(bool crouch)
	{
		if (m_IsGrounded && crouch)
		{
			if (m_Crouching) return;
			m_Capsule.height = m_Capsule.height / 2f;
			m_Capsule.center = m_Capsule.center / 2f;
			m_Crouching = true;
		}
		else
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + transform.up * m_Capsule.radius * k_Half, transform.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
				return;
			}
			m_Capsule.height = m_CapsuleHeight;
			m_Capsule.center = m_CapsuleCenter;
			m_Crouching = false;
		}
	}

	void PreventStandingInLowHeadroom()
	{
		// prevent standing up in crouch-only zones
		if (!m_Crouching)
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + transform.up * m_Capsule.radius * k_Half, transform.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
			}
		}
	}



    /// <summary>
    /// Gives an additional pull of gravity and lowers lateral velocity.
    /// </summary>
    private void ExtraGravity(float factor)
    {
        Vector3 extraGravityForce;
        if (m_Phys.effector)
            extraGravityForce = Time.fixedDeltaTime * m_Phys.effector.gravity;
        else if (m_Phys.attractor)
            extraGravityForce = Time.fixedDeltaTime * m_Phys.attractor.gravity * Vector3.Normalize(m_Phys.attractor.gameObject.transform.position - transform.position);
        else
            return;

        m_Rigidbody.velocity *= (1 - factor) + (factor * Mathf.Abs(Vector3.Dot(m_Rigidbody.velocity.normalized, extraGravityForce.normalized)));
        m_Rigidbody.AddForce(factor * extraGravityForce);
    }


#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Draws the CheckGroundStatus SphereCast in the inspector (gizmos need to be enabled to see it)
        // It displays the endpoint of the SphereCast, not the full cast!
        float radius = 0.3f; // Set this to the collider capsule radius if you change the capsule
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.up * radius) - (transform.up * m_GroundCheckDistance), radius - 0.05f);
    }
#endif

    public void CheckGroundStatus()
	{
        float verticalSpeed = Vector3.Dot(m_Rigidbody.velocity, transform.up); // Only check ground if not moving upwards

        Debug.Log($"Vertical Speed: {verticalSpeed}");
        Debug.Log($"Rigid Body Position: {m_Rigidbody.position}");
        Debug.Log($"Transform Position: {transform.position}");
        Debug.Log($"Ground Check Distance: {m_GroundCheckDistance}");

        RaycastHit hitInfo;
        // Afficher des informations détaillées sur le raycast
        bool raycastHit = Physics.SphereCast(
            transform.position + (transform.up * m_Capsule.radius),
            m_Capsule.radius - 0.05f,
            -transform.up,
            out hitInfo,
            m_GroundCheckDistance
        );

        Debug.Log($"Raycast Hit: {raycastHit}");
        if (raycastHit)
        {
            Debug.Log($"Hit Point: {hitInfo.point}");
            Debug.Log($"Hit Normal: {hitInfo.normal}");
            Debug.Log($"Hit Distance: {hitInfo.distance}");
        }

        // Casts a sphere along a ray for a ground collision check.
        // The sphere is 0.05f smaller than the collider capsule and starts at the bottom point of the capsule, giving it a 0.1f offset for safety.
        if ((verticalSpeed <= 0.85f) && Physics.SphereCast(transform.position + (transform.up * m_Capsule.radius), m_Capsule.radius - 0.05f, -transform.up, out hitInfo, m_GroundCheckDistance))
        {
            m_IsGrounded = true;
            m_GroundNormal = hitInfo.normal;
        }
		else
		{
            // Airborne
			m_IsGrounded = false;
            
            if (m_Phys.effector) // Effector: set ground normal to direction of gravity
                m_GroundNormal = Vector3.Normalize(m_Phys.effector.gravity);
            else if (m_Phys.attractor) // Attractor:
            {
                if (m_prevGravity != m_Phys.attractor.gameObject) // After switching planets, set ground normal to direction of gravity
                    GetGroundNormalFromGravity();
                else if (m_Phys.attractor.standOnNormals && // On StandOnNormals planets...
                    ((!Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, m_FallCheckDistance)) || // If ground is not under player
                            (Vector3.Angle(hitInfo.normal, transform.up) > 45))) // or at too steep of a slope
                    GetGroundNormalFromGravity(); // Make sure that the player does not fall off the planet
            }
        }


        // Update the previous source of gravity
        if (m_Phys.effector)
            m_prevGravity = m_Phys.effector.gameObject;
        else if (m_Phys.attractor)
            m_prevGravity = m_Phys.attractor.gameObject;
	}

    private void GetGroundNormalFromGravity()
    {
        m_GroundNormal = Vector3.Normalize(transform.position - m_Phys.attractor.gameObject.transform.position);
    }
}
