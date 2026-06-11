using UnityEngine;
using UnityEngine.SceneManagement;


// Stuurt het hoofdmenu aan.
// Start het spel
//Afsluiten van de applicatie

public class MainMenuController : MonoBehaviour
{
    [Header("Scene instellingen")]
    [SerializeField] private string gameSceneName = "AbandonedFarm";
    // Naam van de scene waarin het spel zich afspeelt

    [Header("UI verwijzingen")]
    [SerializeField] private GameObject quitButton;
    // Quit-knop, kan verborgen worden op bepaalde platformen

    private void Start()
    {
        // Voorbeeld: verberg de quit-knop als die niet nodig is
        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }

        // Cursor vrijmaken in het hoofdmenu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1f;
    }


    // Wordt gekoppeld aan de Start-knop.
    // Laadt de spel-scene.

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }


 

    // Wordt gekoppeld aan de Quit-knop.
    // Sluit de applicatie of stopt Play Mode in de editor.

    public void OnQuitButtonClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
