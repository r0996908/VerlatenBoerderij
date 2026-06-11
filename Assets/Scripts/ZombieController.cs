using UnityEngine;


// Stuurt het gedrag van de zombie aan.
// Erft van EnemyBase
// Volgt de speler traag
// Speelt willekeurige geluiden af afhankelijk van afstand

public class ZombieController : EnemyBase
{
    [Header("Bewegingsinstellingen")]
    [SerializeField] private float stoppingDistance = 1.5f;
    // Afstand waarop de zombie stopt met bewegen

    [Header("Geluid instellingen")]
    [SerializeField] private float maxHearDistance = 20f;
    // Maximale afstand waarop de zombie hoorbaar is

    [SerializeField] private AudioSource audioSource;
    // Bron die de geluiden afspeelt

    [SerializeField] private AudioClip[] zombieSounds;
    // Lijst van mogelijke zombie-geluiden

    [SerializeField] private float minSoundDelay = 3f;
    [SerializeField] private float maxSoundDelay = 8f;
    // Willekeurige vertraging tussen geluiden

    private float nextSoundTime;
    // Tijdstip waarop het volgende geluid mag afgespeeld worden

    private Rigidbody m_Rigidbody;
    // Rigidbody voor physics-beweging

    private Animator m_Animator;
    // Animator voor loop-animatie

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
        // Physics-gebaseerde beweging in FixedUpdate
        HandleBehaviour();
    }

 
    // Bepaalt het gedrag van de zombie op basis van afstand tot de speler.

    protected override void HandleBehaviour()
    {
        if (playerTransform == null || m_Rigidbody == null) return;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0f;

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > stoppingDistance)
        {
            Vector3 moveDirection = directionToPlayer.normalized;

            // Beweging via linearVelocity (Unity 6)
            m_Rigidbody.linearVelocity = new Vector3(
                moveDirection.x * moveSpeed,
                m_Rigidbody.linearVelocity.y,
                moveDirection.z * moveSpeed
            );

            // Draai naar de speler
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

            if (m_Animator != null)
                m_Animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop horizontale beweging
            m_Rigidbody.linearVelocity = new Vector3(0f, m_Rigidbody.linearVelocity.y, 0f);

            if (m_Animator != null)
                m_Animator.SetBool("isWalking", false);
        }

        HandleSounds(distanceToPlayer);
    }


    // Stuurt het geluid van de zombie aan op basis van afstand en tijd.

    private void HandleSounds(float distance)
    {
        if (audioSource == null || zombieSounds.Length == 0) return;
        if (distance > maxHearDistance) return;

        // Volume afhankelijk van afstand
        float volume = 1f - Mathf.Clamp01(distance / maxHearDistance);
        audioSource.volume = volume;

        // Speel een willekeurig geluid af met een willekeurige vertraging
        if (Time.time >= nextSoundTime)
        {
            AudioClip clip = zombieSounds[Random.Range(0, zombieSounds.Length)];
            audioSource.PlayOneShot(clip);

            nextSoundTime = Time.time + Random.Range(minSoundDelay, maxSoundDelay);
        }
    }
}
