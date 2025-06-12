using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Camera mainCamera;
    public bool isDragging = false;
    private Vector3 offset;

    private float zStart;
    // GANZE KLASSE WIRD GERADE NICHT VERWENDENT. VLLT. UMSCHREIBEN FÜR DRAG AND DROP IMPLEMENTIERUNG!!!!!
    void Start()
    {
        // Holen Sie sich die Hauptkamera, um die Mausposition im Weltraum zu konvertieren
        mainCamera = Camera.main;
        zStart = transform.position.z;
    }

    void Update()
    {
        // Linke Maustaste gedrückt
        //if (Input.GetMouseButtonDown(0))
        if (isDragging)
        {
            // Überprüfen, ob der Mauszeiger auf das GameObject zeigt
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Wenn das GameObject getroffen wurde, starten wir das Ziehen
                if (hit.transform == transform)
                {
                    //isDragging = true;
                    Vector3 newPosition = transform.position;
                    newPosition.z = zStart; // Set the desired z value
                    transform.position = newPosition;
                    // Berechnen des Offsets (Differenz zwischen Mausposition und GameObject-Position)
                    offset = transform.position - hit.point;
                }
            }
        }

        // Wenn das GameObject gezogen wird
        if (isDragging)
        {
            // Mausposition im Weltraum erhalten
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPosition = ray.GetPoint(Vector3.Distance(mainCamera.transform.position, transform.position)) + offset;

            // Behalten der Z-Position bei, sodass es nur auf der XY-Ebene verschoben wird
            targetPosition.z = transform.position.z;

            // Setze die neue Position des GameObjects
            transform.position = targetPosition;
        }

        // Linke Maustaste losgelassen
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
