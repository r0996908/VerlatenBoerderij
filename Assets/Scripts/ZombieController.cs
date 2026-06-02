using UnityEngine;

// ZombieController.cs
// Laat een Zombie langzaam de speler achtervolgen en speelt de juiste animatie af.
// Gebaseerd op de leerstof van Experience Development 2:
//   - Vector-aftrekking om richting naar speler te berekenen (Les 4)
//   - .normalized om een richting met lengte 1 te bekomen (Les 4)
//   - Rigidbody voor gravity en physics (Les 1/3)
//   - Animator aansturen via SetBool (Les 3)
//   - GetComponent om componenten op te halen (Les 1)
//   - [SerializeField] voor private variabelen in Inspector (Les 7/8)
//   - Tags om de speler automatisch te vinden (Les 3)
//   - Encapsulatie: variabelen zijn private (Les 6/7)
//
// SETUP in Unity:
//   1. Voeg een Rigidbody toe aan Crazyman via Add Component
//   2. Zet bij Rigidbody > Constraints: Freeze Rotation X, Y, Z aan
//   3. Stel zombie_controller in als Controller bij de Animator component
//   4. Sleep je Player naar het "Player Transform" veld in de Inspector
//      OF zorg dat je speler de tag "Player" heeft

public class ZombieController : MonoBehaviour
{
    // --- BEWEGING ---

    [SerializeField]
    // Hoe snel de zombie beweegt, in Unity units per seconde.
    // Private + [SerializeField]: zichtbaar in Inspector maar afgeschermd (encapsulatie, Les 6/7).
    private float moveSpeed = 1f;

    [SerializeField]
    // Minimale afstand tot de speler waarbij de zombie stopt met bewegen.
    // Zo loopt de zombie niet door de speler heen.
    private float stoppingDistance = 1.5f;

    // --- SPELER REFERENTIE ---

    [SerializeField]
    // Referentie naar het Transform van de speler.
    // Sleep je Player hierheen in de Inspector, of laat leeg voor automatisch zoeken via tag.
    private Transform playerTransform;

    // --- INTERNE COMPONENTEN ---

    // Referentie naar de Rigidbody van de zombie.
    // Wordt automatisch opgehaald in Start() via GetComponent. (Les 1)
    private Rigidbody m_Rigidbody;

    // Referentie naar de Animator van de zombie.
    // Wordt gebruikt om te wisselen tussen idle en walk animatie. (Les 3)
    private Animator m_Animator;

    // Start() wordt éénmalig aangeroepen bij het begin. (Les 1 - Order of execution)
    private void Start()
    {
        // Haal de Rigidbody op van dit GameObject. (Les 1 - GetComponent)
        m_Rigidbody = GetComponent<Rigidbody>();

        if (m_Rigidbody == null)
        {
            Debug.LogWarning("ZombieController: geen Rigidbody gevonden op " + gameObject.name
                + ". Voeg een Rigidbody toe via Add Component en zet Freeze Rotation X/Y/Z aan.");
        }

        // Haal de Animator op van dit GameObject. (Les 3 - Animator)
        // De Animator stuurt de animaties aan op basis van de isWalking parameter.
        m_Animator = GetComponent<Animator>();

        if (m_Animator == null)
        {
            Debug.LogWarning("ZombieController: geen Animator gevonden op " + gameObject.name
                + ". Zorg dat er een Animator component op Crazyman staat met zombie_controller.");
        }

        // Als er geen speler ingesteld is via de Inspector,
        // zoek dan automatisch naar een GameObject met de tag "Player". (Les 3 - Tags)
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");

            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("ZombieController: geen GameObject met tag 'Player' gevonden. "
                    + "Sleep je Player naar het 'Player Transform' veld in de Inspector, "
                    + "of voeg de tag 'Player' toe aan je speler.");
            }
        }
    }

    // FixedUpdate() wordt gebruikt voor physics-gebaseerde beweging. (Les 3 - Rigidbody)
    // Loopt op een vast interval, wat stabieler is voor physics berekeningen.
    private void FixedUpdate()
    {
        // Stop als er geen spelerreferentie of Rigidbody is.
        if (playerTransform == null || m_Rigidbody == null) return;

        // Bereken de richting van de zombie naar de speler via vectoraftrekking. (Les 4)
        Vector3 directionToPlayer = playerTransform.position - transform.position;

        // Zet y op 0 zodat de zombie niet omhoog/omlaag beweegt.
        directionToPlayer.y = 0f;

        // Bereken de afstand tot de speler (lengte van de vector).
        float distanceToPlayer = directionToPlayer.magnitude;

        // Controleer of de zombie dicht genoeg bij de speler is om te stoppen.
        if (distanceToPlayer > stoppingDistance)
        {
            // .normalized maakt de vector lengte 1 zodat de snelheid
            // constant blijft ongeacht de afstand tot de speler. (Les 4)
            Vector3 moveDirection = directionToPlayer.normalized;

            // Beweeg via Rigidbody velocity zodat gravity actief blijft. (Les 3)
            m_Rigidbody.linearVelocity = new Vector3(
                moveDirection.x * moveSpeed,    // horizontale beweging richting speler
                m_Rigidbody.linearVelocity.y,   // verticale snelheid onveranderd (gravity)
                moveDirection.z * moveSpeed     // horizontale beweging richting speler
            );

            // Draai de zombie vloeiend richting de speler. (Les 4)
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

            // Zet de walk animatie aan via de isWalking Bool parameter. (Les 3 - Animator)
            if (m_Animator != null)
            {
                m_Animator.SetBool("isWalking", true);
            }
        }
        else
        {
            // Zombie is dicht genoeg bij de speler, stop met bewegen.
            m_Rigidbody.linearVelocity = new Vector3(0f, m_Rigidbody.linearVelocity.y, 0f);

            // Zet de idle animatie aan door isWalking op false te zetten. (Les 3 - Animator)
            if (m_Animator != null)
            {
                m_Animator.SetBool("isWalking", false);
            }
        }
    }
}
