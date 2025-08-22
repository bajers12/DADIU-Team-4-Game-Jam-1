using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;   // assign HealthBar_Fill here
    [SerializeField] private GameController gc; // your GameController reference

    private float maxHealth = 100f;
    private Tween currentTween;

    private void Start()
    {
        // Set starting max health (replace with gc.maxPlayerHealth if available)
        maxHealth = gc.playerHealth;

        // Initialize UI
        SetHealth(gc.playerHealth);
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
