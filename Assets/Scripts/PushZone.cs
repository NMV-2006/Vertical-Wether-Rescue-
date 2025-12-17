using UnityEngine;

/// <summary>
/// What happens to player velocity when exiting the zone
/// </summary>
public enum ExitBehavior
{
    KeepVelocity,        // Player keeps all velocity (continues moving)
    DampenVelocity,      // Gradually reduce velocity (smooth stop)
    ClearPushDirection,  // Only remove velocity in push direction
    ClearAllVelocity     // Immediately stop all movement
}

/// <summary>
/// Push Zone - Continuously applies force to the player while they're inside the zone
/// Perfect for wind tunnels, updrafts, geysers, or any pushing mechanic
/// </summary>
public class PushZone : MonoBehaviour
{
    [Header("Push Settings")]
    [Tooltip("Direction and strength of the push force")]
    public Vector3 pushForce = new Vector3(0, 10, 0);
    
    [Tooltip("How the force is applied")]
    public ForceMode forceMode = ForceMode.Acceleration;
    
    [Tooltip("Multiply force over time (false = constant force)")]
    public bool accelerateOverTime = false;
    
    [Tooltip("Maximum force multiplier when accelerating")]
    public float maxForceMultiplier = 3f;
    
    [Tooltip("How fast the force accelerates (seconds to reach max)")]
    public float accelerationTime = 2f;

    [Header("Force Application")]
    [Tooltip("Override player velocity (true) or add to it (false)")]
    public bool overrideVelocity = false;
    
    [Tooltip("Apply force every frame (smooth) or at intervals")]
    public bool continuousForce = true;
    
    [Tooltip("If not continuous, how often to apply force (seconds)")]
    public float forceInterval = 0.1f;
    
    private float lastForceTime = 0f;

    [Header("Player Control")]
    [Tooltip("Disable player movement while in zone")]
    public bool disablePlayerControl = false;
    
    [Tooltip("Reduce player control (0 = no control, 1 = full control)")]
    [Range(0f, 1f)]
    public float controlMultiplier = 0.5f;
    
    [Tooltip("Reset jump count when entering zone")]
    public bool resetJumpOnEnter = true;
    
    [Header("Exit Behavior")]
    [Tooltip("What happens to velocity when player exits the zone")]
    public ExitBehavior exitBehavior = ExitBehavior.DampenVelocity;
    
    [Tooltip("How much velocity to clear (0 = none, 1 = all)")]
    [Range(0f, 1f)]
    public float velocityClearAmount = 0.7f;
    
    [Tooltip("Only clear velocity in the push direction (keeps other movement)")]
    public bool onlyClearPushDirection = true;

    [Header("Visual Feedback")]
    [Tooltip("Particle system to show wind/force direction")]
    public ParticleSystem windEffect;
    
    [Tooltip("Play particles only when player is inside")]
    public bool particlesOnlyWhenOccupied = false;
    
    [Tooltip("Optional audio source for wind sound")]
    public AudioSource windSound;
    
    [Tooltip("Fade audio based on player presence")]
    public bool fadeAudio = true;
    
    [Tooltip("Audio fade speed")]
    public float audioFadeSpeed = 2f;

    [Header("Debug")]
    [Tooltip("Show debug messages in console")]
    public bool showDebugLogs = true;
    
    [Tooltip("Show force direction in scene view")]
    public bool showGizmos = true;

    // Private variables
    private ThirdPersonController playerInZone = null;
    private float timeInZone = 0f;
    private float currentForceMultiplier = 1f;
    private float targetAudioVolume = 0f;
    private float originalAudioVolume = 1f;

    void Start()
    {
        // Setup particle system
        if (windEffect != null)
        {
            if (particlesOnlyWhenOccupied)
            {
                windEffect.Stop();
            }
            else
            {
                windEffect.Play();
            }
        }
        
        // Setup audio
        if (windSound != null)
        {
            originalAudioVolume = windSound.volume;
            if (fadeAudio && particlesOnlyWhenOccupied)
            {
                windSound.volume = 0f;
            }
        }
        
        if (showDebugLogs)
        {
            Debug.Log($"[PushZone] {gameObject.name} initialized with force: {pushForce}");
        }
    }

