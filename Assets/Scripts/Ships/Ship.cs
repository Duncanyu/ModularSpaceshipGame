using UnityEngine;

public class Ship : MonoBehaviour
{
    public ShipHealth Health { get; private set; }

    private void Awake()
    {
        Health = GetComponent<ShipHealth>();

        if (Health == null)
        {
            Debug.LogWarning("[Ship] No ShipHealth component found on this ship.");
        }
        else
        {
            Debug.Log("[Ship] ShipHealth successfully linked.");
        }
    }

    // Future expansion: add central logic here for
    // damage delegation, energy distribution, critical systems, etc.
}
