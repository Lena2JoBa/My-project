using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float force = 10f;
    [SerializeField] private float touchSensitivity = 0.05f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (cam == null && Camera.main != null)
            cam = Camera.main.transform;
    }

    void FixedUpdate()
    {
        Vector2 input = Vector2.zero;

        // Tastatur (Editor / PC): WASD oder Pfeiltasten
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) input.y += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) input.y -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input.x += 1f;
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) input.x -= 1f;
        }

        // Touch (Handy): Finger auf dem Bildschirm ziehen
        var touch = Touchscreen.current;
        if (touch != null && touch.primaryTouch.press.isPressed)
        {
            input += Vector2.ClampMagnitude(
                touch.primaryTouch.delta.ReadValue() * touchSensitivity, 1f);
        }

        if (input == Vector2.zero) return;

        // Bewegung relativ zur Blickrichtung der Kamera, flach auf dem Boden
        Vector3 forward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;
        Vector3 direction = forward * input.y + right * input.x;

        rb.AddForce(direction * force);
    }
}
