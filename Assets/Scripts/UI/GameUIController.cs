using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    public SpaceshipController playerShip;

    [Header("Text Elements")]
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI durabilityText;
    public TextMeshProUGUI speedText;

    void Update()
    {
        if (playerShip == null) return;

        UpdateEnergyUI();
        UpdateResourceUI();
        UpdateDurabilityUI();
        UpdateSpeedUI();
    }

    void UpdateEnergyUI()
    {
        if (energyText != null)
        {
            float current = Mathf.Round(playerShip.GetCurrentEnergy());
            float max = Mathf.Round(playerShip.GetMaxEnergy());
            energyText.text = $"{current}/{max}";
        }
    }

    void UpdateResourceUI()
    {
        if (resourceText != null)
        {
            resourceText.text = $"0/0";
        }
    }

    void UpdateDurabilityUI()
    {
        if (durabilityText != null)
        {
            float current = 0f;
            float max = 0f;
            var tiles = playerShip.GetComponentsInChildren<DamageableTileBase>();
            foreach (var tile in tiles)
            {
                max += tile.GetMaxHealth();
                current += tile.GetCurrentHealth();
            }

            float percent = (max > 0f) ? (current / max) * 100f : 0f;
            durabilityText.text = $"{Mathf.Round(percent)}%";
        }
    }

    void UpdateSpeedUI()
    {
        if (speedText != null)
        {
            float speed = playerShip.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
            speedText.text = $"{speed:F1} u/s";
        }
    }
}
