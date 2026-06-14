using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLevens = 3;
    private int huidigeLevens;

    [SerializeField] private TextMeshProUGUI levensText;
    // Sleep je UI tekst hier naartoe in Inspector

    private void Start()
    {
        huidigeLevens = maxLevens;
        UpdateUI();
    }

    public void NeemSchade()
    {
        huidigeLevens--;
        UpdateUI();

        if (huidigeLevens <= 0)
        {
            GameOver();
        }
    }

    private void UpdateUI()
    {
        if (levensText != null)
            levensText.text = "Levens: " + huidigeLevens;
    }

    private void GameOver()
    {
        SceneManager.LoadScene(1);
    }
}