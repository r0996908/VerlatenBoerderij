using UnityEngine;

// Basisklasse voor alle enemies (Overerving + Polymorfisme)
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected Transform playerTransform;

    protected virtual void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
    }

    protected abstract void HandleBehaviour();
}
