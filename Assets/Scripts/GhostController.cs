using UnityEngine;
using UnityEngine.UIElements;

// GhostController.cs
// Dit script combineert het wandelgedrag (Les 2 - InvokeRepeating) met
// het volgen van de speler (Les 4 - vectorberekening) en een aanvalssysteem.
// De ghost heeft drie staten: Wandelen, Volgen, en Aanvallen.

public class GhostController : MonoBehaviour
{
    // ---- BEWEGING ----

    [SerializeField] private float moveSpeed = 2f;          // Snelheid van de ghost
    [SerializeField] private float minWanderTime = 2f;      // Minimale tijd in één richting (Les 2)
    [SerializeField] private float maxWanderTime = 5f;      // Maximale tijd in één richting (Les 2)

    // ---- SPELER DETECTIE ----

    [SerializeField] private float detectionRadius = 10f;   // Radius waarbinnen de ghost de speler ziet
    [SerializeField] private float attackRadius = 2f;       // Radius waarbinnen de ghost aanvalt
    [SerializeField] private Transform playerTransform;     // Referentie naar de speler (Les 4)

    // ---- AANVAL ----

    [SerializeField] private float attackCooldown = 1f;     // Tijd tussen twee aanvallen
    [SerializeField] private int attackDamage = 10;         // Schade per aanval

    // ---- PRIVÉ VARIABELEN ----
    // Private variabelen zijn niet zichtbaar buiten dit script (Les 6/7 - encapsulatie)

    private Vector3 _wanderDirection;       // Huidige wandelrichting
    private bool _isAttacking = false;      // Bijhouden of de ghost al aan het aanvallen is

    // Start wordt één keer opgeroepen bij het begin van het spel (Les 1)
    private void Start()
    {
        // Als er geen speler gekoppeld is via de Inspector, zoek hem automatisch via tag (Les 3)
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        // Kies een eerste wandelrichting via InvokeRepeating (Les 2)
        ChooseNewWanderDirection();
    }

    // Update wordt elke frame opgeroepen (Les 1)
    private void Update()
    {
        // Als er geen speler is, doe niets
        if (playerTransform == null) return;

        // Bereken de afstand tussen de ghost en de speler (Les 4)
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRadius)
        {
            // Speler is dichtbij genoeg om aan te vallen
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            // Speler is binnen de detectieradius: volg de speler (Les 4)
            FollowPlayer();
        }
        else
        {
            // Speler is te ver weg: wandel random rond (Les 2)
            Wander();
        }
    }

    // Wandelgedrag: beweeg in een willekeurige richting (Les 2)
    private void Wander()
    {
        // Beweeg in de huidige wandelrichting via transform.Translate (Les 1)
        transform.Translate(_wanderDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    // Kies een nieuwe willekeurige wandelrichting (Les 2 - Random)
    private void ChooseNewWanderDirection()
    {
        // Willekeurige richting op de X en Z as (niet omhoog/omlaag)
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        // .normalized zorgt dat de snelheid constant blijft (Les 4)
        _wanderDirection = new Vector3(randomX, 0, randomZ).normalized;

        // Draai de ghost richting de nieuwe wandelrichting
        if (_wanderDirection != Vector3.zero)
        {
            transform.forward = _wanderDirection;
        }

        // Roep deze methode opnieuw op na een willekeurige tijd (Les 2 - Invoke)
        float wanderTime = Random.Range(minWanderTime, maxWanderTime);
        Invoke(nameof(ChooseNewWanderDirection), wanderTime);
    }

    // Volggedrag: beweeg richting de speler (Les 4 - vectorberekening)
    private void FollowPlayer()
    {
        // Bereken de richting van ghost naar speler via vectoraftrekking (Les 4)
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Zet de Y op 0 zodat de ghost niet omhoog/omlaag beweegt
        direction.y = 0;

        // Beweeg in de richting van de speler
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Draai de ghost vloeiend richting de speler via Quaternion.Slerp (Les 4)
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // Aanval: roep een coroutine op om de speler te raken (Les 4 - coroutine)
    private void AttackPlayer()
    {
        // Draai richting de speler tijdens aanval
        FollowPlayer();

        // Start de aanval alleen als die nog niet bezig is (encapsulatie - Les 6/7)
        if (!_isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    // Coroutine voor de aanval met vertraging (Les 4 - coroutine)
    private System.Collections.IEnumerator PerformAttack()
    {
        _isAttacking = true;

        // Hier kan je later schade toevoegen aan de speler
        Debug.Log("Ghost valt aan voor " + attackDamage + " schade!");

        // Wacht de cooldown tijd voor de volgende aanval (Les 4 - yield return)
        yield return new WaitForSeconds(attackCooldown);

        _isAttacking = false;
    }

    // Toon de detectie- en aanvalsradius in de Scene view als gizmos (handig voor debugging)
    private void OnDrawGizmosSelected()
    {
        // Toon detectieradius in geel
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Toon aanvalsradius in rood
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
