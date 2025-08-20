using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card Data")]


public class CardData : ScriptableObject
{
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public Sprite CardImage { get; private set; }
    [field: SerializeField] public int ScoreValue { get; private set; } = 25;
    [field: SerializeField] public DifficultyLevel CardDifficulty { get; private set; }

}
public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}
