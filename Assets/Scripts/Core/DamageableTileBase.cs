using UnityEngine;

public abstract class DamageableTileBase : MonoBehaviour
{
    public float durability = 100f;
    public float maxDurability = 100f;

    public virtual void TakeDamage(float amount)
    {
        durability -= amount;
        durability = Mathf.Max(durability, 0f);

        if (durability <= 0f)
        {
            OnDestroyed();
        }
    }

    protected virtual void OnDestroyed()
    {
        Destroy(gameObject);
    }
}
