using UnityEngine;
using UnityEngine.InputSystem;
public class SphereController : MonoBehaviour
{
    [SerializeField] private float dragGain = 0.02f;
    [SerializeField] private Rigidbody rigidBody;
    private Vector2 previousPosition;
    private Vector3 pendingImpulse;
    void Awake()
    {
        if (rigidBody == null)
            rigidBody = GetComponentInChildren<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogError("Assign the sphere's Rigidbody.", this);
            enabled = false;
        }
    }

    void Update()
    {
        var pointer = Pointer.current;
        if (pointer == null) return;
        Vector2 position = pointer.position.ReadValue();
        if (pointer.press.wasPressedThisFrame)
            previousPosition = position;
        else if (pointer.press.isPressed)
        {
            Vector2 delta = position - previousPosition;
            pendingImpulse +=
                new Vector3(delta.x, 0f, delta.y) * dragGain;
            previousPosition = position;
        }
    }

void FixedUpdate()
{
rigidBody.AddForce(pendingImpulse, ForceMode.Impulse);
pendingImpulse = Vector3.zero;
}
}


