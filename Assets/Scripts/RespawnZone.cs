using UnityEngine;

/// <summary>
/// Respawn Zone - Teleports the player back to a spawn point when they touch this zone
/// Perfect for death zones, fall zones, lava, water, or out-of-bounds areas
/// </summary>
public class RespawnZone : MonoBehaviour
{
    [Header("Respawn Settings")]
    [Tooltip("Where to teleport the player (leave empty to use a spawn point tag)")]
    public Transform respawnPoint;
    
    [Tooltip("If no respawn point is set, search for GameObject with this tag")]
    public string spawnPointTag = "Respawn";
    
    [Tooltip("Reset player velocity when respawning")]
    public bool resetVelocity = true;
    
    [Tooltip("Reset player rotation when respawning")]
    public bool resetRotation = false;
    
    [Tooltip("Rotation to set when respawning (only if resetRotation is true)")]
    public Vector3 respawnRotation = Vector3.zero;

    [Header("Effects")]
    [Tooltip("Particle effect to play at respawn location")]
    public ParticleSystem respawnEffect;
    
    [Tooltip("Sound to play when respawning")]
    public AudioSource respawnSound;
    
    [Tooltip("Delay before respawning (seconds)")]
    public float respawnDelay = 0f;
    
    [Tooltip("Fade screen to black during respawn")]
    public bool fadeScreen = false;
    
    [Tooltip("Fade duration (seconds)")]
    public float fadeDuration = 0.5f;

    [Header("Visual Feedback")]
    [Tooltip("Color of the zone in scene view")]
    public Color gizmoColor = new Color(1f, 0f, 0f, 0.3f); // Red transparent
    
    [Tooltip("Show zone bounds in scene view")]
    public bool showGizmos = true;

    [Header("Debug")]
    [Tooltip("Show debug messages in console")]
    public bool showDebugLogs = true;

    private bool isRespawning = false;

    void Start()
    {
        // If no respawn point is assigned, try to find one by tag
        if (respawnPoint == null)
        {
            GameObject spawnObject = GameObject.FindGameObjectWithTag(spawnPointTag);
            if (spawnObject != null)
            {
                respawnPoint = spawnObject.transform;
                if (showDebugLogs)
                {
                    Debug.Log($"[RespawnZone] Found spawn point: {spawnObject.name}");
                }
            }
            else
            {
                Debug.LogWarning($"[RespawnZone] No respawn point assigned and couldn't find GameObject with tag '{spawnPointTag}'!");
            }
        }
        
        // Make sure this has a trigger collider
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError($"[RespawnZone] {gameObject.name} needs a Collider component!");
        }
        else if (!col.isTrigger)
        {
            Debug.LogWarning($"[RespawnZone] {gameObject.name} collider should be set to 'Is Trigger'!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isRespawning) return; // Prevent multiple respawns

        if (showDebugLogs)
        {
            Debug.Log($"[RespawnZone] Object entered: {other.gameObject.name}");
        }

        // Check if it's the player
        ThirdPersonController player = other.GetComponent<ThirdPersonController>();
        if (player != null)
        {
            if (showDebugLogs)
            {
                Debug.Log($"[RespawnZone] Player touched respawn zone! Teleporting to spawn point...");
            }

            // Respawn the player
            if (respawnDelay > 0)
            {
                StartCoroutine(RespawnWithDelay(player));
            }
            else
            {
                RespawnPlayer(player);
            }
        }
    }

    /// <summary>
    /// Respawn the player with a delay
    /// </summary>
    System.Collections.IEnumerator RespawnWithDelay(ThirdPersonController player)
    {
        isRespawning = true;
        
        // Optional: Fade out
        if (fadeScreen)
        {
            // TODO: Implement screen fade if you have a UI fade system
            yield return new WaitForSeconds(fadeDuration);
        }
        
        yield return new WaitForSeconds(respawnDelay);
        
        RespawnPlayer(player);
        
        // Optional: Fade in
        if (fadeScreen)
        {
            yield return new WaitForSeconds(fadeDuration);
        }
        
        isRespawning = false;
    }

    /// <summary>
    /// Teleport the player to the respawn point
    /// </summary>
    void RespawnPlayer(ThirdPersonController player)
    {
        if (respawnPoint == null)
        {
            Debug.LogError("[RespawnZone] Cannot respawn - no respawn point set!");
            return;
        }

        // Get the CharacterController to disable/enable it
        CharacterController controller = player.GetComponent<CharacterController>();
        
        if (controller != null)
        {
            // Disable CharacterController to allow teleportation
            controller.enabled = false;
        }

        // Teleport to respawn point
        player.transform.position = respawnPoint.position;
        
        if (showDebugLogs)
        {
            Debug.Log($"[RespawnZone] Teleported player to: {respawnPoint.position}");
        }

        // Reset rotation if enabled
        if (resetRotation)
        {
            player.transform.rotation = Quaternion.Euler(respawnRotation);
        }

        // Reset velocity if enabled
        if (resetVelocity)
        {
            player.ApplyExternalForce(Vector3.zero, true);
            if (showDebugLogs)
            {
                Debug.Log("[RespawnZone] Reset player velocity");
            }
        }

        // Re-enable CharacterController
        if (controller != null)
        {
            controller.enabled = true;
        }

        // Play respawn effect
        if (respawnEffect != null)
        {
            respawnEffect.Play();
        }

        // Play respawn sound
        if (respawnSound != null)
        {
            respawnSound.Play();
        }
    }

    /// <summary>
    /// Manually respawn a player (can be called from other scripts)
    /// </summary>
    public void ManualRespawn(ThirdPersonController player)
    {
        RespawnPlayer(player);
    }

    /// <summary>
    /// Set a new respawn point
    /// </summary>
    public void SetRespawnPoint(Transform newPoint)
    {
        respawnPoint = newPoint;
        if (showDebugLogs)
        {
            Debug.Log($"[RespawnZone] Respawn point changed to: {newPoint.name}");
        }
    }

    /// <summary>
    /// Draw gizmos in the scene view
    /// </summary>
    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        // Draw the zone bounds
        Gizmos.color = gizmoColor;
        
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            if (col is BoxCollider boxCol)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(boxCol.center, boxCol.size);
            }
            else if (col is SphereCollider sphereCol)
            {
                Gizmos.DrawSphere(transform.position + sphereCol.center, sphereCol.radius);
            }
            else if (col is CapsuleCollider capsuleCol)
            {
                Gizmos.DrawSphere(transform.position + capsuleCol.center, capsuleCol.radius);
            }
        }
    }

    /// <summary>
    /// Draw gizmos when selected
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        // Draw a line to the respawn point
        if (respawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, respawnPoint.position);
            
            // Draw the respawn point
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(respawnPoint.position, 0.5f);
            Gizmos.DrawLine(respawnPoint.position, respawnPoint.position + Vector3.up * 2f);
        }
        else
        {
            // Draw a warning if no respawn point
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}
