using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

public class ChosenView : MonoBehaviour
{
    [Header("Layout")]
    [Tooltip("Normalized spacing along the spline parameter [0..1]. Bigger = wider gaps.")]
    [SerializeField] private float spacing = 0.12f;

    [Tooltip("If there are many picks, spacing auto-compresses to fit between paddings.")]
    [SerializeField] private bool shrinkToFit = true;

    [Range(0f, 0.2f)]
    [SerializeField] private float edgePadding = 0.04f;

    [Header("Refs")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private CardViewCreator creator;

    private readonly List<CardView> cards = new();
    public IEnumerator AddCard(Card card)
    {
        // Spawn near the view root; creator already scales 0->1
        var cv = creator.CreateCardView(card, transform.position, Quaternion.identity);
        cards.Add(cv);
        yield return UpdatePositions(0.15f);
    }

    public IEnumerator ClearAll(float duration = 0.12f)
    {
        if (cards.Count == 0) yield break;

        var snapshot = new List<CardView>(cards);
        cards.Clear();

        float t = 0f;
        const float stagger = 0.03f;
        foreach (var c in snapshot)
        {
            var s = DOTween.Sequence();
            s.AppendInterval(t);
            s.Append(c.transform.DOScale(0f, duration));
            s.OnComplete(() => Destroy(c.gameObject));
            t += stagger;
        }
        yield return new WaitForSeconds(t + duration + 0.01f);
    }

    private IEnumerator UpdatePositions(float duration)
    {
        if (cards.Count == 0) yield break;

        float cardSpacing = Mathf.Max(0f, spacing);

        if (shrinkToFit && cards.Count > 1)
        {
            float maxSpan = Mathf.Max(0f, 1f - 2f * edgePadding);
            float need = (cards.Count - 1) * cardSpacing;
            if (need > maxSpan) cardSpacing = maxSpan / (cards.Count - 1);
        }

        float firstP = edgePadding; // left aligned
        var spline = splineContainer.Spline;

        for (int i = 0; i < cards.Count; i++)
        {
            float p = Mathf.Clamp01(firstP + i * cardSpacing);

            Vector3 pos = spline.EvaluatePosition(p);
            Vector3 fwd = spline.EvaluateTangent(p);
            Vector3 up  = spline.EvaluateUpVector(p);

            Quaternion rot = Quaternion.LookRotation(fwd, up) * Quaternion.Euler(0f, -90f, 0f);
            Vector3 worldPos = pos + transform.position;

            cards[i].transform.DOMove(worldPos, duration);
            cards[i].transform.DORotateQuaternion(rot, duration);

            // Right-most on top for nice overlaps
            var group = cards[i].GetComponent<SortingGroup>() ?? cards[i].gameObject.AddComponent<SortingGroup>();
            group.sortingOrder = i;
        }

        yield return new WaitForSeconds(duration);
    }
}
