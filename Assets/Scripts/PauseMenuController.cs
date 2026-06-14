using UnityEngine;
using UnityEngine.SceneManagement;


// Stuurt het pauzemenu aan.
// Pauzeert en hervat het spel
// Toont en verbergt het pauzepanel
// Laat terugkeren naar het hoofdmenu
// Game restarten

public class PauseMenuController : MonoBehaviour
{
    [Header("UI verwijzingen")]
    [SerializeField] private GameObject pausePanel;
    // Panel dat getoond wordt wanneer het spel gepauzeerd is

    [Header("Scene instellingen")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    // Naam van de hoofdmenu-scene

    private bool m_IsPaused = false;
    // Flag die bijhoudt of het spel momenteel gepauzeerd is

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Escape om te pauzeren of hervatten
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_IsPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }


    // Pauzeert het spel en toont het pauzepanel.

    private void PauseGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_IsPaused = true;
    }


    // Hervat het spel en verbergt het pauzepanel.

    private void ResumeGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_IsPaused = false;
    }


    // Wordt gekoppeld aan de Resume-knop in het pauzemenu.

    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }

    

 
    // Wordt gekoppeld aan de Quit-knop in het pauzemenu.
    // Laadt het hoofdmenu.
  
    public void OnQuitButtonClicked()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
