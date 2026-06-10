using UnityEngine;

public class ZombieController : MonoBehaviour
{
    // --- BEWEGING ---
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float stoppingDistance = 1.5f;

    // --- SPELER ---
    [SerializeField] private Transform playerTransform;

    // --- AUDIO SETTINGS ---
    [SerializeField] private float maxHearDistance = 20f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] zombieSounds;

    [SerializeField] private float minSoundDelay = 3f;
    [SerializeField] private float maxSoundDelay = 8f;

    private float nextSoundTime;

    // --- COMPONENTEN ---
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();

        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
                playerTransform = playerObject.transform;
        }

        nextSoundTime = Time.time + Random.Range(minSoundDelay, maxSoundDelay);
    }

    private void FixedUpdate()
    {
        if (playerTransform == null || m_Rigidbody == null) return;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0f;

        float distanceToPlayer = directionToPlayer.magnitude;

        // --- BEWEGING ---
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

        HandleSounds(distanceToPlayer);
    }

    private void HandleSounds(float distance)
    {
        if (audioSource == null || zombieSounds.Length == 0) return;

        // ❌ buiten range = geen geluid
        if (distance > maxHearDistance) return;

        // volume gebaseerd op afstand (belangrijk!)
        float volume = 1f - Mathf.Clamp01(distance / maxHearDistance);
        audioSource.volume = volume;

        if (Time.time >= nextSoundTime)
        {
            AudioClip clip = zombieSounds[Random.Range(0, zombieSounds.Length)];
            audioSource.PlayOneShot(clip);

            nextSoundTime = Time.time + Random.Range(minSoundDelay, maxSoundDelay);
        }
    }
}