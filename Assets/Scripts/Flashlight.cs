using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    private bool FlashlightActive = false;

    [Header("Geluid Instellingen")]
    [SerializeField] private AudioSource audioSource; // De speaker
    [SerializeField] private AudioClip clickSound;    // Het geluidsbestand (.mp3/.wav)

    // Start is called before the first frame update
    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
        
        // Automatische check: als je de AudioSource vergeet te slepen, 
        // zoekt hij of er eentje op hetzelfde object staat.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Speel het klikgeluid af zodra je op F drukt!
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
}