    void Update()
    {
        // Apply force to player if they're in the zone
        if (playerInZone != null)
        {
            // Update time in zone
            timeInZone += Time.deltaTime;
            
            // Calculate force multiplier if accelerating
            if (accelerateOverTime)
            {
                float t = Mathf.Clamp01(timeInZone / accelerationTime);
                currentForceMultiplier = Mathf.Lerp(1f, maxForceMultiplier, t);
            }
            else
            {
                currentForceMultiplier = 1f;
            }
            
            // Apply force based on mode
            if (continuousForce)
            {
                ApplyPushForce();
            }
            else
            {
                // Apply force at intervals
                if (Time.time >= lastForceTime + forceInterval)
                {
                    ApplyPushForce();
                    lastForceTime = Time.time;
                }
            }
            
            targetAudioVolume = originalAudioVolume;
        }
        else
        {
            targetAudioVolume = 0f;
        }
        
        // Fade audio
        if (windSound != null && fadeAudio)
        {
            windSound.volume = Mathf.Lerp(windSound.volume, targetAudioVolume, audioFadeSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (showDebugLogs)
        {
            Debug.Log($"[PushZone] Object entered: {other.gameObject.name}");
        }
        
        // Check if it's the player
        ThirdPersonController player = other.GetComponent<ThirdPersonController>();
        if (player != null)
        {
            playerInZone = player;
            timeInZone = 0f;
            currentForceMultiplier = 1f;
            
            if (showDebugLogs)
            {
                Debug.Log($"[PushZone] Player entered zone! Push force: {pushForce}");
            }
            
            // Reset jump count if enabled
            if (resetJumpOnEnter)
            {
                player.ResetJumpCount();
            }
            
            // Start particle effect
            if (windEffect != null && particlesOnlyWhenOccupied)
            {
                windEffect.Play();
            }
            
            // Start audio
            if (windSound != null && !windSound.isPlaying)
            {
                windSound.Play();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (showDebugLogs)
        {
            Debug.Log($"[PushZone] Object exited: {other.gameObject.name}");
        }
        
        // Check if it's the player leaving
        ThirdPersonController player = other.GetComponent<ThirdPersonController>();
        if (player != null && player == playerInZone)
        {
            // Handle exit behavior
            HandleExitBehavior(player);
            
            playerInZone = null;
            timeInZone = 0f;
            
            if (showDebugLogs)
            {
                Debug.Log($"[PushZone] Player exited zone");
            }
            
            // Stop particle effect
            if (windEffect != null && particlesOnlyWhenOccupied)
            {
                windEffect.Stop();
            }
        }
    }

    /// <summary>
    /// Apply the push force to the player
    /// </summary>
    void ApplyPushForce()
    {
        if (playerInZone == null) return;
        
        // Calculate final force with multiplier
        Vector3 finalForce = pushForce * currentForceMultiplier;
        
        // Apply force based on mode
        if (continuousForce)
        {
            // For continuous force, scale by deltaTime for frame-rate independence
            finalForce *= Time.deltaTime;
        }
        
        // Apply the force to the player
        playerInZone.ApplyExternalForce(finalForce, overrideVelocity);
        
        if (showDebugLogs && Time.frameCount % 60 == 0) // Log every 60 frames to avoid spam
        {
            Debug.Log($"[PushZone] Applying force: {finalForce} (multiplier: {currentForceMultiplier:F2})");
        }
    }
    
    /// <summary>
    /// Handle what happens to player velocity when exiting the zone
    /// </summary>
    void HandleExitBehavior(ThirdPersonController player)
    {
        if (exitBehavior == ExitBehavior.KeepVelocity)
        {
            // Do nothing - player keeps all velocity
            return;
        }
        
        // Get the current velocity from the player
        Vector3 currentVelocity = player.GetVelocity();
        
        if (exitBehavior == ExitBehavior.ClearAllVelocity)
        {
            // Clear all velocity immediately
            player.ApplyExternalForce(Vector3.zero, true);
            
            if (showDebugLogs)
            {
                Debug.Log($"[PushZone] Cleared all velocity on exit");
            }
        }
        else if (exitBehavior == ExitBehavior.DampenVelocity)
        {
            Vector3 newVelocity;
            
            if (onlyClearPushDirection)
            {
                // Only dampen velocity in the push direction
                Vector3 pushDirection = pushForce.normalized;
                float velocityInPushDirection = Vector3.Dot(currentVelocity, pushDirection);
                
                // Remove the push direction component
                Vector3 pushVelocity = pushDirection * velocityInPushDirection;
                Vector3 otherVelocity = currentVelocity - pushVelocity;
                
                // Dampen only the push velocity
                pushVelocity *= (1f - velocityClearAmount);
                
                newVelocity = otherVelocity + pushVelocity;
            }
            else
            {
                // Dampen all velocity
                newVelocity = currentVelocity * (1f - velocityClearAmount);
            }
            
            player.ApplyExternalForce(newVelocity, true);
            
            if (showDebugLogs)
            {
                Debug.Log($"[PushZone] Dampened velocity on exit: {currentVelocity} -> {newVelocity}");
            }
        }
        else if (exitBehavior == ExitBehavior.ClearPushDirection)
        {
            // Only clear velocity in the push direction
            Vector3 pushDirection = pushForce.normalized;
            float velocityInPushDirection = Vector3.Dot(currentVelocity, pushDirection);
            
            // Remove the push direction component
            Vector3 pushVelocity = pushDirection * velocityInPushDirection;
            Vector3 newVelocity = currentVelocity - pushVelocity;
            
            player.ApplyExternalForce(newVelocity, true);
            
            if (showDebugLogs)
            {
                Debug.Log($"[PushZone] Cleared push direction velocity: {currentVelocity} -> {newVelocity}");
            }
        }
    }

    /// <summary>
    /// Get the current force being applied
    /// </summary>
    public Vector3 GetCurrentForce()
    {
        return pushForce * currentForceMultiplier;
    }

    /// <summary>
    /// Check if player is currently in the zone
    /// </summary>
    public bool IsPlayerInZone()
    {
        return playerInZone != null;
    }

    /// <summary>
    /// Get how long the player has been in the zone
    /// </summary>
    public float GetTimeInZone()
    {
        return timeInZone;
    }

    /// <summary>
    /// Manually set the push force (for dynamic zones)
    /// </summary>
    public void SetPushForce(Vector3 newForce)
    {
        pushForce = newForce;
    }

    /// <summary>
    /// Draw gizmos in the scene view
    /// </summary>
    void OnDrawGizmos()
    {
        if (!showGizmos) return;
        
        // Draw the zone bounds
        Gizmos.color = new Color(0.5f, 1f, 0.5f, 0.3f);
        
        // Get the collider to show the zone area
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
        }
        
        // Draw force direction arrow
        Gizmos.color = Color.cyan;
        Vector3 start = transform.position;
        Vector3 direction = pushForce.normalized;
        float magnitude = Mathf.Min(pushForce.magnitude * 0.2f, 5f); // Scale for visibility
        Vector3 end = start + direction * magnitude;
        
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(end, 0.3f);
        
        // Draw multiple arrows to show force field
        DrawArrow(start, direction, magnitude);
    }

    /// <summary>
    /// Draw gizmos when selected
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        
        // Draw a more detailed visualization
        Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
        Vector3 direction = pushForce.normalized;
        float magnitude = Mathf.Min(pushForce.magnitude * 0.2f, 5f);
        
        // Draw multiple arrows to show the force field
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3 offset = new Vector3(x, y, z) * 0.5f;
                    Vector3 start = transform.position + offset;
                    DrawArrow(start, direction, magnitude * 0.5f);
                }
            }
        }
    }

    /// <summary>
    /// Helper method to draw an arrow
    /// </summary>
    void DrawArrow(Vector3 start, Vector3 direction, float length)
    {
        Vector3 end = start + direction * length;
        Gizmos.DrawLine(start, end);
        
        // Draw arrowhead
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + 20, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - 20, 0) * Vector3.forward;
        
        Gizmos.DrawLine(end, end + right * (length * 0.3f));
        Gizmos.DrawLine(end, end + left * (length * 0.3f));
    }
}
