using UnityEngine;
using UnityEngine.SceneManagement; // Nodig om tussen scenes te wisselen (Les 6)

// MainMenuController.cs
// Beheert het hoofdmenu van de game.
// Gebaseerd op de leerstof van Experience Development 2:
//   - SceneManagement om tussen scenes te wisselen (Les 6)
//   - [SerializeField] voor private variabelen zichtbaar in Inspector (Les 7/8)
//   - GameObject.SetActive om knoppen te tonen/verbergen (Les 5)
//   - Encapsulatie: variabelen zijn private (Les 6/7)
//
// SETUP in Unity:
//   1. Maak een leeg GameObject in de MainMenu scene, noem het "MenuManager"
//   2. Voeg dit script toe via Add Component
//   3. Sleep de QuitButton naar het juiste veld in de Inspector
//   4. Koppel elke knop aan de juiste methode via Button OnClick() in de Inspector

public class MainMenuController : MonoBehaviour
{
    // --- SCENE NAAM ---

    [SerializeField]
    // De exacte naam van de game scene zoals die staat in de Build Profiles Scene List.
    // Je kan dit aanpassen in de Inspector zonder het script te wijzigen.
    private string gameSceneName = "AbandonedFarm";

    // --- KNOPPEN ---

    [SerializeField]
    // Referentie naar de Quit knop.
    // Sleep de QuitButton hierheen in de Inspector.
    // Deze knop is verborgen in het startmenu en zichtbaar in het pausemenu.
    private GameObject quitButton;

    // Start() wordt éénmalig aangeroepen bij het begin van de scene. (Les 1)
    private void Start()
    {
        // Verberg de Quit knop bij het opstarten van het startmenu. (Les 5 - SetActive)
        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }
    }

    // Wordt aangeroepen als de speler op de Start knop klikt.
    // Laadt de game scene via SceneManager. (Les 6)
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Wordt aangeroepen als de speler op de Settings knop klikt.
    // Voorlopig nog leeg, wordt later ingevuld.
    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings nog niet geïmplementeerd.");
    }

    // Wordt aangeroepen als de speler op de Quit knop klikt.
    public void OnQuitButtonClicked()
    {
        // Sluit de game af in een build. (Les 6)
        Application.Quit();

        // Stopt het spelen in de Unity editor.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
