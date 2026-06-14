using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Respawn Instellingen")]
    public Vector3[] spawnPunten; 
    
    [Header("Licht Instellingen")]
    public float smeltTijd = 2.0f; 

    [Header("Dood & Pijn Effecten")]
    [SerializeField] private AudioClip pijnSchreeuw;        
    [SerializeField] private ParticleSystem doodParticles;   

    private Coroutine smeltCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        // LET OP: "Zaklamp" moet hier verplicht tussen aanhalingstekens staan!
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
        // Geluid afspelen (als het vakje is ingevuld)
        if (pijnSchreeuw != null)
        {
            AudioSource.PlayClipAtPoint(pijnSchreeuw, transform.position);
        }

        // Particles spawnen (als het vakje is ingevuld)
        if (doodParticles != null)
        {
            ParticleSystem effectInstance = Instantiate(doodParticles, transform.position, transform.rotation);
            effectInstance.Play();
            Destroy(effectInstance.gameObject, 3.0f);
        }

        // Verplaats de zombie naar een willekeurig spawnpunt
        if (spawnPunten.Length > 0)
        {
            int willekeurigeIndex = Random.Range(0, spawnPunten.Length);
            transform.position = spawnPunten[willekeurigeIndex];
            Debug.Log("De zombie is succesvol gerespawned!");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Geen spawnpunten gevonden, zombie vernietigd!");
        }
    }
}