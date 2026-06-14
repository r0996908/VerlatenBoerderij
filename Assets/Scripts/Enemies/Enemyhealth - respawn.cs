using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Respawn Instellingen")]
    public Vector3[] spawnPunten; 
    
    [Header("Licht Instellingen")]
    public float smeltTijd = 2.0f; 

    [Header("Dood & Pijn Effecten")]
    [SerializeField] private AudioClip pijnSchreeuw;        // Jouw .mp3/.wav schreeuwgeluid
    [SerializeField] private ParticleSystem doodParticles;   // Jouw Particle System prefab

    private Coroutine smeltCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zaklamp"))
        {
            if (smeltCoroutine == null)
            {
                smeltCoroutine = StartCoroutine(SmeltTimer());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zaklamp"))
        {
            if (smeltCoroutine != null)
            {
                StopCoroutine(smeltCoroutine);
                smeltCoroutine = null;
                Debug.Log("Licht weg! Zombie herstelt.");
            }
        }
    }

    private IEnumerator SmeltTimer()
    {
        Debug.Log("Zombie staat in het licht... timer loopt!");
        yield return new WaitForSeconds(smeltTijd);
        RespawnEnemy();
        smeltCoroutine = null; 
    }

    private void RespawnEnemy()
    {
        // --- 1. GELUID AFSPELEN OP DE PLEK VAN DE DOOD ---
        if (pijnSchreeuw != null)
        {
            // Dit speelt de schreeuw in 3D af op de HUIDIGE positie van de zombie
            AudioSource.PlayClipAtPoint(pijnSchreeuw, transform.position);
        }

        // --- 2. PARTICLES SPAWNEN OP DE PLEK VAN DE DOOD ---
        if (doodParticles != null)
        {
            // Maak een kopie van het deeltjeseffect op de plek van de zombie
            ParticleSystem effectInstance = Instantiate(doodParticles, transform.position, transform.rotation);
            
            // Start het effect
            effectInstance.Play();
            
            // Ruim de particle-kloon na 3 seconden netjes op uit het geheugen
            Destroy(effectInstance.gameObject, 3.0f);
        }

        // --- 3. DE ZOMBIE DAADWERKELIJK VERPLAATSEN ---
        if (spawnPunten.Length > 0)
        {
            int willekeurigeIndex = Random.Range(0, spawnPunten.Length);
            transform.position = spawnPunten[willekeurigeIndex];
            Debug.Log("De zombie schreeuwde het uit en is gerespawned!");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Zombie is permanent vernietigd!");
        }
    }
}