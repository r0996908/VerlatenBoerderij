using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "AbandonedFarm";
    [SerializeField] private GameObject quitButton;

    private void Start()
    {
        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings nog niet geïmplementeerd.");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
