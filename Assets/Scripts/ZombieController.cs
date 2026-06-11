using UnityEngine;

public class ZombieController : EnemyBase
{
    [SerializeField] private float stoppingDistance = 1.5f;

    [SerializeField] private float maxHearDistance = 20f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] zombieSounds;
    [SerializeField] private float minSoundDelay = 3f;
    [SerializeField] private float maxSoundDelay = 8f;

    private float nextSoundTime;

    private Rigidbody m_Rigidbody;
    private Animator m_Animator;

    protected override void Start()
    {
        base.Start();

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        nextSoundTime = Time.time + Random.Range(minSoundDelay, maxSoundDelay);
    }

    private void FixedUpdate()
    {
        HandleBehaviour();
    }

    protected override void HandleBehaviour()
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

        HandleSounds(distanceToPlayer);
    }

    private void HandleSounds(float distance)
    {
        if (audioSource == null || zombieSounds.Length == 0) return;
        if (distance > maxHearDistance) return;

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
