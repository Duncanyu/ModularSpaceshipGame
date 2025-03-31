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
        // Implement collision damage logic here
        // Example placeholder:
        // if (collision.CompareTag("Enemy"))
        // {
        //     collision.GetComponent<Enemy>().TakeDamage(damage);
        //     Destroy(gameObject);
        // }
    }

    public void Initialize(float weaponDamage)
    {
        damage = weaponDamage;
    }
}