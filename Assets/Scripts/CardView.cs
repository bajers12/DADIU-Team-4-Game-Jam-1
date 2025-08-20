// CardView.cs
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text value;
    [SerializeField] private SpriteRenderer cardImage;
    [SerializeField] private GameObject wrapper;

    public Card Card { get; private set; }

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        value.text = card.ScoreValue.ToString();
        cardImage.sprite = card.Sprite;
    }

    // NEW: keyboard “hover” toggles the in-hand visuals
    public void SetHovered(bool hovered)
    {
        if (wrapper != null) wrapper.SetActive(!hovered);
    }
}
