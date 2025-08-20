using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public UI.Slider slider; // Make a slider component for health bar in the Unity Editor and assign it here

    public void SetHealth(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}
