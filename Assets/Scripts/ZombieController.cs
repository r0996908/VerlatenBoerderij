using UnityEngine;

public class ZombieController : MonoBehaviour
{
    // --- BEWEGING ---
    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private float stoppingDistance = 1.5f;

    // --- SPELER ---
    [SerializeField]
    private Transform playerTransform;

    // --- COMPONENTEN ---
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;

    // --- AUDIO ---
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] zombieSounds;

    [SerializeField]
    private float minSoundDelay = 3f;

    [SerializeField]
    private float maxSoundDelay = 8f;

    private float nextSoundTime;

    private void Start()
    {
        // AUDIO
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        nextSoundTime = Time.time + Random.Range(minSoundDelay, maxSoundDelay);

        // COMPONENTEN
        m_Rigidbody = GetComponent<Rigidbody>();

        if (m_Rigidbody == null)
        {
            Debug.LogWarning("No Rigidbody found");
        }

        m_Animator = GetComponent<Animator>();

        if (m_Animator == null)
        {
            Debug.LogWarning("No Animator found");
        }

        // PLAYER AUTO FIND
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");

            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null || m_Rigidbody == null) return;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0f;

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > stoppingDistance)
        {
            Vector3 moveDirection = directionToPlayer.normalized;

            m_Rigidbody.linearVelocity = new Vector3(
                moveDirection.x * moveSpeed,
                m_Rigidbody.linearVelocity.y,
                moveDirection.z * moveSpeed
            );

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

            if (m_Animator != null)
                m_Animator.SetBool("isWalking", true);
        }
        else
        {
            m_Rigidbody.linearVelocity = new Vector3(0f, m_Rigidbody.linearVelocity.y, 0f);

            if (m_Animator != null)
                m_Animator.SetBool("isWalking", false);
        }

        HandleSounds();
    }

    private void HandleSounds()
    {
        if (audioSource == null || zombieSounds.Length == 0) return;

        if (Time.time >= nextSoundTime)
        {
            audioSource.PlayOneShot(zombieSounds[Random.Range(0, zombieSounds.Length)]);
            nextSoundTime = Time.time + Random.Range(minSoundDelay, maxSoundDelay);
        }
    }
}