using UnityEngine;
using System.Collections.Generic;

public class SpaceshipController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public float drag = 2f;
    public float angularDrag = 2f;

    private float baseMoveSpeed;
    private float baseRotationSpeed;

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

        baseMoveSpeed = moveSpeed;
        baseRotationSpeed = rotationSpeed;
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

        moveSpeed = Mathf.Min(baseMoveSpeed * thrustMultiplier, baseMoveSpeed);
        rotationSpeed = Mathf.Min(baseRotationSpeed * turnMultiplier, baseRotationSpeed);

        float rotationAmount = rotationInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        Vector2 direction = transform.up * thrustInput;
        rb.AddForce(direction * moveSpeed);
    }
} 
