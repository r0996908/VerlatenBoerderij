// EnemyCounterText.cs
using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter instance;

    [SerializeField]
    private TextMeshProUGUI counterText;

    private int enemyCount;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        UpdateText();
    }

    public void EnemyDied()
    {
        enemyCount--;
        UpdateText();
    }

    private void UpdateText()
    {
        counterText.text = "Enemies: " + enemyCount;
    }
}