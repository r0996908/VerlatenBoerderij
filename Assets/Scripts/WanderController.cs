using UnityEngine;

// WanderController.cs
// Laat een GameObject (Ghost of Zombie) willekeurig rondlopen.
// Gebaseerd op de leerstof van Experience Development 2:
//   - transform.Translate voor beweging (Les 3)
//   - [SerializeField] voor private variabelen zichtbaar in Inspector (Les 7/8)
//   - InvokeRepeating voor herhaalde methode-aanroepen op timer (Les 2)
//   - Tags voor onderscheid tussen objecten (Les 3)
//   - Encapsulatie: variabelen zijn private, enkel zichtbaar via [SerializeField] (Les 6/7)
//
// Gebruik:
//   Voeg dit script toe aan je Ghost en/of Zombie GameObject in Unity.
//   Pas de waarden aan in de Inspector zonder de code te hoeven aanpassen.

public class WanderController : MonoBehaviour
{
    // --- BEWEGING ---

    [SerializeField]
    // Hoe snel het object beweegt, in Unity units per seconde.
    // Private zodat andere scripts dit niet zomaar kunnen aanpassen (encapsulatie).
    // [SerializeField] maakt het wel zichtbaar en aanpasbaar in de Inspector.
    private float moveSpeed = 2f;

    // --- RICHTING WISSELEN ---

    [SerializeField]
    // Minimale tijd in seconden dat het object in dezelfde richting beweegt
    // voordat een nieuwe willekeurige richting gekozen wordt.
    private float minWanderTime = 2f;

    [SerializeField]
    // Maximale tijd in seconden in dezelfde richting.
    // De echte wachttijd wordt willekeurig gekozen tussen min en max (Random.Range).
    private float maxWanderTime = 5f;

    // --- INTERNE TOESTAND ---

    // De huidige bewegingsrichting als Vector3 (x, y, z).
    // Private omdat andere scripts de richting niet mogen aanpassen.
    // Wordt bijgehouden als Vector3, beweging enkel horizontaal (y = 0).
    private Vector3 m_MoveDirection;

    // Start() wordt éénmalig aangeroepen bij het begin van de applicatie
    // of wanneer het GameObject aangemaakt wordt. (Les 1 - Order of execution)
    private void Start()
    {
        // Kies meteen een eerste willekeurige richting bij het starten.
        PickNewDirection();

        // InvokeRepeating roept een methode herhaald aan na een startdelay
        // en daarna elke X seconden. (Les 2)
        // Hier gebruiken we het om op een willekeurig interval van richting te wisselen.
        // We starten met een willekeurige tijd tussen min en max.
        float firstInterval = Random.Range(minWanderTime, maxWanderTime);

        // Roep "PickNewDirection" aan na firstInterval seconden,
        // en daarna elke maxWanderTime seconden als basis.
        // In PickNewDirection zelf stoppen we InvokeRepeating en starten we opnieuw
        // zodat het interval elke keer opnieuw willekeurig is.
        Invoke(nameof(PickNewDirection), firstInterval);
    }

    // Update() wordt elke frame aangeroepen. (Les 1 - Order of execution)
    // Hier verplaatsen we het object elke frame in de huidige richting.
    private void Update()
    {
        // transform.Translate beweegt het object in de opgegeven richting.
        // * moveSpeed: snelheid in units per seconde
        // * Time.deltaTime: zorgt dat de beweging framerate-onafhankelijk is (Les 3)
        //   (zonder Time.deltaTime zou het object op 60fps 2x zo snel gaan als op 30fps)
        transform.Translate(m_MoveDirection * moveSpeed * Time.deltaTime);
    }

    // PickNewDirection() is een privé methode die een nieuwe willekeurige richting kiest.
    // Abstractie: de logica voor richtingsbepaling zit hier, niet verspreid over andere methodes. (Les 6/7)
    private void PickNewDirection()
    {
        // Random.Range geeft een willekeurig getal tussen 0 en 360 graden. (Les 2)
        float randomAngle = Random.Range(0f, 360f);

        // Bereken een richting op het horizontale vlak (y = 0) op basis van de hoek.
        // Mathf.Sin en Mathf.Cos werken in radialen, vandaar de omzetting met Mathf.Deg2Rad.
        // Dit geeft een Vector3 met lengte 1 (genormaliseerd) in een willekeurige horizontale richting.
        m_MoveDirection = new Vector3(
            Mathf.Sin(randomAngle * Mathf.Deg2Rad),  // x-component
            0f,                                        // y = 0: geen verticale beweging
            Mathf.Cos(randomAngle * Mathf.Deg2Rad)   // z-component
        );

        // Plan de volgende richtingswissel in na een willekeurige tijd.
        // CancelInvoke stopt een eerder geplande Invoke zodat er geen dubbele oproepen zijn.
        CancelInvoke(nameof(PickNewDirection));
        float nextInterval = Random.Range(minWanderTime, maxWanderTime);
        Invoke(nameof(PickNewDirection), nextInterval);
    }
}
