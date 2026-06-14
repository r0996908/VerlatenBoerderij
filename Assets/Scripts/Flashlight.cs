using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    private bool FlashlightActive = false;

    [Header("Geluid Instellingen")]
    [SerializeField] private AudioSource audioSource; // De speaker
    [SerializeField] private AudioClip clickSound;    // Het geluidsbestand

    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }

            if (FlashlightActive == false)
            {
                FlashlightLight.gameObject.SetActive(true);
                FlashlightActive = true;
            }
            else
            {
                FlashlightLight.gameObject.SetActive(false);
                FlashlightActive = false;
            }
        }
    }

    // De zaklamp vernietigt alleen de zombie als het licht actief is!
    private void OnTriggerStay(Collider other)
    {
        if (FlashlightActive && other.CompareTag("enemy"))
        {
            Debug.Log("Licht schijnt op " + other.name + "! Zombie vernietigd.");
            Destroy(other.gameObject);
        }
    }
}