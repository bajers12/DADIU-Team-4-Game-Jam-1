using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string Title { get; }
    public string Description { get; }
    public Sprite Sprite { get; }
    public int ScoreValue { get; }
    public DifficultyLevel Difficulty { get; }

    public float HealAmount { get; }

    public float Multiplier { get; }

    // Expose a read-only view; copy from data so runtime never mutates the asset.
    private readonly List<CardStep> _sequence = new();
    public IReadOnlyList<CardStep> Sequence => _sequence;

    public Card(CardData data)
    {
        Title = data.Title;
        Description = data.Description;
        Sprite = data.CardImage;
        ScoreValue = data.ScoreValue;
        Difficulty = data.CardDifficulty;
        Multiplier = data.NextMultiplier;
        HealAmount = data.HealAmount;

        _sequence.Clear();
        if (data.Sequence != null && data.Sequence.Length > 0)
            _sequence.AddRange(data.Sequence);
    }
}

