using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float projectileGravity;

    protected override void Shoot(float damage)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        Vector2 projectileForce = transform.up * projectileSpeed;
        rb.AddForce(projectileForce, ForceMode2D.Impulse);
        rb.gravityScale = projectileGravity;

        Projectile projComponent = projectile.GetComponent<Projectile>();
        if (projComponent != null)
        {
            projComponent.Initialize(damage);
        }
    }
}
