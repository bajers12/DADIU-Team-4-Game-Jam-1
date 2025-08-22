using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public Sprite CardImage { get; private set; }
    [field: SerializeField] public int ScoreValue { get; private set; } = 25;
    [field: SerializeField] public DifficultyLevel CardDifficulty { get; private set; }

    [field: SerializeField] public float NextMultiplier { get; private set; }
    [field: SerializeField] public float HealAmount { get; private set; }

    [Header("Fixed input sequence (never changes)")]
    [Tooltip("Define the exact combo for this card. Duplicates allowed. 'Pause' = empty step.")]
    [field: SerializeField] public CardStep[] Sequence { get; private set; } = new CardStep[0];
}

public enum DifficultyLevel { Easy, Medium, Hard }
public enum CardStep { U, D, L, R, LR, UD, UL, UR, RD, LD, P}
