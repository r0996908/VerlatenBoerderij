// GhostController.cs
using UnityEngine;

// Stuurt het gedrag van een geest aan.
// Erft van EnemyBase (overerving)
// Kan zweven, speler volgen en aanvallen

public class GhostController : EnemyBase
{
    [Header("Wander instellingen")]
    [SerializeField] private float minWanderTime = 2f;
    [SerializeField] private float maxWanderTime = 5f;

    [Header("Detectie en aanval")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 10;

    private Vector3 wanderDirection;
    // Huidige willekeurige richting waarin de geest rondloopt

    private bool isAttacking = false;
    // Flag om te voorkomen dat meerdere aanvallen tegelijk starten

    protected override void Start()
    {
        base.Start(); // Zorgt dat playerTransform gezet wordt
        ChooseNewWanderDirection(); // Start met een willekeurige richting
    }

    private void Update()
    {
        // Elke frame het gedrag van de geest uitvoeren
        HandleBehaviour();
    }

    // Bepaalt wat de geest doet op basis van afstand tot de speler.
    protected override void HandleBehaviour()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRadius)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            FollowPlayer();
        }
        else
        {
            Wander();
        }
    }


    // Laat de geest in een willekeurige richting rondlopen.

    private void Wander()
    {
        transform.Translate(wanderDirection * moveSpeed * Time.deltaTime, Space.World);
    }


    // Kiest een nieuwe willekeurige richting en plant zichzelf opnieuw in via Invoke.

    private void ChooseNewWanderDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        wanderDirection = new Vector3(randomX, 0f, randomZ).normalized;

        // Draai de geest in de nieuwe richting
        if (wanderDirection != Vector3.zero)
            transform.forward = wanderDirection;

        // Kies een willekeurige tijd tot de volgende richtingsverandering
        float wanderTime = Random.Range(minWanderTime, maxWanderTime);
        Invoke(nameof(ChooseNewWanderDirection), wanderTime);
    }


    // Laat de geest naar de speler toe bewegen.

    private void FollowPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0f; // Geen verticale beweging

        // Verplaats de geest richting speler
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Draai de geest vloeiend naar de speler
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // Start een aanval op de speler.

    private void AttackPlayer()
    {
        // Blijf naar de speler toe bewegen tijdens aanval
        FollowPlayer();

        if (!isAttacking)
            StartCoroutine(PerformAttack());
    }


    // Coroutine die een aanval uitvoert met een cooldown.

    private System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Hier zou je later echte damage kunnen toepassen op een health-systeem
        Debug.Log("Ghost valt aan voor " + attackDamage + " schade!");

        // Wacht tot de cooldown voorbij is
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }


    // Teken de detectie- en aanvalsradius in de editor voor debugging.

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
