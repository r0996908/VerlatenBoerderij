using UnityEngine;
using UnityEngine.SceneManagement; 

public class RestartGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // GetActivateScene → GetActiveScene
    }
}