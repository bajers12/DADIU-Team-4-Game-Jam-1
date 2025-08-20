using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.Rendering;
public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private readonly List<CardView> cards = new();
    // HandView.cs (add these two members)
    public int CardsCount => cards.Count;
    public CardView GetCard(int index) => cards[index];

    public IEnumerator AddCard(CardView cardView)
    {
        cards.Add(cardView);
        yield return UpdateCardPosition(0.15f);
    }
    private IEnumerator UpdateCardPosition(float duration)
    {
        if (cards.Count == 0) yield break;

        float cardSpacing = 1f / 10f;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2f;
        Spline spline = splineContainer.Spline;

        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;

            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);

            Quaternion rotation = Quaternion.LookRotation(forward, up);
            rotation *= Quaternion.Euler(0f, -90f, 0f);

            // no tiny Z-fudge needed anymore
            Vector3 worldPos = splinePosition + transform.position;

            cards[i].transform.DOMove(worldPos, duration);
            cards[i].transform.DORotateQuaternion(rotation, duration);

            // right-most on top
            var group = cards[i].GetComponent<SortingGroup>();
            if (group == null) group = cards[i].gameObject.AddComponent<SortingGroup>();
            group.sortingOrder = i;
        }

        yield return new WaitForSeconds(duration);
    }

    public void RemoveCard(CardView card)
    {
        if (cards.Remove(card))
            StartCoroutine(UpdateCardPosition(0.15f));
    }
}
