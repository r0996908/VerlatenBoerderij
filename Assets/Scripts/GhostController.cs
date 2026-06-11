using UnityEngine;

public class GhostController : EnemyBase
{
    [SerializeField] private float minWanderTime = 2f;
    [SerializeField] private float maxWanderTime = 5f;

    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRadius = 2f;

    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 10;

    private Vector3 wanderDirection;
    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
        ChooseNewWanderDirection();
    }

    private void Update()
    {
        HandleBehaviour();
    }

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

    private void Wander()
    {
        transform.Translate(wanderDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    private void ChooseNewWanderDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        wanderDirection = new Vector3(randomX, 0, randomZ).normalized;

        if (wanderDirection != Vector3.zero)
            transform.forward = wanderDirection;

        float wanderTime = Random.Range(minWanderTime, maxWanderTime);
        Invoke(nameof(ChooseNewWanderDirection), wanderTime);
    }

    private void FollowPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void AttackPlayer()
    {
        FollowPlayer();

        if (!isAttacking)
            StartCoroutine(PerformAttack());
    }

    private System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;

        Debug.Log("Ghost valt aan voor " + attackDamage + " schade!");

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
