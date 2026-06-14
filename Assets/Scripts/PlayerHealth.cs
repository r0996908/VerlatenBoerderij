using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Spawn Instellingen")]
    [SerializeField] private Vector3 homeSpawnPositie = new Vector3(0f, 1f, 0f);

    [Header("UI Instellingen")]
    [SerializeField] private GameObject gameOverCanvas;

    [Header("Audio Instellingen")]
    [SerializeField] private AudioClip mijnPijnSchreeuw;

    [Header("Animatie / Vertraging")]
    [SerializeField] private float doodVertraging = 2.5f;

    private bool isAlDood = false;

    private void Start()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    // Wordt geactiveerd bij een fysieke botsing (als de zombie tegen je aanloopt)
    // Wordt geactiveerd bij een fysieke botsing tegen je lichaam
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy") && !isAlDood)
        {
            StartCoroutine(DoodSequence());
        }
    }

    // Wordt geactiveerd als de zombie in een Trigger loopt
    private void OnTriggerEnter(Collider other)
    {
        // 1. Check of het de enemy is
        if (other.CompareTag("enemy") && !isAlDood)
        {
            // 2. DE BELANGRIJKSTE CHECK:
            // We kijken of dit script toevallig de botsing binnenkrijgt via de zaklamp.
            // Als dit specifieke GameObject een Flashlight-component heeft, OF een child is waar het licht op zit, STOPPEN WE.
            if (GetComponent<Flashlight>() != null || GetComponentInChildren<Flashlight>() != null)
            {
                // We controleren of de zombie de zaklamp raakt.
                // Als de zombie het LICHT raakt, mag de speler NIET doodgaan!
                // We willen alleen doodgaan als de zombie de ECHTE speler-body raakt.
                
                // Unity stuurt triggers van kinderen door naar de parent. 
                // Om te checken of hij de body raakt, kijken we of de collider GEEN trigger is.
                if (other.isTrigger) 
                {
                    return; // Het was een trigger-botsing (het licht), dus negeer de dood!
                }
            }

            // EXTRA CHECK: Als de zombie jouw 'FlashlightLight' object raakt via de trigger, negeer het!
            if (gameObject.name != "PlayerCapsule" && gameObject.name != "Player")
            {
                // Als dit script per ongeluk op de zaklamp zelf staat, ga dan niet dood.
                return; 
            }

            // 3. Pas als de zombie écht door je lichtstraal heen is gelopen en je lichaam raakt:
            StartCoroutine(DoodSequence());
        }
    }
}