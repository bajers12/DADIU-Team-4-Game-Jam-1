using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.Rendering;

public class HandView : MonoBehaviour
{
    [Header("Layout")]
    [Tooltip("Normalized spacing along the spline parameter [0..1]. Bigger = wider gaps.")]
    [SerializeField] private float spacing = 0.10f;   // <- was 1f/10f

    [Tooltip("Keep cards from running off the ends by shrinking spacing when there are many.")]
    [SerializeField] private bool shrinkToFit = true;

    [Tooltip("Keep a small margin from both spline ends (only used when shrinkToFit is on).")]
    [Range(0f, 0.2f)]
    [SerializeField] private float edgePadding = 0.05f;

    [SerializeField] private SplineContainer splineContainer;

    private readonly List<CardView> cards = new();
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

        float cardSpacing = Mathf.Max(0f, spacing);

        // Optionally compress spacing so we stay within [edgePadding, 1 - edgePadding]
        if (shrinkToFit && cards.Count > 1)
        {
            float maxSpan = Mathf.Max(0f, 1f - 2f * edgePadding);
            float neededSpan = (cards.Count - 1) * cardSpacing;
            if (neededSpan > maxSpan)
                cardSpacing = maxSpan / (cards.Count - 1);
        }

        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2f;
        Spline spline = splineContainer.Spline;

        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            // Keep inside [0,1] just in case
            p = Mathf.Clamp01(p);

            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);

            Quaternion rotation = Quaternion.LookRotation(forward, up) * Quaternion.Euler(0f, -90f, 0f);

            Vector3 worldPos = splinePosition + transform.position;

            cards[i].transform.DOMove(worldPos, duration);
            cards[i].transform.DORotateQuaternion(rotation, duration);

            var group = cards[i].GetComponent<SortingGroup>();
            if (group == null) group = cards[i].gameObject.AddComponent<SortingGroup>();
            group.sortingOrder = i + 11; // right-most on top
        }

        yield return new WaitForSeconds(duration);
    }

    public void RemoveCard(CardView card)
    {
        if (cards.Remove(card))
            StartCoroutine(UpdateCardPosition(0.15f));
    }
}
