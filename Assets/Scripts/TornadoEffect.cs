using UnityEngine;

/// <summary>
/// Controls the physics and visuals of a Tornado.
/// It pulls objects towards the center, lifts them up, and spins them around.
/// </summary>
public class TornadoEffect : MonoBehaviour
{
    [Header("Tornado Physics")]
    [Tooltip("How far away objects are affected by the tornado")]
    public float pullRadius = 20f;

    [Tooltip("Force pulling objects towards the center")]
    public float pullForce = 50f;

    [Tooltip("Force lifting objects upwards")]
    public float liftForce = 20f;

    [Tooltip("Force spinning objects around the tornado")]
    public float spinForce = 30f;

    [Header("Visuals")]
    [Tooltip("Reference to the Particle System (Visual Tornado)")]
    public ParticleSystem tornadoParticles;

    [Tooltip("Speed multiplier for the particles")]
    public float particleSpeedMultiplier = 1f;

    [Header("Debug")]
    public bool showDebugGizmos = true;

    private void Start()
    {
        // Ensure particle system is assigned
        if (tornadoParticles == null)
        {
            tornadoParticles = GetComponent<ParticleSystem>();
        }

        if (tornadoParticles != null)
        {
            var main = tornadoParticles.main;
            main.simulationSpeed = particleSpeedMultiplier;
        }
    }

    private void FixedUpdate()
    {
        // Find all colliders within the pull radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);

        foreach (Collider col in colliders)
        {
            // Check if the object has a Rigidbody (physics object)
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                ApplyTornadoForce(rb);
            }

            // Check if the object is our ThirdPersonController (custom character)
            ThirdPersonController player = col.GetComponent<ThirdPersonController>();
            if (player != null)
            {
                // We might need to handle the player differently if they are not purely Rigidbody driven
                // For now, assuming ThirdPersonController can accept external forces via Rigidbody or custom method
                // Based on PushZone.cs, it uses ApplyExternalForce.
                
                ApplyForceToPlayer(player);
            }
        }
    }

    private void ApplyTornadoForce(Rigidbody rb)
    {
        // Calculate direction from object to tornado center
        Vector3 directionToCenter = (transform.position - rb.position).normalized;
        float distance = Vector3.Distance(transform.position, rb.position);

        // Normalize distance (0 at center, 1 at edge)
        float distanceFactor = 1f - Mathf.Clamp01(distance / pullRadius);

        // Pull towards center (stronger when closer)
        Vector3 pull = directionToCenter * pullForce * distanceFactor;

        // Lift upwards (stronger when closer)
        // We assume the tornado is vertical (Y-axis)
        Vector3 lift = Vector3.up * liftForce * distanceFactor;

        // Spin around the center
        // Cross product of Up and DirectionToCenter gives a tangent vector
        Vector3 spinDir = Vector3.Cross(Vector3.up, directionToCenter).normalized;
        Vector3 spin = spinDir * spinForce * distanceFactor;

        // Apply all forces
        rb.AddForce(pull + lift + spin, ForceMode.Force);
    }

    private void ApplyForceToPlayer(ThirdPersonController player)
    {
        Vector3 directionToCenter = (transform.position - player.transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        float distanceFactor = 1f - Mathf.Clamp01(distance / pullRadius);

        Vector3 pull = directionToCenter * pullForce * distanceFactor;
        Vector3 lift = Vector3.up * liftForce * distanceFactor;
        
        // Spin direction
        Vector3 spinDir = Vector3.Cross(Vector3.up, directionToCenter).normalized;
        Vector3 spin = spinDir * spinForce * distanceFactor;

        Vector3 totalForce = pull + lift + spin;

        // Use the method found in PushZone.cs
        player.ApplyExternalForce(totalForce * Time.fixedDeltaTime, false);
    }

    private void OnDrawGizmos()
    {
        if (showDebugGizmos)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, pullRadius);
        }
    }
}
