using UnityEngine;

public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [SerializeField] private CardView cardViewHover;

    public void Show(Card card, Vector3 position)
    {
        cardViewHover.gameObject.SetActive(true);
        cardViewHover.Setup(card);
        cardViewHover.transform.position = position;
        cardViewHover.transform.localScale = Vector3.one * 0.8f;
    }

    public void Hide()
    {
        cardViewHover.gameObject.SetActive(false);
    }

}
