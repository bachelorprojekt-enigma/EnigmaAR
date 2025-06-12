using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    // Bewegungsgeschwindigkeit der Kamera
    public float moveSpeed = 5f;

    // Mausempfindlichkeit für die Kamerarotation
    public float mouseSensitivity = 2f;

    // Die Ausgangsposition der Maus für die Rotation
    private Vector3 lastMousePosition;

    // Drehwinkel der Kamera (Rotation)
    private float rotationX = 0f;
    private float rotationY = 0f;

    // Update wird einmal pro Frame aufgerufen
    void Update()
    {
        // Bewegung der Kamera (WASD-Tasten)
        MoveCamera();

        // Rotation der Kamera (rechte Maustaste)
        RotateCamera();
    }

    // Methode zur Bewegung der Kamera
    private void MoveCamera()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D oder Pfeiltasten links/rechts
        float vertical = Input.GetAxis("Vertical"); // W/S oder Pfeiltasten hoch/runter

        // Berechne die Richtung in die die Kamera sich bewegen soll
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Bewege die Kamera
        transform.position += move * (moveSpeed * Time.deltaTime);
    }

    // Methode zur Rotation der Kamera
    private void RotateCamera()
    {
        // Wenn die rechte Maustaste gedrückt ist, Kamera rotieren
        if (Input.GetMouseButton(1)) // rechte Maustaste
        {
            // Berechne die Mausbewegung
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Kamera um X (horizontal) und Y (vertikal) Achse drehen
            rotationX -= mouseY * mouseSensitivity;
            rotationY += mouseX * mouseSensitivity;

            // Begrenzen der Rotation (verhindert, dass die Kamera sich umdreht)
            rotationX = Mathf.Clamp(rotationX, -80f, 80f);

            // Wende die Rotation auf die Kamera an
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }
}