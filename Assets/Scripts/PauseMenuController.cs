using UnityEngine;
using UnityEngine.SceneManagement; // Nodig om tussen scenes te wisselen (Les 6)

// PauseMenuController.cs
// Beheert het pausemenu tijdens het spelen van de game.
// Gebaseerd op de leerstof van Experience Development 2:
//   - SceneManagement om naar het hoofdmenu te gaan (Les 6)
//   - [SerializeField] voor private variabelen zichtbaar in Inspector (Les 7/8)
//   - GameObject.SetActive om het pausemenu te tonen/verbergen (Les 5)
//   - Time.timeScale om het spel te pauzeren (Les 4)
//   - Cursor.lockState om de cursor te tonen/verbergen (Les 5)
//   - Encapsulatie: variabelen zijn private (Les 6/7)

public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    // Referentie naar het PausePanel GameObject.
    // Sleep het PausePanel hierheen in de Inspector.
    private GameObject pausePanel;

    [SerializeField]
    // De exacte naam van de hoofdmenu scene.
    private string mainMenuSceneName = "MainMenu";

    // Bijhouden of het spel momenteel gepauzeerd is. (encapsulatie, Les 6/7)
    private bool m_IsPaused = false;

    // Start() wordt éénmalig aangeroepen bij het begin van de scene. (Les 1)
    private void Start()
    {
        // Verberg het pausemenu bij het starten van de game. (Les 5 - SetActive)
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Zorg dat het spel normaal loopt bij het starten. (Les 4)
        Time.timeScale = 1f;

        // Vergrendel en verberg de cursor tijdens het spelen.
        // Locked: cursor zit vast in het midden van het scherm.
        // Invisible: cursor is niet zichtbaar.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update() wordt elke frame aangeroepen. (Les 1)
    private void Update()
    {
        // Controleer of de speler op ESC drukt. (Les 1 - Input)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Pauzeert het spel en toont het pausemenu.
    private void PauseGame()
    {
        // Toon het pausemenu. (Les 5 - SetActive)
        pausePanel.SetActive(true);

        // Bevriest het spel volledig. (Les 4)
        Time.timeScale = 0f;

        // Maak de cursor zichtbaar en vrij zodat de speler op knoppen kan klikken.
        // Free: cursor kan vrij bewegen over het scherm.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_IsPaused = true;
    }

    // Hervat het spel en verbergt het pausemenu.
    private void ResumeGame()
    {
        // Verberg het pausemenu. (Les 5 - SetActive)
        pausePanel.SetActive(false);

        // Laat het spel opnieuw normaal lopen. (Les 4)
        Time.timeScale = 1f;

        // Vergrendel de cursor opnieuw voor de camera besturing.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_IsPaused = false;
    }

    // --- KNOP METHODES ---

    // Wordt aangeroepen als de speler op de Doorgaan knop klikt.
    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }

    // Wordt aangeroepen als de speler op de Settings knop klikt.
    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings nog niet geïmplementeerd.");
    }

    // Wordt aangeroepen als de speler op de Quit knop klikt.
    // Gaat terug naar het hoofdmenu. (Les 6 - SceneManagement)
    public void OnQuitButtonClicked()
    {
        // Zet Time.timeScale terug naar normaal voor het laden van een nieuwe scene.
        Time.timeScale = 1f;

        // Maak cursor zichtbaar voor het hoofdmenu.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Laad de hoofdmenu scene. (Les 6)
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
