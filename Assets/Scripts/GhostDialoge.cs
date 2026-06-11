using UnityEngine;
using TMPro;


// Stuurt de dialoog van een geest aan.
// Toont tekst in een panel wanneer speler in de buurt is
// Doorloopt meerdere lijnen bij input

public class GhostDialogue : MonoBehaviour
{
    [Header("UI verwijzingen")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    // Tekstveld waarin de dialoog getoond wordt

    [SerializeField] private GameObject dialoguePanel;
    // Panel dat de dialoog bevat

    [Header("Dialooglijnen")]
    [SerializeField]
    private string[] lines =
    {
        "Wie... wie ben jij?",
        "Deze boerderij verlaat je niet zomaar.",
        "Ik wacht hier al jaren... ga weg."
    };
    // De verschillende zinnen die de geest zegt

    private int currentLine = 0;
    // Index van de huidige zin

    private bool playerInRange = false;
    // Flag die aangeeft of de speler in de triggerzone staat

    private void Update()
    {
        HandleInput();
    }

    // Controleert op input zolang de speler in de buurt is.

    private void HandleInput()
    {
        if (!playerInRange) return;

        // E of linkermuisknop om naar de volgende zin te gaan
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }


    // Toont de volgende zin in de dialoog.
 
    private void ShowNextLine()
    {
        if (currentLine < lines.Length)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = lines[currentLine];
            currentLine++;
        }
        else
        {
            // Dialoog is afgelopen, panel verbergen en resetten
            dialoguePanel.SetActive(false);
            currentLine = 0;
        }
    }


    // Wordt uitgevoerd wanneer een collider de triggerzone binnenkomt.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

 
    // Wordt uitgevoerd wanneer een collider de triggerzone verlaat.

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialoguePanel.SetActive(false);
            currentLine = 0;
        }
    }
}
