using UnityEngine;
using System.Collections.Generic;

public class SpaceshipController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public float drag = 2f;
    public float angularDrag = 2f;

    private Rigidbody2D rb;
    private float rotationInput;
    private float thrustInput;
    private List<EngineBase> engines = new List<EngineBase>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
        engines.AddRange(GetComponentsInChildren<EngineBase>());
    }

    void Update()
    {
        rotationInput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        float thrustMultiplier = 0f;
        float turnMultiplier = 0f;

        foreach (var engine in engines)
        {
            thrustMultiplier += engine.GetSpeedContribution(1f);
            turnMultiplier += engine.GetTurnContribution(1f);
        }

        float actualMoveSpeed = moveSpeed * thrustMultiplier;
        float actualRotationSpeed = rotationSpeed * turnMultiplier;

        float rotationAmount = rotationInput * actualRotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        Vector2 direction = transform.up * thrustInput;
        rb.AddForce(direction * actualMoveSpeed);

        if (thrustInput > 0f)
        {
            foreach (var engine in engines)
            {
                engine.ApplyThrust();
            }
        }
    }

    public void RegisterEngine(EngineBase engine)
    {
        if (!engines.Contains(engine))
        {
            engines.Add(engine);
        }
    }

    public void UnregisterEngine(EngineBase engine)
    {
        if (engines.Contains(engine))
        {
            engines.Remove(engine);
        }
    }
}
