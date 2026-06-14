// WinTrigger.cs
using UnityEngine;

// Detecteert wanneer de speler de escape-zone bereikt.
// Roept WinManager aan.

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private WinManager winManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winManager.TriggerWin();
        }
    }
}
