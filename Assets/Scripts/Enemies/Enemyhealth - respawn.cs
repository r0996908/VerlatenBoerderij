using UnityEngine;
using System.Collections;


// Beheert de gezondheid van een enemy.
// Enemy smelt wanneer hij in zaklamp-licht staat
// Respawn op willekeurige spawnpunten

public class EnemyHealth : MonoBehaviour
{
    [Header("Respawn Instellingen")]
    public Vector3[] spawnPunten;
    // Lijst van mogelijke respawn locaties

    [Header("Licht Instellingen")]
    public float smeltTijd = 2.0f;
    // Hoe lang de enemy in het licht moet staan om te smelten

    [Header("Dood & Pijn Effecten")]
    [SerializeField] private AudioClip pijnSchreeuw;
    // Geluid bij dood

    [SerializeField] private ParticleSystem doodParticles;
    // Particles bij dood

    private Coroutine smeltCoroutine;
    // Houdt bij of de smelt-timer loopt

private void OnTriggerEnter(Collider other)
{
    // We kijken nu of het object dat ons raakt de Tag "Zaklamp" heeft
    if (other.CompareTag("Zaklamp"))
    {
        RespawnEnemy();
    }
}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Zaklamp"))
        {
            if (smeltCoroutine != null)
            {
                StopCoroutine(smeltCoroutine);
                smeltCoroutine = null;
            }
        }
    }

    private IEnumerator SmeltTimer()
    {
        yield return new WaitForSeconds(smeltTijd);
        RespawnEnemy();
        smeltCoroutine = null;
    }

    private void RespawnEnemy()
    {
        // Speel geluid
        if (pijnSchreeuw != null)
            AudioSource.PlayClipAtPoint(pijnSchreeuw, transform.position);

        // Particles
        if (doodParticles != null)
        {
            ParticleSystem effect = Instantiate(doodParticles, transform.position, transform.rotation);
            effect.Play();
            Destroy(effect.gameObject, 3f);
        }

        // Respawn op random punt
        if (spawnPunten.Length > 0)
        {
            int index = Random.Range(0, spawnPunten.Length);
            transform.position = spawnPunten[index];
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
