using System.Collections; // Nodig voor de timer (IEnumerator)
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Spawn Instellingen")]
    [Tooltip("De X, Y, Z coördinaten waar je opnieuw wilt spawnen na je dood.")]
    [SerializeField] private Vector3 homeSpawnPositie = new Vector3(0f, 1f, 0f);

    [Header("UI Instellingen")]
    [Tooltip("Sleep hier jouw GameOverCanvas naartoe uit de Hierarchy.")]
    [SerializeField] private GameObject gameOverCanvas;

    [Header("Audio Instellingen")]
    [Tooltip("Het geluid dat JIJ (de speler) maakt als je doodgaat.")]
    [SerializeField] private AudioClip mijnPijnSchreeuw;

    [Header("Animatie / Vertraging")]
    [Tooltip("Hoeveel seconden moet het duren voordat het scherm komt en je respawnt?")]
    [SerializeField] private float doodVertraging = 2.5f;

    private bool isAlDood = false; // Zorgt ervoor dat de timer niet 100x tegelijk start

    private void Start()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // GEWIJZIGD: Zoekt nu naar "enemy" met een kleine letter!
        if (collision.gameObject.CompareTag("enemy") && !isAlDood)
        {
            StartCoroutine(DoodSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // GEWIJZIGD: Zoekt nu naar "enemy" met een kleine letter!
        if (other.CompareTag("enemy") && !isAlDood)
        {
            StartCoroutine(DoodSequence());
        }
    }

    // De timer-functie die zorgt voor de spanning
    private IEnumerator DoodSequence()
    {
        isAlDood = true; 
        Debug.Log("Speler is gegrepen door een enemy! Schreeuw start...");

        // 1. Speel DIRECT de schreeuw af
        if (mijnPijnSchreeuw != null)
        {
            AudioSource.PlayClipAtPoint(mijnPijnSchreeuw, transform.position);
        }

        // 2. Zet de speler direct stil zodat je niet kunt weglopen tijdens het sterven
        CharacterController cc = GetComponentInParent<CharacterController>();
        if (cc == null) cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // 3. WACHTEN: Unity wacht nu een paar seconden (bijv. 2.5) voor de horror-vibe
        yield return new WaitForSeconds(doodVertraging);

        // 4. PAS NU komt het Game Over scherm tevoorschijn
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;                  
        }

        // 5. Teleporteer de speler naar de spawnpositie
        transform.position = homeSpawnPositie;
        Physics.SyncTransforms(); // Dwing Unity om de positie NU te updaten

        // Als de speler direct weer mag lopen na de respawn, haal dan de '//' hieronder weg:
        // if (cc != null) cc.enabled = true; 
        
        isAlDood = false; 
    }
}
