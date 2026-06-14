using UnityEngine;
using TMPro;

public class GhostDialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject dialoguePanel;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private bool panelOpen = false;

    private string[] lines = {
        "Je kunt niet ontsnappen...",
        "Ik zie je overal...",
        "Boo!"
    };

    private void OnMouseDown()
    {
        if (!panelOpen)
        {
            int random = Random.Range(0, lines.Length);
            dialogueText.text = lines[random];
            dialoguePanel.SetActive(true);
            panelOpen = true;
        }
        else
        {
            dialoguePanel.SetActive(false);
            panelOpen = false;
        }
    }
}