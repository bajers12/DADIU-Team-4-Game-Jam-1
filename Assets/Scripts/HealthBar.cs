using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;   // assign HealthBar_Fill here
    [SerializeField] private GameController gc; // your GameController reference

    private float maxHealth = 100f;
    private Tween currentTween;

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        SetHealth(health);
        Debug.Log("Max health set to: " + maxHealth);
    }

    /// <summary>
    /// Call this whenever player health changes
    /// </summary>
    public void SetHealth(float newHealth)
    {
        float targetPercent = Mathf.Clamp01(newHealth / maxHealth);

        // Kill any old tween before starting a new one
        currentTween?.Kill();

        // Animate smoothly to the new value
        currentTween = fillImage
            .DOFillAmount(targetPercent, 0.3f)
            .SetEase(Ease.OutQuad);
    }
}
