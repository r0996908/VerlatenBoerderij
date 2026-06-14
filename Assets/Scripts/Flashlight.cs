using UnityEngine;


// Stuurt de zaklamp aan.
// F om licht aan/uit te zetten
// Alleen wanneer licht actief is kan de zaklamp enemies vernietigen
// Zaklamp gebruikt een trigger collider om enemies te detecteren

public class Flashlight : MonoBehaviour
{
    [Header("Licht Instellingen")]
    [SerializeField] private GameObject FlashlightLight;
    // Het daadwerkelijke lichtobject dat aan/uit gezet wordt

    private bool FlashlightActive = false;
    // Houdt bij of het licht aan staat

    [Header("Geluid Instellingen")]
    [SerializeField] private AudioSource audioSource;
    // Audio bron voor klikgeluid

    [SerializeField] private AudioClip clickSound;
    // Geluid dat afgespeeld wordt bij aan/uit zetten

    private void Start()
    {
        // Licht start uit
        FlashlightLight.SetActive(false);

        // Als er geen AudioSource is, probeer hem te vinden
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // F om zaklamp aan/uit te zetten
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Speel klikgeluid
            if (audioSource != null && clickSound != null)
                audioSource.PlayOneShot(clickSound);

            // Toggle licht
            FlashlightActive = !FlashlightActive;
            FlashlightLight.SetActive(FlashlightActive);
        }
    }


    // Vernietigt enemies zolang het licht actief is.

    private void OnTriggerStay(Collider other)
    {
        // Licht moet aan staan
        if (!FlashlightActive) return;

        // Enemy geraakt → vernietigen
        if (other.CompareTag("enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
