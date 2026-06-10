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
//   3. Sleep de knoppen naar de juiste velden in de Inspector
//   4. Koppel elke knop aan de juiste methode via Button OnClick() in de Inspector

public class MainMenuController : MonoBehaviour
{
    // --- SCENE NAAM ---

    [SerializeField]
    // De exacte naam van je game scene.
    // Pas dit aan in de Inspector als je scene anders heet.
    private string gameSceneName = "AbandonedFarm";

    // --- KNOPPEN ---

    [SerializeField]
    // Referentie naar de Quit knop.
    // Sleep de QuitButton hierheen in de Inspector.
    // Deze knop wordt verborgen bij het startmenu en getoond bij het pausemenu.
    private GameObject quitButton;

    // Start() wordt éénmalig aangeroepen bij het begin van de scene. (Les 1)
    private void Start()
    {
        // Verberg de Quit knop bij het opstarten van het startmenu.
        // SetActive(false) maakt een GameObject onzichtbaar en inactief. (Les 5)
        // De Quit knop is alleen zichtbaar tijdens het pausemenu in de game scene.
        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }
    }

    // --- KNOP METHODES ---
    // Koppel deze methodes aan de knoppen via Button OnClick() in de Inspector.

    // Wordt aangeroepen als de speler op de Start knop klikt.
    // Laadt de game scene. (Les 6 - SceneManagement)
    public void OnStartButtonClicked()
    {
        // LoadScene laadt een nieuwe scene op basis van de naam. (Les 6)
        SceneManager.LoadScene(gameSceneName);
    }

    // Wordt aangeroepen als de speler op de Settings knop klikt.
    // Voorlopig nog leeg, kan later ingevuld worden.
    public void OnSettingsButtonClicked()
    {
        // TODO: instellingen toevoegen in een later stadium
        Debug.Log("Settings knop geklikt - nog niet geïmplementeerd.");
    }

    // Wordt aangeroepen als de speler op de Quit knop klikt.
    // Sluit de applicatie af.
    public void OnQuitButtonClicked()
    {
        // Application.Quit() sluit de game af in een build. (Les 6)
        Application.Quit();

        // Stopt het spelen in de Unity editor (werkt niet in een build).
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
