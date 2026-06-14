using System.Collections;
using UnityEngine;


// Beheert de gezondheid van de speler.
// Speler sterft wanneer een enemy hem raakt
// Zaklamp wordt genegeerd (enemy mag zaklamp raken zonder speler te doden)
// Toont Game Over UI

public class PlayerHealth : MonoBehaviour
{
    [Header("UI Instellingen")]
    [SerializeField] private GameObject gameOverCanvas;
    // Canvas dat getoond wordt wanneer de speler sterft

    [Header("Audio Instellingen")]
    [SerializeField] private AudioClip mijnPijnSchreeuw;
    // Geluid dat afgespeeld wordt wanneer de speler sterft

    [Header("Animatie / Vertraging")]
    [SerializeField] private float doodVertraging = 2.5f;
    // Tijd tussen geraakt worden en Game Over tonen

    private bool isAlDood = false;
    // Voorkomt dat de speler meerdere keren sterft

    private void Start()
    {
        // Zorg dat Game Over UI onzichtbaar is bij start
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);
    }


    // Wordt geactiveerd bij een fysieke botsing (bv. zombie loopt tegen speler)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy") && !isAlDood)
        {
            StartCoroutine(DoodSequence());
        }
    }


    // Wordt geactiveerd wanneer een trigger collider de speler raakt.

    private void OnTriggerEnter(Collider other)
    {
        // ❗ BELANGRIJK:
        // Als de enemy de ZAKLAMP raakt, mag de speler NIET doodgaan.
        if (other.gameObject.layer == LayerMask.NameToLayer("Zaklamp"))
            return;

        // Enemy raakt de speler
        if (other.CompareTag("enemy") && !isAlDood)
        {
            StartCoroutine(DoodSequence());
        }
    }


    // Speelt doodanimatie, geluid en toont Game Over scherm.

    private IEnumerator DoodSequence()
    {
        isAlDood = true;

        // Speel pijnschreeuw af
        if (mijnPijnSchreeuw != null)
            AudioSource.PlayClipAtPoint(mijnPijnSchreeuw, transform.position);

        // Wacht voor effect
        yield return new WaitForSeconds(doodVertraging);

        // Toon Game Over UI
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        // Pauzeer spel
        Time.timeScale = 0f;

        // Cursor vrijmaken
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
