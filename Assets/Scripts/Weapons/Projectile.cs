using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public void Initialize(float weaponDamage)
    {
        damage = weaponDamage;
    }
}