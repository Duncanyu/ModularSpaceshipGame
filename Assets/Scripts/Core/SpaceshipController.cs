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
    private List<WeaponBase> weapons = new List<WeaponBase>();
    private float currentEnergy;
    public float maxEnergy = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
        engines.AddRange(GetComponentsInChildren<EngineBase>());

        baseMoveSpeed = moveSpeed;
        baseRotationSpeed = rotationSpeed;
        currentEnergy = 0f;

        var reactors = GetComponentsInChildren<ReactorBase>();
        foreach (var reactor in reactors)
        {
            reactor.TryRegisterReactor();
        }
    }

    public void RegisterEngine(EngineBase engine)
    {
        if (!engines.Contains(engine))
        {
            engines.Add(engine);
        }
    }

    public void RegisterWeapon(WeaponBase weapon)
    {
        if (!weapons.Contains(weapon))
        {
            weapons.Add(weapon);
        }
    }

    public void UnregisterEngine(EngineBase engine)
    {
        if (engines.Contains(engine))
        {
            engines.Remove(engine);
        }
    }

    public bool TryConsumeEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            return true;
        }
        return false;
    }

    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
    }

    public void AddMaxEnergy(float amount)
    {
        maxEnergy += amount;
    }

    public void RegisterReactor(ReactorBase reactor)
    {
        AddMaxEnergy(reactor.maxEnergyContribution);
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public float GetMaxEnergy()
    {
        return maxEnergy;
    }

    void Update()
    {
        rotationInput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        // Debug.Log($"{currentEnergy}/{maxEnergy} E");
    }

    void FixedUpdate()
    {
        float thrustMultiplier = 0f;
        float turnMultiplier = 0f;

        foreach (var engine in engines)
        {
            engine.ConsumeEnergy(Time.fixedDeltaTime);
            thrustMultiplier += engine.GetSpeedContribution(1f);
            turnMultiplier += engine.GetTurnContribution(1f);
        }

        foreach (var weapon in weapons)
        {
            weapon.DrainPassiveEnergy(Time.fixedDeltaTime);
        }

        moveSpeed = Mathf.Min(baseMoveSpeed * thrustMultiplier, baseMoveSpeed);
        rotationSpeed = Mathf.Min(baseRotationSpeed * turnMultiplier, baseRotationSpeed);

        float rotationAmount = rotationInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        Vector2 direction = transform.up * thrustInput;
        rb.AddForce(direction * moveSpeed);
    }
} 
