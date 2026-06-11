using UnityEngine;
using UnityEngine.SceneManagement;

// Laat de speler de huidige scene herstarten met de R-toets.

public class RestartGame : MonoBehaviour
{
    private void Update()
    {
        // R om de huidige scene opnieuw te laden
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
