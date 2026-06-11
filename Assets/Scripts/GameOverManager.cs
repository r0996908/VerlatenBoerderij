using UnityEngine;
using UnityEngine.SceneManagement;

// Toont het Game Over scherm wanneer de speler geraakt wordt.
// Les 2/3: Colliders & triggers
// Les 5: UI via SetActive()
// Les 6: SceneManagement
// Encapsulatie + [SerializeField]

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPanel;
    // UI panel dat getoond wordt bij Game Over (Les 5)

    private bool isGameOver = false;
    // Houdt bij of het spel al game over is (encapsulatie)

    private void Start()
    {
        // Zorg dat het panel onzichtbaar is bij start (Les 5)
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // Deze methode wordt opgeroepen door enemies wanneer ze de speler raken
    public void TriggerGameOver()
    {
        if (isGameOver) return; // voorkomt dubbele triggers

        isGameOver = true;

        // Toon UI (Les 5)
        gameOverPanel.SetActive(true);

        // Pauzeer het spel (Les 4)
        Time.timeScale = 0f;

        // Cursor zichtbaar maken zodat speler knoppen kan klikken (Les 5)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Wordt gekoppeld aan een UI-knop in het Game Over panel
    public void RestartGame()
    {
        Time.timeScale = 1f; // reset tijd
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
