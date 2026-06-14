// WinManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

// Toont het win-scherm wanneer de speler het doel bereikt.
// Pauzeert het spel en laat terugkeren naar het menu.

public class WinManager : MonoBehaviour
{
    [Header("UI verwijzingen")]
    [SerializeField] private GameObject winPanel;

    private bool hasWon = false;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        Time.timeScale = 1f;
    }


    // Wordt opgeroepen wanneer de speler het doel bereikt.

    public void TriggerWin()
    {
        if (hasWon) return;

        hasWon = true;

        winPanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // tijd terug normaal
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

}
