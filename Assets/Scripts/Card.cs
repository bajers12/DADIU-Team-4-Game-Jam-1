
using UnityEngine;

public class Card
{
    public string Title => data.Title;
    public string Description => data.Description;
    public Sprite Sprite => data.CardImage;

    public int ScoreValue => data.ScoreValue;
    private CardData data;
    public Card(CardData cardData)
    {
        data = cardData;

    }
}
