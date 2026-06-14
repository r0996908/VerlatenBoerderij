using UnityEngine;

/// <summary>
// Basisklasse voor alle vijanden in het spel.
// Toont child and parent relatie (GhostController en ZombieController erven hiervan)
//Zorgt dat elke enemy automatisch de speler vindt
//Dwingt af dat elke enemy zijn eigen gedrag implementeert (abstracte methode)

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Bewegingsinstellingen")]
    [SerializeField] protected float moveSpeed = 2f;
    // Snelheid waarmee de enemy beweegt (encapsulatie + Inspector zichtbaar)

    [Header("Speler Referentie")]
    [SerializeField] protected Transform playerTransform;
    // Wordt automatisch ingevuld via tag "Player" als dit niet manueel is ingesteld

    // Start wordt uitgevoerd wanneer het object geactiveerd wordt.
    // Hier zoeken we automatisch de speler op basis van de Player-tag.
    protected virtual void Start()
    {
        // Als de speler niet manueel werd toegewezen in de Inspector
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
                playerTransform = player.transform;
        }
    }

    // Abstracte methode die verplicht wordt gemplementeerd in elke subklasse.
    // Elke enemy heeft zijn eigen gedrag.
    protected abstract void HandleBehaviour();
}
