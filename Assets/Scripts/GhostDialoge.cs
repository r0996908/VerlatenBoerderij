using UnityEngine;
using TMPro;

public class GhostDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField]
    private string[] lines = {
        "Wie... wie ben jij?",
        "Deze boerderij verlaat je niet zomaar.",
        "Ik wacht hier al jaren... ga weg."
    };

    private int currentLine = 0;
    private bool playerInRange = false;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

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
            dialoguePanel.SetActive(false);
            currentLine = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

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
