using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public float drag = 2f;
    public float angularDrag = 2f;

    private Rigidbody2D rb;
    private float rotationInput;
    private float thrustInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
    }

    void Update()
    {
        rotationInput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        float rotationAmount = rotationInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        Vector2 direction = transform.up * thrustInput;
        rb.AddForce(direction * moveSpeed);
    }
}
